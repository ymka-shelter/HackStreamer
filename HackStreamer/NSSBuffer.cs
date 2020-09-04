//  Email:  yetiicb@hotmail.com 
// 
//  Copyright (C) 2002-2004 Idael Cardoso.  

using System;
using System.Runtime.InteropServices;
using WindowsMediaLib;

namespace Hack
{
    internal class ManBuffer : INSSBuffer
    {
        private int m_UsedLength;
        private int m_MaxLength;
        private byte[] m_Buffer;
        private GCHandle handle;

        /// <summary> 
        /// Create a buffer with specified size 
        /// </summary> 
        /// <param name="size">Maximun size of buffer</param> 
        public ManBuffer(int size)
        {
            m_Buffer = new byte[size];
            m_MaxLength = m_UsedLength = size;
            handle = GCHandle.Alloc(m_Buffer, GCHandleType.Pinned);
        }
        ~ManBuffer()
        {
            handle.Free();
        }

        /// <summary> 
        /// How many bytes are currently used in the buffer.  
        /// Equivalent to INSSBuffer.GetLentgh and INSSBuffer.SetLength 
        /// </summary> 
        public int UsedLength
        {
            get { return m_UsedLength; }
            set
            {
                if (value <= m_MaxLength)
                {
                    m_UsedLength = value;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        /// <summary> 
        /// Maximun buffer size. Equivalent to INSSBuffer.GetMaxLength 
        /// </summary> 
        public int MaxLength
        {
            get { return m_MaxLength; }
        }

        /// <summary> 
        /// Internal byte array that contain buffer data. 
        /// </summary> 
        public byte[] Buffer
        {
            get { return m_Buffer; }
        }

        #region INSSBuffer Members 

        public void GetLength(out int pdwLength)
        {
            pdwLength = m_UsedLength;
        }

        public void SetLength(int dwLength)
        {
            UsedLength = dwLength;
        }

        public void GetMaxLength(out int pdwLength)
        {
            pdwLength = m_MaxLength;
        }

        public void GetBuffer(out IntPtr ppdwBuffer)
        {
            ppdwBuffer = handle.AddrOfPinnedObject();
        }

        public void GetBufferAndLength(out IntPtr ppdwBuffer, out int pdwLength)
        {
            ppdwBuffer = handle.AddrOfPinnedObject();
            pdwLength = m_UsedLength;
        }

        #endregion

    }

}     