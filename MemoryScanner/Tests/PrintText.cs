using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Tests
{
    class PrintText
    {
        MemoryReader memRead;
        IntPtr TibiaHandle;
        public PrintText(MemoryReader _memread)
        {
            memRead = _memread;
            TibiaHandle = memRead.Handle;

        }
        public void CreateCodeCave(int r, int g, int b, int x, int y, byte font, string text,string name)
        {
            byte[] bytes = System.Text.ASCIIEncoding.Default.GetBytes(text);
            IntPtr stringAdr = WinApi.VirtualAllocEx(TibiaHandle, IntPtr.Zero, (uint)bytes.Length, WinApi.AllocationType.Commit | WinApi.AllocationType.Reserve, WinApi.MemoryProtection.ExecuteReadWrite);
                    
            memRead.WriteBytes(stringAdr.ToInt32(), bytes, (uint)bytes.Length);

            CodeCaveHelper cv = new CodeCaveHelper();
            cv.AddLine((byte)0x6A, (byte)0x00); //push 0
            cv.AddLine((byte)0x68, (UInt32)stringAdr.ToInt32()); //mov ecx string

            cv.AddLine((byte)0x68, (UInt32)b); //push blue int32

            cv.AddLine((byte)0x68, (UInt32)g); //push green int32

            cv.AddLine((byte)0x68, (UInt32)r); //push red int32

            cv.AddLine((byte)0x6A, (byte)font); //push font byte

            cv.AddLine((byte)0x68, (UInt32)y); //push x int32

            cv.AddLine((byte)0xBA, (UInt32)x); //push y int32

            cv.AddLine((byte)0xb9, (UInt32)0x1); //push 1                      

            cv.AddLine((byte)0xB8, (UInt32)Util.GlobalVars.PrintTextFunc); // mov eax dword PrintName

            cv.AddLine((byte)0xff, (byte)0xD0); // call eax Thanks Darkstar

            cv.AddLine((byte)0x83, (byte)0xc4, (byte)0x1c); //add esp,20
            cv.AddByte(0xC3);

            IntPtr CaveAddress = WinApi.VirtualAllocEx(TibiaHandle, IntPtr.Zero, (uint)cv.Data.Length, WinApi.AllocationType.Commit | WinApi.AllocationType.Reserve, WinApi.MemoryProtection.ExecuteReadWrite);
            memRead.WriteBytes(CaveAddress.ToInt32(), cv.Data, (uint)cv.Data.Length);
            ChangePrintFpsCall(CaveAddress);
            System.Windows.Forms.Clipboard.SetText(CaveAddress.ToString("X"));
                    
        }
        private void ChangePrintFpsCall(IntPtr CaveAddress)
        {
            byte[] ReplaceBytes = new byte[]{
                  0xE8, 0xff, 0xff, 0xff,0xff,

            };
            Int32 offset2 = CaveAddress.ToInt32() - (int)Util.GlobalVars.PrintFPS - 5;

            Array.Copy(BitConverter.GetBytes(offset2), 0, ReplaceBytes, 1, 4);
          
            memRead.WriteBytes(Util.GlobalVars.PrintFPS, ReplaceBytes, (uint)ReplaceBytes.Length);
            memRead.WriteByte(Util.GlobalVars.ShowFPS, 1);
        }

    }
}
