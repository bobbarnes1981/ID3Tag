using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ID3Tag
{
    public class ID3v1 : ID3
    {
        private string title;
        private string artist;
        private string album;
        private string year;
        private string comment;
        private string genre;
        private string trackNumber;
        private string extTitle;
        private string extArtist;
        private string extAlbum;
        private string extSpeed;
        private string extGenre;
        private string extStartTime;
        private string extEndTime;
        public string Title
        {
            get
            {
                return title + extTitle;
            }
        }
        public string Artist
        {
            get
            {
                return artist + extArtist;
            }
        }
        public string Album
        {
            get
            {
                return album + extAlbum;
            }
        }
        public string Year
        {
            get
            {
                return year;
            }
        }
        public string Comment
        {
            get
            {
                return comment;
            }
        }
        public string GenreNum
        {
            get
            {
                return genre;
            }
        }
        public string TrackNumber
        {
            get
            {
                return trackNumber;
            }
        }
        public string Speed
        {
            get
            {
                return extSpeed;
            }
        }
        public string GenreTxt
        {
            get
            {
                return extGenre;
            }
        }
        public string StartTime
        {
            get
            {
                return extStartTime;
            }
        }
        public string EndTime
        {
            get
            {
                return extEndTime;
            }
        }
        public ID3v1(BinaryReader br)
        {
            if (br.BaseStream.Length > 128)
            {
                br.BaseStream.Position = br.BaseStream.Length - 128;
                if (br.ReadBytes(3).SequenceEqual<byte>(ID3.Ident_ID3v1))
                {
                    tagVersion = new VersionNumber(1, 0, 0);
                    title = ASCIIEncoding.ASCII.GetString(br.ReadBytes(30)).Trim('\0');
                    artist = ASCIIEncoding.ASCII.GetString(br.ReadBytes(30)).Trim('\0');
                    album = ASCIIEncoding.ASCII.GetString(br.ReadBytes(30)).Trim('\0');
                    year = ASCIIEncoding.ASCII.GetString(br.ReadBytes(4)).Trim('\0');
                    byte[] commentBytes = br.ReadBytes(30);
                    string commentString = ASCIIEncoding.ASCII.GetString(commentBytes);
                    if (commentBytes[28] == 0x00)
                    {
                        comment = commentString.Substring(0, 28).Trim('\0');
                        trackNumber = ((int)commentBytes[29]).ToString();
                    }
                    else
                    {
                        comment = commentString.Trim('\0');
                        trackNumber = "";
                    }
                    genre = ((int)br.ReadByte()).ToString();

                    extTitle = "";
                    extArtist = "";
                    extAlbum = "";
                    extSpeed = "";
                    extGenre = "";
                    extStartTime = "";
                    extEndTime = "";

                    if (br.BaseStream.Length > 227)
                    {
                        br.BaseStream.Position = br.BaseStream.Length - 227;
                        if (br.ReadBytes(4).SequenceEqual<byte>(ID3.Ident_ID3v1e))
                        {
                            tagVersion = new VersionNumber(1, 1, 0);
                            extTitle = ASCIIEncoding.ASCII.GetString(br.ReadBytes(60)).Trim('\0');
                            extArtist = ASCIIEncoding.ASCII.GetString(br.ReadBytes(60)).Trim('\0');
                            extAlbum = ASCIIEncoding.ASCII.GetString(br.ReadBytes(60)).Trim('\0');
                            extSpeed = ((int)br.ReadByte()).ToString();
                            extGenre = ASCIIEncoding.ASCII.GetString(br.ReadBytes(30)).Trim('\0');
                            extStartTime = ASCIIEncoding.ASCII.GetString(br.ReadBytes(6));
                            extEndTime = ASCIIEncoding.ASCII.GetString(br.ReadBytes(6));
                        }
                    }
                }
                else
                {
                    throw new Exception("File does not contain an ID3v1 tag.");
                }
            }
            else
            {
                throw new Exception("File is not large enough to contain an ID3v1 tag.");
            }
        }
    }
}
