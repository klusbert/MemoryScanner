using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner
{
    class CodeCaveHelper
    {
        private byte[] buffer;
        private int position, length, bufferSize = 16394;
        public CodeCaveHelper()
        {
            buffer = new byte[bufferSize];

        }
        public void AddLine(params object[] arg)
        {
            for (int i = 0; i < arg.Length; i++)
            {
                object obj = arg[i];

                if (obj is string)
                {
                    AddString((string)obj);
                }
                else if (obj is UInt32)
                {
                    AddInt32((UInt32)obj);
                }
                else if (obj is UInt16)
                {
                    AddInt32((UInt16)obj);
                }

                else if (obj is byte)
                {
                    AddByte((byte)obj);
                }
                else if (obj is Int32)
                {
                    AddInt32((Int32)obj);
                }
                else if (obj is byte[])
                {
                    AddBytes((byte[])obj);
                }


            }
        }
        public byte[] Data
        {

            get
            {
                byte[] t = new byte[length];
                Array.Copy(buffer, t, length);
                return t;


            }
        }

        public void AddByte(byte value)
        {
            if (1 + length > bufferSize)
                throw new Exception("NetworkMessage buffer is full.");

            AddBytes(new byte[] { value });
        }

        public void AddBytes(byte[] value)
        {
            if (value.Length + length > bufferSize)
                throw new Exception("NetworkMessage buffer is full.");


            Array.Copy(value, 0, buffer, position, value.Length);
            position += value.Length;

            if (position > length)
                length = position;
        }

        public void AddString(string value)
        {
            AddInt16((ushort)value.Length);
            AddBytes(System.Text.ASCIIEncoding.Default.GetBytes(value));
        }

        public void AddInt16(ushort value)
        {
            AddBytes(BitConverter.GetBytes(value));
        }

        public void AddInt32(uint value)
        {
            AddBytes(BitConverter.GetBytes(value));
        }
        public void AddInt32(int value)
        {
            AddBytes(BitConverter.GetBytes(value));
        }
        public void AddInt(int value)
        {
            AddBytes(BitConverter.GetBytes(value));
        }
    }
}
