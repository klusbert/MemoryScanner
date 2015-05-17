using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryScanner.Util
{
    public class Packet
    {
        private byte[] buffer;
        private int m_position;
        private int m_length;
        #region "Contructors"
        private int bufferSize = 16394;
        public Packet()
        {
            buffer = new byte[bufferSize];
            m_position = 2;
        }
        #endregion

        #region "Properties"
        public int Length
        {
            get { return m_length; }
            set { m_length = value; }
        }

        public int Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        public byte[] Data
        {
            get
            {
                byte[] t = new byte[m_length];
                Array.Copy(buffer, t, m_length);
                return t;
            }
        }
        public byte[] RawData
        {
            get
            {
                byte[] t = new byte[m_length - 2];
                Array.Copy(buffer, 2, t, 0, m_length - 2);
                return t;
            }
        }
        #endregion
        public int GetPacketHeaderSize()
        {
            return 2;
        }

        public void AddByte(byte value)
        {
            if (1 + m_length > bufferSize)
            {
                throw new Exception("NetworkMessage buffer is full.");
            }
            AddBytes(new byte[] { value });
        }

        public void AddBytes(byte[] value)
        {
            if (value.Length + m_length > bufferSize)
            {
                throw new Exception("NetworkMessage buffer is full.");
            }
            Array.Copy(value, 0, buffer, m_position, value.Length);
            m_position += value.Length;
            if (m_position > m_length)
            {
                m_length = m_position;
            }
        }

        public void AddString(string value)
        {
            AddUInt16(Convert.ToUInt16(value.Length));
            AddBytes(System.Text.ASCIIEncoding.Default.GetBytes(value));
        }

        public void AddUInt16(ushort value)
        {
            AddBytes(BitConverter.GetBytes(value));
        }


        public void AddUInt32(uint value)
        {
            AddBytes(BitConverter.GetBytes(value));
        }
    }
}
