using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    class RedSquare:GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        public RedSquare(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
                return Util.GlobalVars.Redsquare - memScan.BaseAddress;
            }
            return Util.GlobalVars.Redsquare;
        }
        public override string GetString()
        {
            int adr = this.GetAddress();
            return "RedSquare = 0x" + adr.ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
