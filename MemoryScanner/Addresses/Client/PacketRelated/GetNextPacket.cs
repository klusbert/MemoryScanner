using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MemoryScanner.Addresses
{
    class GetNextPacket:GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public GetNextPacket(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
            byte[] SearchBytes = new byte[] { 0x8B, 0xF8, 0x89, 0xBD, 0xF8, 0xFD, 0xFF, 0xFF, 0x83, 0xFF, 0xFF };
            List<int> value = memScan.ScanBytes(SearchBytes);
            if (value.Count > 0)
            {
                int GetNextPacket = value[0] - 5;       
            
                 Util.GlobalVars.ParseFunction =    Util.GlobalVars.SearchForFunctionStart(memRead, GetNextPacket);
                 Util.GlobalVars.ConnectionStatus = memRead.ReadInt32(GetNextPacket + 58);

                m_address = GetNextPacket;
                Util.GlobalVars.GetNextPacket = m_address;
              
            }
            if (!Util.GlobalVars.ShowWithBase)
            {
                return m_address - memScan.BaseAddress;
            }
            return m_address; 
        }
        public override string GetString()
        {
            return "GetNextPacket = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
