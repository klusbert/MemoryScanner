using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Addresses.Player
{
    public class GotoX : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public GotoX(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
                return "GotoX";
            }
        }

        public override void Search()
        {
            byte[] SearchBytes = new byte[] { 0x03, 0xC7, 0x89, 0x85, 0xF8, 0xFE, 0xFF, 0xFF };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                int gotxAddress = memRead.ReadInt32(values[0] + 10);
                int gotoYAddress = memRead.ReadInt32(values[0] + 18);
                int gotoZAddress = memRead.ReadInt32(values[0] + 29);

                MyAddresses.Gotoy.Address = gotoYAddress;
                MyAddresses.Gotoz.Address = gotoZAddress;
                m_address = gotxAddress;
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
