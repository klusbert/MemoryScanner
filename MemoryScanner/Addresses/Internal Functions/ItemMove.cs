using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Addresses
{
    public class ItemMove : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public ItemMove(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
        {
            this.memRead = _memRead;
            this.memScan = _memScan;
            this.Type = _type;
        }
        public override AddressType AddressCategory
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
                return "ItemMoveFunction";
            }
        }
        public override void Search()
        {

            byte[] SearchBytes = new byte[] { 0xB9, 0x78, 0x00, 0x00, 0x00, 0xE8 };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if(values.Count > 0)
            {
                int i = memRead.GetFunctionStart(values[0]);
                m_address = i;
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
