using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ID3Tag
{
    public class ID3v2Frame
    {
        private string id;
        private byte[] flags;
        private int size;
        private string data;
        private byte encoding;
        public string ID
        {
            get
            {
                return id;
            }
        }
        public byte[] Flags
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
        public string Data
        {
            get
            {
                return data;
            }
        }
        public byte Encoding
        {
            get
            {
                return encoding;
            }
        }
        public ID3v2Frame(BinaryReader br)
        {
            // read frame header
            id = System.Text.ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));
            // size
            size = 0;
            size += br.ReadByte() << 24;
            size += br.ReadByte() << 16;
            size += br.ReadByte() << 8;
            size += br.ReadByte();
            // flags
            flags = br.ReadBytes(2);
            // FIXME: needs to deal with frames better
            if (id.StartsWith("T") || id == "XSOP")
            {
                // encoding
                encoding = br.ReadByte();
                switch (Encoding)
                {
                    case 0x00:
                        // ISO-8859-1 (ASCII).
                        data = ASCIIEncoding.ASCII.GetString(br.ReadBytes(Size - 1));
                        break;
                    case 0x01:
                        // UCS-2 in ID3v2.2 and ID3v2.3, UTF-16 encoded Unicode with BOM.
                        if (size > 2)
                        {
                            byte[] BOM;
                            BOM = br.ReadBytes(2);
                            if (BOM[0] == 0xff && BOM[1] == 0xfe)
                            {
                                data = ASCIIEncoding.Unicode.GetString(br.ReadBytes(size - 3));
                            }
                            else if (BOM[0] == 0xfe && BOM[1] == 0xff)
                            {
                                data = ASCIIEncoding.BigEndianUnicode.GetString(br.ReadBytes(size - 3));
                            }
                            else
                            {
                                data += ASCIIEncoding.ASCII.GetString(BOM);
                                data += ASCIIEncoding.ASCII.GetString(br.ReadBytes(Size - 3));
                            }
                        }
                        else
                        {
                            data = ASCIIEncoding.ASCII.GetString(br.ReadBytes(Size - 1));
                        }
                        break;
                    case 0x02:
                        // UTF-16BE encoded Unicode without BOM in ID3v2.4 only.
                        data = ASCIIEncoding.BigEndianUnicode.GetString(br.ReadBytes(Size - 1));
                        break;
                    case 0x03:
                        // UTF-8 encoded Unicode in ID3v2.4 only.
                        data = ASCIIEncoding.UTF8.GetString(br.ReadBytes(Size - 1));
                        break;
                    default:
                        throw new Exception(string.Format("Unhandled Encoding Type '{0:X2}'\r\n'{1}'\r\n'{2}'\r\n'{3}'", Encoding, id, data, size));
                }
            }
            else
            {
                br.ReadBytes(size);
            }
        }
    }
}
