using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Addresses
{
    public class StatusBarTime : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;

        public StatusBarTime(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
                return "StatusBarTime";
            }
        }
        public override void Search()
        {
            byte[] SearchBytes = new byte[] { 0xC7, 0x45, 0xFC, 0xFF, 0xFF, 0xFF, 0xFF, 0xC7, 0x85, 0x90, 0xFB, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0xC7, 0x45, 0xFC, 0x0B, 0x00, 0x00, 0x00, 0x83, 0x3D };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                // got it
                int adr = memRead.ReadInt32(values[0] + SearchBytes.Length);
                m_address = adr;
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
