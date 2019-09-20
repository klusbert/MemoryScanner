using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Addresses
{
    public class PeekMessageA : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;

        public PeekMessageA(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
        public override int Address
        {
            get
            {
                return m_address;
            }
            set
            {
                m_address = value;
            }
        }
        public override string Name
        {
            get
            {
                return "PeekMessage";
            }
        }
        public override void Search()
        {
            byte[] SearchBytes = new byte[] { 0x6A, 0x03, 0xB8, 0x67, 0x03, 0x00, 0x00, 0x50, 0x50, 0xFF, 0x76, 0x20, 0x8D, 0x45, 0xE4, 0x50, 0xFF, 0x15 };
            List<int> values = memScan.ScanBytes(SearchBytes);
          

            if (values.Count > 0)
            {
                values[0] += SearchBytes.Length;
                m_address = memRead.ReadInt32(values[0]);
            }
        }
        public override string GetString()
        {
            int val = 0;
            if (m_address == 0)
            {
                Search();
            }
            if (!Util.GlobalVars.ShowWithBase)
            {
                val = Address - memScan.BaseAddress;
            }
            else
            {
                val = Address;
            }
            return Name + " = 0x" + val.ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
