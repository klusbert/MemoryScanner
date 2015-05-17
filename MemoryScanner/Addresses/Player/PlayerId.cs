using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    class PlayerId:GetAddresses
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
        public override int GetAddress()
        {
            if (m_address > 0)
            {
                return m_address;
            }
            byte[] SearchBytes = new byte[] { 0xFF, 0xB3, 0x08, 0x01, 0x00, 0x00, 0xFF, 0xB3, 0x04, 0x01, 0x00, 0x00, 0x6A, 0x00, 0x33, 0xD2, 0xB9, 0xFF, 0xFF, 0x00, 0x00, };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 1)
            {
                int playerId = memRead.ReadInt32(values[1] - 4);
                Util.GlobalVars.PlayerId = playerId;
                Util.GlobalVars.PlayerX = playerId + 4;
                Util.GlobalVars.PlayerY = playerId + 8;
                Util.GlobalVars.PlayerZ = playerId + 12;
                Util.GlobalVars.Redsquare = memRead.ReadInt32(values[1] + 49);
                m_address = playerId;              

            }
            if (!Util.GlobalVars.ShowWithBase)
            {
                return m_address - memScan.BaseAddress;
            }
            return m_address;
        }
        public override string GetString()
        {
            return "Id = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
