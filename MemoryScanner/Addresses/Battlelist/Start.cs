using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    public class BlistStart : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public BlistStart(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
                return "BattleList Start";
            }
        }
        public override void Search()
        {
       
            byte[] SearchBytes = new byte[] { 0x83, 0xCF, 0xFF, 0x33, 0xC9, 0x89, 0x8D, 0xCC, 0xFE, 0xFF, 0xFF, 0x81 };
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
            
                MyAddresses.MaxCreatures.Address = memRead.ReadInt32(values[0] + 13);
                MyAddresses.BlistStep.Address = memRead.ReadInt32(values[0] + 21);
                m_address = memRead.ReadInt32(values[0] + 27);                
             
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
