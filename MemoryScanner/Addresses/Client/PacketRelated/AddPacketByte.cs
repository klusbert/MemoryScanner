using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    class AddPacketByte : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public AddPacketByte(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
         
            byte[] SearchBytes = new byte[] { 0xB9, 0x96, 0x00, 0x00, 0x00, 0xE8 };//MOV ECX,096        
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                
               m_address = memRead.GetCallFunction(values[0] + 12);
               Util.GlobalVars.AddBytePacket = m_address;
              
            }
            if (!Util.GlobalVars.ShowWithBase)
            {
                return m_address - memScan.BaseAddress;
            }
            return m_address;

        }
        public override string GetString()
        {
            return "AddPacketByte = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
