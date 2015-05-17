using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    class BlistStart:GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public BlistStart(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
            byte[] SearchBytes = new byte[] { 0x83, 0xCF, 0xFF, 0x33, 0xC9, 0x89, 0x8D, 0xCC, 0xFE, 0xFF, 0xFF, 0x81 };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                Util.GlobalVars.MaxCreatures = memRead.ReadInt32(values[0] + 13);
                Util.GlobalVars.StepCreatures = memRead.ReadInt32(values[0] + 21);
                m_address = memRead.ReadInt32(values[0] + 27);                
             
            }
            if (!Util.GlobalVars.ShowWithBase)
            {
               return m_address - memScan.BaseAddress;
            }
            return m_address;

        }
        public override string GetString()
        {
            return "Start = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
