using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concentus.Oggfile
{
    public class OpusTagsPacket
    {
        string comment;
        IDictionary<string, string> fields = new Dictionary<string, string>();

        public static OpusTagsPacket Parse(byte[] packet, int packetLength)
        {
            if (packetLength < 8)
                return null;
            if (!"OpusTags".Equals(Encoding.UTF8.GetString(packet, 0, 8)))
                return null;
            OpusTagsPacket returnVal = new OpusTagsPacket();
            int cursor = 8;
            int nextFieldLength = BitConverter.ToInt32(packet, cursor);
            cursor += 4;
            if (nextFieldLength > 0)
            {
                returnVal.comment = Encoding.UTF8.GetString(packet, cursor, nextFieldLength);
                cursor += nextFieldLength;
            }
            int numTags = BitConverter.ToInt32(packet, cursor);
            cursor += 4;
            for (int c = 0; c < numTags; c++)
            {
                nextFieldLength = BitConverter.ToInt32(packet, cursor);
                cursor += 4;
                if (nextFieldLength > 0)
                {
                    string tag = Encoding.UTF8.GetString(packet, cursor, nextFieldLength);
                    cursor += nextFieldLength;
                    int eq = tag.IndexOf('=');
                    if (eq > 0)
                    {
                        string key = tag.Substring(0, eq);
                        string val = tag.Substring(eq + 1);
                        returnVal.fields[key] = val;
                    }
                }
            }

            return returnVal;
        }
    }
}
