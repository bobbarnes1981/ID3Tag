using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ID3Tag
{
    public abstract class ID3
    {
        public struct VersionNumber
        {
            private int major;
            private int minor;
            private int revision;
            public int Major
            {
                get
                {
                    return major;
                }
            }
            public int Minor
            {
                get
                {
                    return minor;
                }
            }
            public int Revision
            {
                get
                {
                    return revision;
                }
            }
            public VersionNumber(int major, int minor, int revision)
            {
                this.major = major;
                this.minor = minor;
                this.revision = revision;
            }
            public override string ToString()
            {
                return string.Format("{0}.{1}.{2}", major, minor, revision);
            }
        }
        public static byte[] Ident_ID3v1 = new byte[] { 0x54, 0x41, 0x47 };         // TAG
        public static byte[] Ident_ID3v1e = new byte[] { 0x54, 0x41, 0x47, 0x2B };  // TAG+
        public static byte[] Ident_ID3v2 = new byte[] { 0x49, 0x44, 0x33 };         // ID3
        protected VersionNumber tagVersion;
        public VersionNumber TagVersion
        {
            get
            {
                return tagVersion;
            }
        }
    }
}
