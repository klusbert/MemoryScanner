using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    class XorKey:GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        public XorKey(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
        {
            this.memRead = _memRead;
            this.memScan = _memScan;
            this.Type = _type;
        }
        public override GetAddresses.AddressType AddressCategory
        {
            get
            {
                return Type ;
            }
          
        }
        public override int GetAddress()
        {

            if (!Util.GlobalVars.ShowWithBase)
            {
                return Util.GlobalVars.XorKey - memScan.BaseAddress;
            }
            return Util.GlobalVars.XorKey;
        }
        public override string GetString()
        {
            return "XorKey = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            return base.CheckAddress();
        }
         
    }
}
