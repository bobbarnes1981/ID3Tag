using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ID3Tag
{
    public class ID3Factory
    {
        public static ID3 DecodeID3(string path)
        {
            if (File.Exists(path))
            {
                ID3 newTags = null;
                FileStream fs = File.OpenRead(path);
                BinaryReader br = new BinaryReader(fs);
                if (br.ReadBytes(3).SequenceEqual<byte>(ID3.Ident_ID3v2))
                {
                    newTags = new ID3v2(br);
                }
                else
                {
                    br.BaseStream.Position = br.BaseStream.Length - 128;
                    if (br.ReadBytes(3).SequenceEqual<byte>(ID3.Ident_ID3v1))
                    {
                        newTags = new ID3v1(br);
                    }
                    else
                    {
                        throw new Exception(string.Format("File does not contain an ID3 tag.", path));
                    }
                }

                br.Close();
                fs.Close();

                return newTags;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
    }
}
