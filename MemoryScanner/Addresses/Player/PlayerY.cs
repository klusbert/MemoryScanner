using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    class PlayerY:GetAddresses
    {

        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        public PlayerY(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
                return Util.GlobalVars.PlayerY - memScan.BaseAddress;
            }
            return Util.GlobalVars.PlayerY;
        }
        public override string GetString()
        {
            int adr = this.GetAddress();
            return "Y = 0x" + adr.ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
