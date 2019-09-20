using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MemoryScanner.Addresses
{
    public class Mc : GetAddresses
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
                return "MC";
            }
        }
        public override void Search()
        {      
            List<int> values = memScan.ScanString("TibiaPlayerMutex");
            if (values.Count > 0)
            {
                values = memScan.ScanInt32(values[0]);
                if (values.Count > 0)
                {
                    m_address = values[0] -3;
                 
                }
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
            int adr = Address;
            
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
            Writer.WriteByte(Address, 0x75);// write jnz short
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
