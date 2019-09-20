using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    public class AttackCount : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public AttackCount(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
                return "AttackCount";
            }
        }
        public override void Search()
        {      
            byte[] SearchBytes = new byte[] { 0xB9, 0xA1, 0x00,0x00, 0x00, 0xE8 };     
            List<int> values = memScan.ScanBytes(SearchBytes);//MOV ECX,0A1
            if(values.Count > 0)
            {               
                MyAddresses.SendPacket.Address = memRead.GetCallFunction(values[0] + 30);
                MyAddresses.CreatePacket.Address = memRead.GetCallFunction(values[0] + 5);
                m_address = memRead.ReadInt32(values[0] - 4);               
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
