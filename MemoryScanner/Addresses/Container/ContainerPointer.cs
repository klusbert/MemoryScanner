using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    class ContainerPointer:GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public ContainerPointer(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
            if (m_address > 0)
            {
                return m_address;
            }
            byte[] SearchBytes = new byte[] { 0x83, 0xC4, 0x04, 0x8B, 0x4D, 0xF4, 0x64, 0x89, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x59, 0x5F, 0x5E, 0x8B, 0xE5, 0x5D, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xC7, 0x01 };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                int adr = memRead.ReadInt32(values[0] - 4);
                m_address = adr;
               
            }
            if (!Util.GlobalVars.ShowWithBase)
            {
                return m_address - memScan.BaseAddress;
            }
            return m_address;
        }
        public override string GetString()
        {
            return "Pointer = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
