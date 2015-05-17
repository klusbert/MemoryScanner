using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Addresses
{
    class WalkFunction : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public WalkFunction(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
            byte[] SearchBytes = new byte[] { 0x6A, 0x01, 0x6A, 0xFF, 0x6A, 0xFF, 0xE8 };//push 1 push -1 push -1
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                int adr = memRead.GetCallFunction(values[0] + 6);
                m_address = adr;
               
            }
            if (!Util.GlobalVars.ShowWithBase)
            {
                return m_address - memScan.BaseAddress;
            }
            return m_address;
        }
        public override string GetString()
        {
            return "WalkFunction = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
