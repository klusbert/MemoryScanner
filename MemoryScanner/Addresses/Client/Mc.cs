using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MemoryScanner.Addresses
{
    class Mc : GetAddresses
    {
        MemoryScanner memScan;
        MemoryReader memRead;
        AddressType Type;
        private int m_address;
        public Mc(MemoryReader _memRead, MemoryScanner _memScan, AddressType _type)
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
        public override int GetAddress()
        {
            if (m_address > 0)
            {
                return m_address;
            }
            List<int> values = memScan.ScanString("TibiaPlayerMutex");
            if (values.Count > 0)
            {
                values = memScan.ScanInt32(values[0]);
                if (values.Count > 0)
                {
                    m_address = values[0] -3;
                 
                }
            }
            if (!Util.GlobalVars.ShowWithBase)
            {
                return m_address - memScan.BaseAddress;
            }
            return m_address;
        }
        public override string GetString()
        {
            return "Mc = 0x" + this.GetAddress().ToString("X");
        }
        public override bool CheckAddress()
        {
            int adr = this.GetAddress();
            
            WinApi.PROCESS_INFORMATION pi = new WinApi.PROCESS_INFORMATION();
            WinApi.STARTUPINFO si = new WinApi.STARTUPINFO();
            string path = Util.GlobalVars.FullPath;
            string arguments = "";
          
            WinApi.CreateProcess(path, " " + arguments, IntPtr.Zero, IntPtr.Zero,
                false, WinApi.CREATE_SUSPENDED, IntPtr.Zero,
                System.IO.Path.GetDirectoryName(path), ref si, out pi);
            IntPtr handle = WinApi.OpenProcess(WinApi.PROCESS_ALL_ACCESS, 0, pi.dwProcessId);
            Process p = Process.GetProcessById(Convert.ToInt32(pi.dwProcessId));
            MemoryReader Writer = new MemoryReader(p);
            Writer.WriteByte(adr, 0xEB);// write jmp short
         
            WinApi.ResumeThread(pi.hThread);
            p.WaitForInputIdle();
            Writer.WriteByte(this.GetAddress(), 0x75);// write jnz short
            WinApi.CloseHandle(handle);
            WinApi.CloseHandle(pi.hProcess);
            WinApi.CloseHandle(pi.hThread);
            if (p.MainWindowTitle == "Tibia Error")
            {
                return false;
            }
            return true;
        }

    }
}
