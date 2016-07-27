using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MemoryScanner.Tests
{
    public class PacketListnerClient
    {
        public delegate void PacketListnerDel(byte[] data);
        public event PacketListnerDel OutGoingPacket;
        private MemoryReader memRead;
        private IntPtr TibiaHandle;
        public IntPtr GotPacketAdr;
        private IntPtr CodeCaveAdr;
        private bool running = false;
        private byte[] OrigalBytes;
        public bool IsRunning = false;
        public PacketListnerClient(MemoryReader _memread)
        {
            memRead = _memread;
            TibiaHandle = _memread.Handle;          
        }
        public void SetUpCodeCave()
        {
            
            CodeCaveHelper cv = new CodeCaveHelper();
            //Let's get some space for our codecave         
            CodeCaveAdr = WinApi.VirtualAllocEx(TibiaHandle, IntPtr.Zero, 1024, WinApi.AllocationType.Commit | WinApi.AllocationType.Reserve, WinApi.MemoryProtection.ExecuteReadWrite);
            GotPacketAdr = WinApi.VirtualAllocEx(TibiaHandle, IntPtr.Zero, 1, WinApi.AllocationType.Commit | WinApi.AllocationType.Reserve, WinApi.MemoryProtection.ExecuteReadWrite);

            memRead.WriteByte(GotPacketAdr.ToInt32(), 0);
          
            cv.AddLine((byte)0x8b, (byte)0xd8); // store eax

            cv.AddLine((byte)0xc7, (byte)0x05, (UInt32)GotPacketAdr.ToInt32(), (UInt32)0x00000001); //sets gotpacket to 1

            cv.AddLine((byte)0x90);
            cv.AddLine((byte)0xA1, (UInt32)GotPacketAdr.ToInt32());

            cv.AddLine((byte)0x83, (byte)0xF8, (byte)1);
            cv.AddLine((byte)0x74, (byte)0xF6);
            cv.AddLine((byte)0x8b, (byte)0xC3);
        //    cv.AddLine((byte)0x50); 
            cv.AddLine((byte)0xE8);
            //cv.AddInt(((int)Addresses.MyAddresses.SendPacket.Address +17 - (CodeCaveAdr.ToInt32()) - 5) -cv.Data.Length +1);  // calls getnextPacket
            cv.AddInt(((int)Addresses.MyAddresses.SendPacket.Address  - (CodeCaveAdr.ToInt32()) - 5) - cv.Data.Length + 1);  // calls getnextPacke
            cv.AddLine((byte)0xC3);

           
            memRead.WriteBytes(CodeCaveAdr.ToInt32(), cv.Data, (uint)cv.Data.Length);
            Thread t = new Thread(new ThreadStart(ReadingPacket));
            running = true;
            t.Start();
            //ReplaceCode();
            IsRunning = true;
            System.Windows.Forms.Clipboard.SetText(CodeCaveAdr.ToString("X"));
        }
        private void ReplaceCode()
        {
            CodeCaveHelper cv = new CodeCaveHelper();
            cv.AddLine((byte)0xE8);
            cv.AddInt(((int)(CodeCaveAdr.ToInt32() - Addresses.MyAddresses.SendPacket.Address + 17 - 5)));
           // OrigalBytes = memRead.ReadBytes(Addresses.MyAddresses.SendPacket.Address + 3,(uint)cv.Data.Length);
            memRead.WriteBytes(Addresses.MyAddresses.SendPacket.Address + 3, cv.Data, (uint)cv.Data.Length);
          
         
        }
        public void CleanUp()
        {

            if (running == false) { return; }
            running = false;
            memRead.WriteBytes(Addresses.MyAddresses.SendPacket.Address + 3, OrigalBytes, (uint)OrigalBytes.Length);
            memRead.WriteByte(GotPacketAdr.ToInt32(), 0);
            WinApi.VirtualFreeEx(TibiaHandle, CodeCaveAdr, 1024, WinApi.AllocationType.Release);
            WinApi.VirtualFreeEx(TibiaHandle, GotPacketAdr, 1, WinApi.AllocationType.Release);
        }
        private void ReadingPacket()
        {
            while (running)
            {
                if (memRead.ReadByte(GotPacketAdr.ToInt32()) == 1)
                {
                    byte packetLen = memRead.ReadByte(Addresses.MyAddresses.OutGoingPacketLen.Address);
                     byte[] packet =   memRead.ReadBytes(Addresses.MyAddresses.OutGoingBuffer.Address +8,(uint)packetLen );
                     if (OutGoingPacket != null)
                     {
                         OutGoingPacket.Invoke(packet);
                     }                 
                    memRead.WriteByte(GotPacketAdr.ToInt32(), 0);
                }
            }
        }
    }
}
