using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    public class MapPointer : GetAddresses
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
                return "MapPointer";
            }
        }
        public override void Search()
        {            
            byte[] SearchBytes = new byte[] { 0xFF, 0x70, 0xFC, 0x8D, 0x70, 0xFC, 0x68, 0x70, 0x01, 0x00, 0x00, 0x50, 0xE8 };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
               
                m_address = memRead.ReadInt32(values[0] - 14);
                MyAddresses.StepTile.Address = memRead.ReadInt32(values[0] + 7);
                MyAddresses.MapArray.Address = memRead.ReadInt32(values[0] + 35);         
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
