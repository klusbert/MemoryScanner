using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MemoryScanner.Addresses
{
    public class PrintFps : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;

        public PrintFps(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
                return "PrintFPS";
            }
        }
        public override void Search()
        {         
          //  byte[] SearchBytes = new byte[] { 0x6A, 0x00, 0x8D, 0x45, 0xD0, 0x50, 0x83, 0xEC, 0x0C, 0x8B, 0xD4, 0xC7, 0x02, 0xC8, 0x00, 0x00, 0x00, 0xC7, 0x42, 0x04, 0xC8, 0x00, 0x00, 0x00, 0xC7, 0x42, 0x08, 0xC8, 0x00, 0x00, 0x00, 0x6A, 0x02 };
            byte[]SearchBytes = new byte []{0x2B, 0xD8, 0x8B, 0xC3, 0x99, 0x2B, 0xC2, 0xD1, 0xF8, 0x83, 0xC0, 0x04, 0x50, 0x8B, 0xD6, 0xB9, 0x01, 0x00, 0x00, 0x00};
            List<int> values = memScan.ScanBytes(SearchBytes);
            if (values.Count > 0)
            {
                values[0] += SearchBytes.Length;
                m_address = values[0];
                MyAddresses.PrintText.Address = memRead.GetCallFunction(values[0]);
          
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
