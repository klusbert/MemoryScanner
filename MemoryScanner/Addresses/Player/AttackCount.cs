using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    class AttackCount : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public AttackCount(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
            byte[] SearchBytes = new byte[] { 0xB9, 0xA1, 0x00,0x00, 0x00, 0xE8 };     
            List<int> values = memScan.ScanBytes(SearchBytes);//MOV ECX,0A1
            if(values.Count > 0)
            {
                Util.GlobalVars.AttackCountRegion = values[0];
                m_address = memRead.ReadInt32(values[0] - 4);
               
            }
            if (!Util.GlobalVars.ShowWithBase)
            {
                return m_address - memScan.BaseAddress;
            }
            return m_address;
        }
        public override string GetString()
        {
            return "AttackCount = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
