using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Tests
{
   public class PlayerName
    {
        MemoryReader memRead;
        public PlayerName(MemoryReader _memread)
        {       
            memRead = _memread;
        }
        public string GetPlayerName()
        {
            string name = "";
            //   byte[] cache = memRead.ReadBytes(Addresses.MyAddresses.BlistStart.Address, (uint)(Addresses.MyAddresses.MaxCreatures.Address * Addresses.MyAddresses.BlistStep.Address));

            int blistStart= Addresses.MyAddresses.BlistStart.Address;
            int blistStep = Addresses.MyAddresses.BlistStep.Address;
            int blistMaxCreatures= Addresses.MyAddresses.MaxCreatures.Address;
            int blistEnd = blistStart+ (blistStep * blistMaxCreatures);
            int PlayerId = memRead.ReadInt32(Addresses.MyAddresses.PlayerId.Address);

            for (int adr = blistStart; adr < blistEnd; adr += blistStep)
            {
                if(memRead.ReadInt32(adr) ==PlayerId)
                {
                    name = memRead.ReadString(adr + 4);
                    break;
                }
            }
                return name;
        }

    }
}
