using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Util
{
    public static class GlobalVars
    {
     

        public static string FullPath = "";
        public static bool ShowWithBase = true;
    


        public static int FunctionDistance(int func1,int func2)
        {
            int offset = func1 - func2;
            return Math.Abs(offset);
        }
        public static bool CompareByteArrays(byte[] ary1, byte[] ary2)
        {
            if (ary1.Length != ary2.Length)
                return false;
            for (int i = 0; i < ary1.Length; i++)
                if (ary1[i] != ary2[i])
                    return false;

            return true;
        }
        public static int SearchForFunctionStart(MemoryReader memRead,int adr)
        {
                byte[] SearchBytes = new byte[] { 0xCC, 0x55 };
                int i = adr;
           
                do
                {
                    byte[] value = memRead.ReadBytes(i, 2);
                    if(CompareByteArrays(value,SearchBytes))
                    {
                        return i +1;
                        
                    }
                    i -= 1;
                    
                } while (true);
        }
    }
}
