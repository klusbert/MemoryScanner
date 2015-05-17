using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    class Mana:GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        public Mana(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
              return  Util.GlobalVars.Mana - memScan.BaseAddress;
            }
            return Util.GlobalVars.Mana;
        }
        public override string GetString()
        {
            int adr = this.GetAddress();
        
            return "HitPoints = 0x" + adr.ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }

    }
}
