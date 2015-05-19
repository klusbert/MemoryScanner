using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Tests
{
    class SendPacket
    {
        public IntPtr tProcessHandle { get; set; }
        public Process tProcess { get; set; }
        public MemoryReader memRead { get; set; }

        public SendPacket(MemoryReader _memread)
        {
            Process _process = _memread.Process;
            tProcessHandle = _process.Handle;
            tProcess = _process;
            memRead = _memread;
        }
        private int m_tibiaThread = 0;

        private int GetMainThread()
        {
            if (m_tibiaThread > 0) { return m_tibiaThread; }    

            ProcessThreadCollection Threads = tProcess.Threads;
            if (Threads != null)
            {
                foreach (ProcessThread pT in Threads)
                {
                    if (pT.StartAddress == IntPtr.Zero)
                    {

                        m_tibiaThread = pT.Id;
                        break;
                    }
                }
            }
            return m_tibiaThread;
        }
        private IntPtr OpenAndSuspendThread(int threadID)
        {

            IntPtr pOpenThread = default(IntPtr);

            pOpenThread = WinApi.OpenThread((WinApi.ThreadAccess.GET_CONTEXT | WinApi.ThreadAccess.SUSPEND_RESUME | WinApi.ThreadAccess.SET_CONTEXT), false, Convert.ToUInt32(GetMainThread()));
            WinApi.SuspendThread(pOpenThread);
            return pOpenThread;
        }
        private void ResumeAndCloseThread(IntPtr thread)
        {
            WinApi.ResumeThread(thread);
            WinApi.CloseHandle(thread);
        }

        public void SendPacketToServer(byte[] packet)
        {
            CodeCaveHelper cv = new CodeCaveHelper();
            IntPtr MainThread = OpenAndSuspendThread(tProcess.Id);
            uint OldPackelen = memRead.ReadUInt32(Addresses.MyAddresses.OutGoingPacketLen.Address);
            byte[] OldPacket = memRead.ReadBytes(Addresses.MyAddresses.OutGoingBuffer.Address, OldPackelen);
            IntPtr CodeCave = WinApi.VirtualAllocEx(tProcessHandle, IntPtr.Zero, 1024, WinApi.AllocationType.Commit | WinApi.AllocationType.Reserve, WinApi.MemoryProtection.ExecuteReadWrite);


            //createPacket
            byte packetType = (byte)packet[0];
            cv.AddLine((byte)0xb9, (UInt32)packetType);
            cv.AddLine((byte)0xB8, (uint)Addresses.MyAddresses.CreatePacket.Address);
            cv.AddLine((byte)0xff, (byte)0xD0);

            for (int i = 1; i < packet.Length; i++)
            {
                byte val = packet[i];

                cv.AddLine((byte)0xb9, (UInt32)val);
                cv.AddLine((byte)0xB8, (uint)Addresses.MyAddresses.AddPacketByte.Address);
                cv.AddLine((byte)0xff, (byte)0xD0);

            }

            cv.AddLine((byte)0xb1, (byte)0x01); //push 1 as bool( using Xtea encrypt or not
            cv.AddLine((byte)0xB8, (uint)Addresses.MyAddresses.SendPacket.Address);
            cv.AddLine((byte)0xff, (byte)0xD0); // call eax Thanks Darkstar

            cv.AddByte(0xC3);//ret
            System.Windows.Forms.Clipboard.SetText(CodeCave.ToString("X"));
      
            memRead.WriteBytes(CodeCave.ToInt32(), cv.Data, (uint)cv.Data.Length);
         
            IntPtr hThread = WinApi.CreateRemoteThread(tProcessHandle, IntPtr.Zero, 0, CodeCave, IntPtr.Zero, 0, IntPtr.Zero);
            WinApi.WaitForSingleObject(hThread, 0xFFFFFFFF);
            WinApi.CloseHandle(hThread);
            WinApi.VirtualFreeEx(tProcessHandle, CodeCave, 1024, WinApi.AllocationType.Release);         
   
            memRead.WriteUInt32(Addresses.MyAddresses.OutGoingPacketLen.Address, OldPackelen);
            memRead.WriteBytes(Addresses.MyAddresses.OutGoingBuffer.Address, OldPacket, (uint)OldPackelen);

            ResumeAndCloseThread(MainThread);
        }

    }
}
