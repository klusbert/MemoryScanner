using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    public class PlayerId : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public PlayerId(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
                return "PlayerId";
            }
        }
        public override void Search()
        {
          
            byte[] SearchBytes = new byte[] { 0xFF, 0xB3, 0x08, 0x01, 0x00, 0x00, 0xFF, 0xB3, 0x04, 0x01, 0x00, 0x00, 0x6A, 0x00, 0x33, 0xD2, 0xB9, 0xFF, 0xFF, 0x00, 0x00, };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 1)
            {
                int playerId = memRead.ReadInt32(values[1] - 4);
    
         //       MyAddresses.PlayerX.Address = playerId + 4;
          //      MyAddresses.PlayerY.Address = playerId + 8;
           //     MyAddresses.PlayerZ.Address = playerId + 12;
                MyAddresses.RedSqare.Address = memRead.ReadInt32(values[1] + 49);
                m_address = playerId;         
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
