using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MemoryScanner.Addresses
{
   public class GetAddresses
    {
        public enum AddressType
        {
            BattleList = 0,
            Client = 1,
            Map = 2,
            Player = 3,
            Container=4,
            Packet = 5,
            None =6,
            InternalFunction =7,
            TextDisplay = 8
        }
        public virtual AddressType AddressCategory
        {
            get
            {
                return AddressType.None; 
            }
        }
        public virtual int Address
        {
            get
            {
                return 0;
            }
            set
            {
                value = 0;
            }
        }
       public virtual string Name
        {
            get { return ""; }
        }
        public virtual bool CheckAddress()
        {
            return true;
        }
        public virtual void Search()
        {
            return ;
        }
        public virtual string GetString()
        {
            return "";
        }
    }
}
