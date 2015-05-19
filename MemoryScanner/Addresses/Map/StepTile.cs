using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Addresses
{
    public class StepTile : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public StepTile(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
        public override void Search()
        {
            return;
        }
        public override string GetString()
        {
            int val = 0;
            if (m_address == 0)
            {
                Search();
            }
            val = Address;                
            return "StepTile = 0x" + val.ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
