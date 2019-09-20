using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Addresses
{
    public class StatusBarText : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public StatusBarText(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
                return "StatusBarText";
            }
        }
        public override void Search()
        {
          
            byte[] SearchBytes = new byte[] { 0x2B, 0xCE, 0x8B, 0xFF, 0x8A, 0x06, 0x88, 0x04, 0x31, 0x8D };
            // List<int> values = memScan.ScanString("Sorry, not possible.");
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                
                m_address = memRead.ReadInt32(values[0] - 4);

               // MyAddresses.StatusBarTime.Address = m_address - 12;
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
