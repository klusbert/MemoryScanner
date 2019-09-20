using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    public class ShowFPS : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public ShowFPS(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
                return "ShowFPS";
            }
        }
        public override void Search()
        {
        
            byte[] SearchBytes = new byte[] { 0x8B, 0x01, 0xFF, 0x71, 0x18, 0xFF, 0x71, 0x14, 0x6A, 0x01, 0xFF, 0x50, 0x4C };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                values[0] += SearchBytes.Length + 2;
                m_address = memRead.ReadInt32(values[0]);         
          
                MyAddresses.NopFPS.Address = values[0] + 5;
               
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
