using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ID3Tag;

namespace ID3Test
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: ID3Test.exe <filename.mp3>");
                return;
            }
            ID3 id3tags = ID3Factory.DecodeID3(args[0]);
            Console.WriteLine(id3tags.GetType());
            switch (id3tags.GetType().ToString())
            {
                case "ID3Tag.ID3v1":
                    DisplayID3v1tags((ID3v1)id3tags);
                    break;
                case "ID3Tag.ID3v2":
                    DisplayID3v2tags((ID3v2)id3tags);
                    break;
            }
        }
        static void DisplayID3v1tags(ID3v1 tags)
        {
            Console.WriteLine("Not done yet.");
        }
        static void DisplayID3v2tags(ID3v2 tags)
        {
            Console.WriteLine("Track : {0}", ((ID3v2Frame)tags.Frames["TRCK"]).Data);
            Console.ReadLine();
        }
    }
}
