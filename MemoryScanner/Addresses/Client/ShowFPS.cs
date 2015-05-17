using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    class ShowFPS:GetAddresses
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
        public override int GetAddress()
        {
            if (m_address > 0)
            {
                return m_address;
            }
            byte[] SearchBytes = new byte[] { 0x8B, 0x01, 0xFF, 0x71, 0x18, 0xFF, 0x71, 0x14, 0x6A, 0x01, 0xFF, 0x50, 0x4C };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                values[0] += SearchBytes.Length + 2;
                m_address = memRead.ReadInt32(values[0]);              
                Util.GlobalVars.NopFPS = values[0] + 5;
                Util.GlobalVars.ShowFPS = m_address;
               
            }
            if (!Util.GlobalVars.ShowWithBase)
            {
                return m_address - memScan.BaseAddress;
            }
            return m_address;
        }
        public override string GetString()
        {
            return "ShowFPS = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
