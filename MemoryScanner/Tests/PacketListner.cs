using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace MemoryScanner.Tests
{
    class PacketListner
    {
        public delegate void PacketListnerDel(byte[] data);
        public event PacketListnerDel IncommingPacket;

        private MemoryReader memRead;
        private IntPtr TibiaHandle;
        private IntPtr GotPacketAdr;
        private IntPtr CodeCaveAdr;
        private bool running = false;
        private int origanGetNextPacket = 0;
        private NetWorkStream networkStream;
  
        public bool SplitPackets = true;

        public PacketListner(MemoryReader _memread)
        {
            memRead = _memread;
            TibiaHandle = _memread.Handle;
            networkStream = new NetWorkStream(_memread);       
        }
        public void SetUpCodeCave()
        {
            CodeCaveHelper cv = new CodeCaveHelper();
            //Let's get some space for our codecave
            origanGetNextPacket = memRead.GetCallFunction(Addresses.MyAddresses.GetnextPacket.Address);
            CodeCaveAdr = WinApi.VirtualAllocEx(TibiaHandle, IntPtr.Zero, 1024, WinApi.AllocationType.Commit | WinApi.AllocationType.Reserve, WinApi.MemoryProtection.ExecuteReadWrite);
            GotPacketAdr = WinApi.VirtualAllocEx(TibiaHandle, IntPtr.Zero, 1, WinApi.AllocationType.Commit | WinApi.AllocationType.Reserve, WinApi.MemoryProtection.ExecuteReadWrite);

            memRead.WriteByte(GotPacketAdr.ToInt32(), 0);
            cv.AddLine((byte)0xE8);
            cv.AddInt(((int)origanGetNextPacket - (CodeCaveAdr.ToInt32()) - 5));  // calls getnextPacket


            cv.AddLine((byte)0x8b, (byte)0xd8); // store eax

            cv.AddLine((byte)0xc7, (byte)0x05, (UInt32)GotPacketAdr.ToInt32(), (UInt32)0x00000001); //sets gotpacket to 1

            cv.AddLine((byte)0x90);
            cv.AddLine((byte)0xA1, (UInt32)GotPacketAdr.ToInt32());

            cv.AddLine((byte)0x83, (byte)0xF8, (byte)1);
            cv.AddLine((byte)0x74, (byte)0xF6);
            cv.AddLine((byte)0x8b, (byte)0xC3);
            cv.AddLine((byte)0xC3);

            System.Windows.Forms.Clipboard.SetText(CodeCaveAdr.ToString("X"));
       
            memRead.WriteBytes(CodeCaveAdr.ToInt32(), cv.Data, (uint)cv.Data.Length);
            Thread t = new Thread(new ThreadStart(ReadingPacket));
            running = true;
            t.Start();
            ReplaceCode();        


        }
        private void ReplaceCode()
        {
            CodeCaveHelper cv = new CodeCaveHelper();
            cv.AddLine((byte)0xE8);
            cv.AddInt(((int)(CodeCaveAdr.ToInt32() - Addresses.MyAddresses.GetnextPacket.Address) - 5));
        
            memRead.WriteBytes(Addresses.MyAddresses.GetnextPacket.Address, cv.Data, (uint)cv.Data.Length);
        }
        public void CleanUp()
        {
            if (running == false) { return; }
            running = false;
         
            CodeCaveHelper cv = new CodeCaveHelper();
            cv.AddLine((byte)0xE8);
            cv.AddInt(((int)(origanGetNextPacket - Addresses.MyAddresses.GetnextPacket.Address) - 5));
    
            memRead.WriteBytes(Addresses.MyAddresses.GetnextPacket.Address, cv.Data, (uint)cv.Data.Length);
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
                    if (networkStream.Postion == 8)
                    {
                        //got a new packet 
                        if (IncommingPacket != null)
                        {
                            IncommingPacket.Invoke(networkStream.Data);
                        }                       
                
                    }
             
                    memRead.WriteByte(GotPacketAdr.ToInt32(), 0);
                }               
                System.Threading.Thread.Sleep(1);
            }
        } 
           
  
        class NetWorkStream
        {
            private MemoryReader memRead;
            public NetWorkStream(MemoryReader _memRead)
            {
                memRead = _memRead;
            }
            public int Lenght
            {
                get { return memRead.ReadInt32(Addresses.MyAddresses.ReciveStream.Address + 4); }
            }
            public int Postion
            {
                get { return memRead.ReadInt32(Addresses.MyAddresses.ReciveStream.Address + 8) - 1; }
            }
            public byte[] Data
            {
                get
                {
                    byte[] packet = new byte[Lenght];
                    uint streamPointer = memRead.ReadUInt32(Addresses.MyAddresses.ReciveStream.Address);
                    packet = memRead.ReadBytes(streamPointer +8, (uint)Lenght);
                    return packet;
                }
            }
        }
    }
}
