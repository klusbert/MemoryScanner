using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Tests
{
  public  class Walk
    {
      private MemoryReader memRead;
      private IntPtr Adr;
      private IntPtr Handle;
      public Walk(MemoryReader _memRead)
      {
          memRead = _memRead;
          Handle = _memRead.Handle;
          CreateCave();
      }
      public bool MakeWalk(int x, int y, byte isdiagonal)
      {

          memRead.WriteByte(Adr.ToInt64() + 1, isdiagonal);
          memRead.WriteInt32(Adr.ToInt64() + 3, y);
          memRead.WriteInt32(Adr.ToInt64() + 8, x);

          uint ExitCode = 0;
          IntPtr thread1 = WinApi.CreateRemoteThread(Handle, IntPtr.Zero, 0, Adr, IntPtr.Zero, 0, IntPtr.Zero);
          WinApi.WaitForSingleObject(thread1, 0xFFFFFFFF);
          WinApi.GetExitCodeThread(thread1, ref ExitCode);

          if (ExitCode == 14 || ExitCode == 22)
          {
              //the walk worked nothing blocking the path.
              return true;
          }
          return false;
      }
      public void CleanUp()
      {
          WinApi.VirtualFreeEx(Handle, Adr, 20, WinApi.AllocationType.Release);
      }
      private void CreateCave()
      {
          int x, y;
          x = 0;
          y = 0;
          CodeCaveHelper cv = new CodeCaveHelper();
          cv.AddLine((byte)0x6A, (byte)0x00); //push 0
          cv.AddLine((byte)0x68, (Int32)y); //push y int32
          cv.AddLine((byte)0x68, (Int32)x); //push x int32
          cv.AddLine((byte)0xb8, (uint)Addresses.MyAddresses.WalkFunction.Address); //  MOV EAX, <DWORD> | ty DarkStar
          cv.AddLine((byte)0xFF, (byte)0xD0);// call eax
          cv.AddLine((byte)0xc3);

          Adr = WinApi.VirtualAllocEx(Handle, IntPtr.Zero, (uint)cv.Data.Length, WinApi.AllocationType.Commit | WinApi.AllocationType.Reserve, WinApi.MemoryProtection.ExecuteReadWrite);
          memRead.WriteBytes(Adr.ToInt64(), cv.Data, (uint)cv.Data.Length);
          System.Windows.Forms.Clipboard.SetText(Adr.ToString("X"));

      }
    }
}
