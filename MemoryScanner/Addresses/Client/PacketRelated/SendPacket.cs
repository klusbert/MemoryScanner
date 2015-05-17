using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MemoryScanner.Addresses
{
    class SendPacket :GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public SendPacket(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
            m_address= memRead.GetCallFunction(Util.GlobalVars.AttackCountRegion + 30);
            Util.GlobalVars.SendPacket = m_address;

            if (!Util.GlobalVars.ShowWithBase)
            {
                return m_address - memScan.BaseAddress;
            }
            return m_address;
        }
        public override string GetString()
        {
            return "SendPacket = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {         
            
            return base.CheckAddress();
        }
    }
}
