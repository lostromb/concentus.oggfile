using Concentus.Oggfile;
using Concentus.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OggTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (FileStream fileIn = new FileStream(@"C:\Users\logan\Documents\Visual Studio 2015\Projects\concentus.oggfile\OggTest\Rocket.opus", FileMode.Open))
            {
                OpusDecoder decoder = OpusDecoder.Create(48000, 2);

                OggContainerReader reader = new OggContainerReader(fileIn, false);
                reader.FindNextStream();
                int streamSerial = reader.StreamSerials[0];
                IPacketProvider packetProvider = reader.GetStream(streamSerial);
                DataPacket packet = packetProvider.GetNextPacket();
                while (packet != null)
                {
                    Console.WriteLine("Reading packet with length " + packet.Length);
                    byte[] buf = new byte[packet.Length];
                    packet.Read(buf, 0, packet.Length);

                    string packetContents = Encoding.UTF8.GetString(buf, 0, buf.Length);
                    if (packetContents.StartsWith("OpusHead"))
                    {
                        Console.WriteLine("Got header: " + packetContents);
                    }
                    else if (packetContents.StartsWith("OpusTags"))
                    {
                        Console.WriteLine("Got tags: " + packetContents);
                        OpusTagsPacket.Parse(buf, buf.Length);
                    }
                    else
                    {
                        Console.WriteLine("Decoding packet...");
                        OpusPacketInfo info = OpusPacketInfo.ParseOpusPacket(buf, 0, buf.Length);
                        int numSamples = OpusPacketInfo.GetNumSamples(buf, 0, buf.Length, 48000);
                        Console.WriteLine("Number of samples = " + numSamples);
                        short[] output = new short[numSamples * 2];
                        decoder.Decode(buf, 0, buf.Length, output, 0, numSamples, false);
                    }

                    packet.Done();
                    packet = packetProvider.GetNextPacket();
                }

                Console.WriteLine("Stream ended");
            }
        }
    }
}
