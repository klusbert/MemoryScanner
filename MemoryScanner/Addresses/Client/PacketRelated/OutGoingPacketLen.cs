using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Addresses
{
    class OutGoingPacketLen:GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        public OutGoingPacketLen(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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

            if (!Util.GlobalVars.ShowWithBase)
            {
                return Util.GlobalVars.OutGoingBufferLen - memScan.BaseAddress;
            }
            return Util.GlobalVars.OutGoingBufferLen;       
        }
        public override string GetString()
        {
            return "OutGoingPacketLen = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
    }
}
