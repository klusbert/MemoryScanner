using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Addresses
{
    public class WalkFunction : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public WalkFunction(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
        public override void Search()
        {
           
            byte[] SearchBytes = new byte[] { 0x6A, 0x01, 0x6A, 0xFF, 0x6A, 0xFF, 0xE8 };//push 1 push -1 push -1
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                int adr = memRead.GetCallFunction(values[0] + 6);
                m_address = adr;
               
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
            return "WalkFunction = 0x" + val.ToString("X");
        }
        public override bool CheckAddress()
        {
            Tests.Walk walk = new Tests.Walk(memRead);
            int x = 1;
            int y = 0;
            byte diag = 0;
            diag = (byte)(Math.Abs(x) * Math.Abs(y));
            bool worked =   walk.MakeWalk(x, y, diag);
            walk.CleanUp();
            return worked;
        }
    }
}
