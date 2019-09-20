using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    public class PlayerX : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public PlayerX(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
                return "PlayerX";
            }
        }

        public override void Search()
        {
            byte[] SearchBytes = new byte[] { 0x89, 0x65, 0xF0, 0x33, 0xDB, 0x89, 0x9D, 0xBC, 0xFE, 0xFF, 0xFF, 0x89, 0x5D, 0xFC, 0xA1 };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                int playerX = memRead.ReadInt32(values[0] + SearchBytes.Length);
                int playerY = playerX +4;
                int playerZ = playerX + 8;
                m_address = playerX;
                MyAddresses.PlayerY.Address = playerY;
                MyAddresses.PlayerZ.Address = playerZ;


            }
            

            return;
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
