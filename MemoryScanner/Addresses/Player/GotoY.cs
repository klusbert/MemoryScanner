﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MemoryScanner.Addresses.GetAddresses;

namespace MemoryScanner.Addresses.Player
{
 public   class GotoY : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public GotoY(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
        {
            this.memRead = _memRead;
            this.memScan = _memScan;
            this.Type = _type;
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
        public override GetAddresses.AddressType AddressCategory
        {
            get
            {
                return Type;
            }

        }
        public override string Name
        {
            get
            {
                return "GotoY";
            }
        }

        public override void Search()
        {
          
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