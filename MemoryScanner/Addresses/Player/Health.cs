using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    class Health :GetAddresses 
    {

        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public Health(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
            if (m_address > 0)
            {
                return m_address;
            }
            List<int> values = memScan.ScanString("Hit Points");
            if(values.Count > 0)
            {
                values = memScan.ScanInt32(values[0]);
                Util.GlobalVars.XorKey  = memRead.ReadInt32(values[0] + 14);
                Util.GlobalVars.Mana = memRead.ReadInt32(values[0] + 50);
                m_address  = memRead.ReadInt32(values[0] + 20);
              
            }
            if (!Util.GlobalVars.ShowWithBase)
            {
                return m_address - memScan.BaseAddress;
            }
            return m_address;
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
