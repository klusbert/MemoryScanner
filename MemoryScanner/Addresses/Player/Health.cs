using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    public class Health : GetAddresses 
    {

        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public Health(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
                return "Health";
            }
        }
        public override void Search()
        {
         
            List<int> values = memScan.ScanString("Hit Points");
            if(values.Count > 0)
            {
                
                values = memScan.ScanInt32(values[1]);

            //    values.Add(0x47e83b);
              if (values.Count > 0)
                {
                    values[0] -= 1;
                    MyAddresses.XorKey.Address = memRead.ReadInt32(values[0] + 14);
                    MyAddresses.ManaMax.Address = MyAddresses.XorKey.Address + 4;

                    MyAddresses.Mana.Address = memRead.ReadInt32(values[0] + 276);
                    m_address = memRead.ReadInt32(values[0] + 20);
                }
                  
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
