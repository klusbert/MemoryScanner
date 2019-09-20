using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Tests.Container
{
   public class Inventory
   {
       private MemoryReader memRead;

       public Inventory( MemoryReader _memread)
       {
           memRead = _memread;
       }

       private int ContainerPointer()
       {
           return memRead.ReadInt32(Addresses.MyAddresses.ContainerPointer.Address) + 4;
       }
       public int ContainersCount()
       {
           return memRead.ReadInt32(ContainerPointer() + 4);
       }
      public void GetContainers()
       {
           int Count;
          int FirstNode = memRead.ReadInt32(Addresses.MyAddresses.ContainerPointer.Address);
          Count = memRead.ReadInt32(FirstNode + 8);
          TreeNode Node = new TreeNode(memRead.ReadInt32(FirstNode + 4), memRead);
     

         while(Count > 0)
         {

         }
       }
   class TreeNode
   {
       int BaseAddress;
       public int Left;
       public int Parrent;
       public int Right;
       public int Index;
       public int ContainerAddress;

       public byte Color;
       public byte IsNull;
       MemoryReader Memread;
     public TreeNode(int _baseAddr,MemoryReader _memread)
       {
           BaseAddress = _baseAddr;
           Memread = _memread;
           Left= Memread.ReadInt32(BaseAddress);
           Parrent = Memread.ReadInt32(BaseAddress+4);
           Right = Memread.ReadInt32(BaseAddress+8);
           Color = Memread.ReadByte(BaseAddress + 12);
           IsNull = Memread.ReadByte(BaseAddress + 13);
           Index = Memread.ReadInt32(BaseAddress + 16);
           ContainerAddress = Memread.ReadInt32(BaseAddress + 20);
       }
       
   }

   }
}
