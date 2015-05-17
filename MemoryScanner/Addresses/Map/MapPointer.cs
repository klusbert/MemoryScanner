using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    class MapPointer : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public MapPointer(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
            byte[] SearchBytes = new byte[] { 0xFF, 0x70, 0xFC, 0x8D, 0x70, 0xFC, 0x68, 0x70, 0x01, 0x00, 0x00, 0x50, 0xE8 };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                Util.GlobalVars.MapRegion = values[0];
                m_address = memRead.ReadInt32(values[0] - 14);
                Util.GlobalVars.StepTile = memRead.ReadInt32(values[0] + 7);
            }
            if (!Util.GlobalVars.ShowWithBase)
            {
                return m_address - memScan.BaseAddress;
            }
            return m_address;
        }
        public override string GetString()
        {
            return "MapPointer = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
