using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MemoryScanner.Addresses
{
    public class GetNextPacket : GetAddresses
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
                return "GetNextPacket";
            }
        }
        public override void Search()
        {
       
            byte[] SearchBytes = new byte[] { 0x8B, 0xF8, 0x89, 0xBD, 0xF8, 0xFD, 0xFF, 0xFF, 0x83, 0xFF, 0xFF };
            List<int> value = memScan.ScanBytes(SearchBytes);
            if (value.Count > 0)
            {
                int GetNextPacket = value[0] - 5;  
                    
                m_address = GetNextPacket;                
                MyAddresses.ParseFunction.Address = Util.GlobalVars.SearchForFunctionStart(memRead, GetNextPacket);
                MyAddresses.Status.Address = memRead.ReadInt32(GetNextPacket + 58);
              
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
