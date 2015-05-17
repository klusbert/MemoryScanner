using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Addresses
{
    class RecivePointer:GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public RecivePointer(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
        public override int GetAddress()
        {
            if(m_address > 0)
            {
                return m_address;
            }
            byte[] SearchBytes = new byte[] { 0x8B, 0xEC, 0xFF, 0x75, 0x10, 0xFF, 0x75, 0x0C, 0xFF, 0x75, 0x08, 0xFF, 0x71, 0x04 };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                //recive
                int adr = values[0] + SearchBytes.Length + 2;
                adr = memRead.ReadInt32(adr);
                m_address = adr;
            }
            if (values.Count > 1)
            {
                //send
                int adr = values[1] + SearchBytes.Length + 2;
                adr = memRead.ReadInt32(adr);
                Util.GlobalVars.SendPointer = adr;
            }

            if (!Util.GlobalVars.ShowWithBase)
            {
                return m_address - memScan.BaseAddress;
            }
            return m_address;
        }
        public override string GetString()
        {
            return "RecvPointer = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
