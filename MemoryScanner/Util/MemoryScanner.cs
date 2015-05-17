using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MemoryScanner{
      
   public class MemoryScanner
   {
       #region imports
      
 
       #endregion
        public Process Process;
        public byte[] Buffer;
        public int MemoryStarts;
        public int MemoryEnds;
        public int BaseAddress;
        public MemoryScanner(Process _process)
        {
            Process = _process;
            StoreMemoryBuffer();
          
        }
       public void StoreMemoryBuffer()
        {
           ProcessModule module = Process.MainModule;
           Buffer = new byte[module.ModuleMemorySize];
           IntPtr numberOfBytesRead;
           WinApi.ReadProcessMemory(Process.Handle, module.BaseAddress, Buffer, (uint)module.ModuleMemorySize, out numberOfBytesRead);
           MemoryStarts = module.BaseAddress.ToInt32();
           MemoryEnds = MemoryStarts + module.ModuleMemorySize;
           BaseAddress = (int)module.BaseAddress;
        }
       public List<int> ScanBytes(byte[] value)
       {
           List<int> result = new List<int>();
           int len = value.Length;
           int end = Buffer.Length - len;
           for (int i = 0; i < end; ++i)
           {
               int j = 0;
               for (; j < len && Buffer[i + j] == value[j]; ++j) ;
               if (j == len)
               {
                   result.Add(MemoryStarts + i);
               }
           }
           return result;

       }
       public List<int> ScanInt16(short value)
       {
           return ScanBytes(BitConverter.GetBytes(value));
       }
       public List<int> ScanInt32(int value)
       {
           return ScanBytes(BitConverter.GetBytes(value));
       }
       public List<int> ScanString(string value)
       {
           byte[] bytes = System.Text.ASCIIEncoding.Default.GetBytes(value);
           return ScanBytes(bytes);
       }
       private byte[] GetData(int offset,int len)
       {
           byte[] data = new byte[len];
           Array.Copy(Buffer, offset, data, 0, len);
           return data;

       }
     
    }
}
