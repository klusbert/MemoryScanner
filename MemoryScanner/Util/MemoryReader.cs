using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner
{
  public class MemoryReader
    {
        public Process Process;
        public IntPtr Handle;
        public MemoryReader(Process _process)
        {
            Process = _process;
            Handle = _process.Handle;

        }
        public  bool WriteBytes(long address, byte[] bytes, uint length)
        {
            IntPtr bytesWritten;

            // Write to memory
            int result = WinApi.WriteProcessMemory(Handle, new IntPtr(address), bytes, length, out bytesWritten);

            return result != 0;
        }
        public bool WriteByte(long address,byte value)
        {
            return WriteBytes( address, new byte[] { value }, 1);
        }
        public  bool WriteInt32(long address, int value)
        {
            return WriteBytes(address, BitConverter.GetBytes(value), 4);
        }
        public  bool WriteUInt32( long address, uint value)
        {
            return WriteBytes( address, BitConverter.GetBytes(value), 4);
        }
        public  bool WriteString( long address, string str)
        {
            str += '\0';
            byte[] bytes = System.Text.ASCIIEncoding.Default.GetBytes(str);
            return WriteBytes( address, bytes, (uint)bytes.Length);
        }

        public  byte[] ReadBytes(long address, uint bytesToRead)
        {
            IntPtr ptrBytesRead;
            byte[] buffer = new byte[bytesToRead];

            WinApi.ReadProcessMemory(Handle, new IntPtr(address), buffer, bytesToRead, out ptrBytesRead);

            return buffer;
        }
        public  byte ReadByte( long address)
        {
            return ReadBytes(address, 1)[0];
        }
        public  short ReadInt16( long address)
        {
            return BitConverter.ToInt16(ReadBytes( address, 2), 0);
        }
        public  int ReadInt32( long address)
        {
            return BitConverter.ToInt32(ReadBytes( address, 4), 0);
        }
        public  uint ReadUInt32(long address)
        {
            return BitConverter.ToUInt32(ReadBytes( address, 4), 0);
        }
         public string ReadString(long address)
        {
            string s = "";
            byte temp = ReadByte( address++);
            while (temp != 0)
            {
                s += (char)temp;
                temp = ReadByte( address++);
            }
            return s;
        }
        public int GetCallFunction(long address)
        {
            int offset = this.ReadInt32(address +1);
            return (int)address + offset + 5;
        }
   
      public int GetFunctionStart(long address)
        {
            byte[] SearchBytes = new byte[] { 0xCC, 0x55  };
            long adr = address;
          while(true)
          {
              byte[] val = ReadBytes(adr, 2);
              if (Enumerable.SequenceEqual(SearchBytes,val))
              {
                  return (int)adr +1;
              }
              adr -= 1;
          }
            return 0;

        }
    }
}
