using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    class FullLight:GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public FullLight(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
            byte[] SearchBytes = new byte[] { 0x3D, 0xFF, 0x00, 0x00, 0x00, 0x0F, 0x4F, 0xC1, 0x53, 0x8B, 0x1D };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {              
                m_address = values[0] + 9;
             
            }
            if (!Util.GlobalVars.ShowWithBase)
            {
                return m_address - memScan.BaseAddress;
            }
            return m_address;
        }
        public override string GetString()
        {
            return "MapFullLight = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
