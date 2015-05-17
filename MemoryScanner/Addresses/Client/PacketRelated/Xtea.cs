using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Addresses
{
    class Xtea:GetAddresses
    {
          MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        public Xtea(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
        {
            this.memRead = _memRead;
            this.memScan = _memScan;
            this.Type = _type;
        }
        public override GetAddresses.AddressType AddressCategory
        {
            get
            {
                return Type;
            }

        }
        public override int GetAddress()
        {

            if (!Util.GlobalVars.ShowWithBase)
            {
                return Util.GlobalVars.XteaKey - memScan.BaseAddress;
            }
            return Util.GlobalVars.XteaKey;       
        }
        public override string GetString()
        {
            return "XTEA = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
