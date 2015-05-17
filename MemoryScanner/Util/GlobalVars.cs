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
        public static int AttackCountRegion = 0;
        public static int MapRegion = 0;
        public static int PlayerVarsRegion = 0;
        public static int XorKey = 0;
        public static int Mana = 0;
        public static int PlayerId;

        public static int PlayerX = 0;
        public static int PlayerY = 0;
        public static int PlayerZ = 0;
        public static int Redsquare = 0;
        public static int ParseFunction = 0;
        public static int GetNextPacket = 0;
        public static int ReciveStream = 0;
        public static int ConnectionStatus = 0;
        public static int PrintTextFunc = 0;
        public static int PrintFPS = 0;
        public static int ShowFPS = 0;
        public static int MaxCreatures = 0;
        public static int StepCreatures = 0;
        public static int NopFPS = 0;
        public static int SendPointer = 0;
        public static int StatusBarTime = 0;
        public static int StepTile = 0;
        public static int XteaKey = 0;
        public static int OutGoingBufferLen = 0;
        public static int OutGoingBuffer = 0;

        public static int AddBytePacket = 0;
        public static int CreatePacket = 0;
        public static int SendPacket = 0;
        


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
