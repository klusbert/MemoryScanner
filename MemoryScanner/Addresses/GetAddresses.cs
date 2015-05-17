using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MemoryScanner.Addresses
{
    class GetAddresses
    {
        public enum AddressType
        {
            BattleList = 0,
            Client = 1,
            Map = 2,
            Player = 3,
            Container=4,
            None =5,
        }
        public virtual AddressType AddressCategory
        {
            get
            {
                return AddressType.None; 
            }
        }
        public virtual bool CheckAddress()
        {
            return true;
        }
        public virtual int GetAddress()
        {
            return 0;
        }
        public virtual string GetString()
        {
            return "";
        }
    }
}
