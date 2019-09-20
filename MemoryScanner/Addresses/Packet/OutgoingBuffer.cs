using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Addresses
{
    public class OutgoingBuffer : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public OutgoingBuffer(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
                return "OutGoingBuffer";
            }
        }
        public override void Search()
        {
          
            byte[] SearchBytes = new byte[] { 0xC6, 0x45, 0xFC, 0x01, 0xBF, 0x06, 0x00, 0x00, 0x00, 0x89, 0xBD, 0xA0, 0xFE, 0xFF, 0xFF, 0x8D, 0x46, 0x02, 0x3B, 0xF8, 0x7D, 0x19 };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                values[0] += SearchBytes.Length;
                int adr = values[0];
                int OutGoingBuffer = memRead.ReadInt32(adr + 2);
                int xtea = memRead.ReadInt32(adr + 11);
                int len = memRead.ReadInt32(adr + 34);
                m_address = OutGoingBuffer;             
                MyAddresses.XTEA.Address = xtea;
                MyAddresses.OutGoingPacketLen.Address = len;               

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
