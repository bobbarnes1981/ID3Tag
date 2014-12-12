using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ID3Tag
{
    public class ID3v2Header
    {
        private string identifier = "ID3";
        private byte[] version;
        private byte flags;
        private int size;
        public string Identifier
        {
            get
            {
                return identifier;
            }
        }
        public byte[] Version
        {
            get
            {
                return version;
            }
        }
        public byte Flags
        {
            get
            {
                return flags;
            }
        }
        public int Size
        {
            get
            {
                return size;
            }
        }
        public ID3v2Header(BinaryReader br)
        {
            version = br.ReadBytes(2);
            flags = br.ReadByte();
            byte[] sizeBytes = br.ReadBytes(4);
            size = 0;
            for (int j = 0; j < 4; j++)
            {
                size += ((int)sizeBytes[j]) << (3 - j) * 7;
            }
        }
    }
}
