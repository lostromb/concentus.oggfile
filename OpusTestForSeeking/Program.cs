using Concentus.Oggfile;
using Concentus.Structs;
using System;
using System.IO;

namespace OpusTestForSeeking
{
    class Program
    {
        static void Main(string[] args)
        {
            string opusFile = "opus.opus";
            string outFile = "out.raw";
            string out2File = "out2.raw";

            using (var fileIn = new FileStream(opusFile, FileMode.Open))
            {
                using (var fileOut = new FileStream(outFile, FileMode.Create))
                {
                    var decoder = OpusDecoder.Create(48000, 2);
                    var opusOggReadStream = new OpusOggReadStream(decoder, fileIn);
                    while (opusOggReadStream.HasNextPacket)
                    {
                        short[] packet = opusOggReadStream.DecodeNextPacket();
                        if (packet != null)
                        {
                            byte[] buffer = new byte[packet.Length * 2];
                            Buffer.BlockCopy(packet, 0, buffer, 0, buffer.Length);
                            fileOut.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }

            using (var fileIn = new FileStream(opusFile, FileMode.Open))
            {
                using (var fileOut2 = new FileStream(out2File, FileMode.Create))
                {
                    var decoder = OpusDecoder.Create(48000, 2);
                    var opusOggReadStream = new OpusOggReadStream(decoder, fileIn);

                    long positionAt1Minute = Convert.ToInt64(48000 * 60);
                    opusOggReadStream.SeekTo(positionAt1Minute);

                    while (opusOggReadStream.HasNextPacket)
                    {
                        short[] packet = opusOggReadStream.DecodeNextPacket();
                        if (packet != null)
                        {
                            byte[] buffer = new byte[packet.Length * 2];
                            Buffer.BlockCopy(packet, 0, buffer, 0, buffer.Length);
                            fileOut2.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
        }
    }
}