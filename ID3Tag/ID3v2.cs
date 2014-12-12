using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ID3Tag
{
    public class ID3v2 : ID3
    {
        private Hashtable _Frames = new Hashtable();

        private List<ID3v2Frame> _AENCFrames = new List<ID3v2Frame>();
        private List<ID3v2Frame> _APICFrames = new List<ID3v2Frame>();
        private List<ID3v2Frame> _COMMFrames = new List<ID3v2Frame>();
        private List<ID3v2Frame> _GEOBFrames = new List<ID3v2Frame>();
        private List<ID3v2Frame> _LINKFrames = new List<ID3v2Frame>();
        private List<ID3v2Frame> _POPMFrames = new List<ID3v2Frame>();
        private List<ID3v2Frame> _PRIVFrames = new List<ID3v2Frame>();
        private List<ID3v2Frame> _SYLTFrames = new List<ID3v2Frame>();
        private List<ID3v2Frame> _TXXXFrames = new List<ID3v2Frame>();
        private List<ID3v2Frame> _UFIDFrames = new List<ID3v2Frame>();
        private List<ID3v2Frame> _USLTFrames = new List<ID3v2Frame>();
        private List<ID3v2Frame> _WCOMFrames = new List<ID3v2Frame>();
        private List<ID3v2Frame> _WOARFrames = new List<ID3v2Frame>();
        private List<ID3v2Frame> _WXXXFrames = new List<ID3v2Frame>();

        public Hashtable Frames
        {
            get
            {
                return _Frames;
            }
        }
        private ID3v2Header _Header;
        private ID3v2HeaderExtended _HeaderExtended;
        public ID3v2Header Header
        {
            get
            {
                return _Header;
            }
        }
        public ID3v2HeaderExtended HeaderExtended
        {
            get
            {
                return _HeaderExtended;
            }
        }
        public ID3v2(BinaryReader br)
        {
            br.BaseStream.Position = 0;

            if (br.ReadBytes(3).SequenceEqual<byte>(ID3.Ident_ID3v2))
            {
                _Frames.Add("AENC", _AENCFrames);
                _Frames.Add("APIC", _APICFrames);
                _Frames.Add("COMM", _COMMFrames);
                _Frames.Add("GEOB", _GEOBFrames);
                _Frames.Add("LINK", _LINKFrames);
                _Frames.Add("POPM", _POPMFrames);
                _Frames.Add("PRIV", _PRIVFrames);
                _Frames.Add("SYLT", _SYLTFrames);
                _Frames.Add("TXXX", _TXXXFrames);
                _Frames.Add("UFID", _UFIDFrames);
                _Frames.Add("USLT", _USLTFrames);
                _Frames.Add("WCOM", _WCOMFrames);
                _Frames.Add("WOAR", _WOARFrames);
                _Frames.Add("WXXX", _WXXXFrames);

                _Header = new ID3v2Header(br);

                tagVersion = new VersionNumber(2, _Header.Version[0], _Header.Version[1]);

                if ((_Header.Flags & 0x40) == 0x40)
                {
                    _HeaderExtended = new ID3v2HeaderExtended(br);
                }
                else
                {
                    // Set up blank Extended Header
                }

                for (int i = 0; i < Header.Size; i++)
                {
                    ID3v2Frame newFrame = new ID3v2Frame(br);
                    switch (newFrame.ID)
                    {
                        case "\0\0\0\0":
                            // empty frame, do not add
                            break;
                        case "AENC":
                            _AENCFrames.Add(newFrame);
                            break;
                        case "APIC":
                            _APICFrames.Add(newFrame);
                            break;
                        case "COMM":
                            _COMMFrames.Add(newFrame);
                            break;
                        case "GEOB":
                            _GEOBFrames.Add(newFrame);
                            break;
                        case "LINK":
                            _LINKFrames.Add(newFrame);
                            break;
                        case "POPM":
                            _POPMFrames.Add(newFrame);
                            break;
                        case "PRIV":
                            _PRIVFrames.Add(newFrame);
                            break;
                        case "SYLT":
                            _SYLTFrames.Add(newFrame);
                            break;
                        case "TXXX":
                            _TXXXFrames.Add(newFrame);
                            break;
                        case "UFID":
                            _UFIDFrames.Add(newFrame);
                            break;
                        case "USLT":
                            _USLTFrames.Add(newFrame);
                            break;
                        case "WCOM":
                            _WCOMFrames.Add(newFrame);
                            break;
                        case "WOAR":
                            _WOARFrames.Add(newFrame);
                            break;
                        case "WXXX":
                            _WXXXFrames.Add(newFrame);
                            break;
                        default:
                            _Frames.Add(newFrame.ID, newFrame);
                            break;
                    }
                    i += newFrame.Size + 10;
                }
            }
            else
            {
                throw new Exception("File does not contain an ID3v2 tag.");
            }
        }
        public string CreateFilePath(string parseString)
        {
            Regex tagMatcher = new Regex("<[A-Za-z0-9]{4}>|<[A-Za-z0-9]{4}:[0-9]+>");
            MatchCollection tags = tagMatcher.Matches(parseString);
            int padCount;
            foreach (Match m in tags)
            {
                string tagSubstitute = m.ToString();
                int sIndex = tagSubstitute.IndexOf(':');
                if (sIndex != -1)
                {
                    int eIndex = tagSubstitute.IndexOf('>');
                    int.TryParse(tagSubstitute.Substring(sIndex + 1, eIndex - (sIndex + 1)), out padCount);
                }
                else
                {
                    padCount = 0;
                }
                object obj = _Frames[tagSubstitute.Substring(1, 4)];
                if (obj != null && obj.GetType() == typeof(ID3v2Frame))
                {
                    ID3v2Frame frame = ((ID3v2Frame)obj);
                    string substitution = frame.Data;
                    // fix 1/2 track numbers
                    if (frame.ID == "TRCK" || frame.ID == "TPOS")
                    {
                        Regex numberMatcher = new Regex("^[0-9]+");
                        substitution = numberMatcher.Match(substitution).ToString();
                    }
                    if (substitution.Length < padCount)
                    {
                        for (int i = 0; i < padCount - substitution.Length; i++)
                        {
                            substitution = "0" + substitution;
                        }
                    }
                    parseString = parseString.Replace(tagSubstitute, substitution);
                }
                else
                {
                    parseString = parseString.Replace(tagSubstitute, "");
                }
            }
            parseString = parseString.Replace("\\\\", "\\");
            // horrible hack to stop D:\ becoming D-\
            parseString = parseString.Substring(0, 2) + parseString.Substring(2).Replace(':', '-');

            parseString = parseString.Replace('"', '\'');
            parseString = parseString.Replace('/', '-');
            parseString = parseString.Replace('*', '-');
            parseString = parseString.Replace('<', '-');
            parseString = parseString.Replace('>', '-');
            parseString = parseString.Replace('|', '-');
            //parseString = parseString.Replace((char)0x00, '-');

            parseString = parseString.Replace(".\\", "\\");
            parseString = parseString.Replace(".\\", "\\");
            parseString = parseString.Replace(".\\", "\\");

            parseString = parseString.Replace("?", "");
            return parseString;
        }
    }
}
