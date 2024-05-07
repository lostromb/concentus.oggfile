using Concentus.Oggfile;
using Concentus.Structs;

namespace OpusTimeShift
{
    public class Program
    {
        // An encoded Opus frame representing 10ms of silence.
        private static readonly byte[] SILENCE_FRAME_10MS_48K = new byte[] { 0x20, 0x0b, 0xe5, 0xf3, 0xb2, 0x31 };

        public static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("USAGE: OpusTimeShift.exe \"INPUT FILE.opus\" {TIME DELTA MS} \"Output File.opus\"");
                Console.WriteLine("To make the file 500ms shorter: OpusTimeShift.exe \"input.opus\" -500 \"output.opus\"");
                Console.WriteLine("To make the file 150ms longer: OpusTimeShift.exe \"input.opus\" 150 \"output.opus\"");
                return;
            }

            string inputFileName = args[0];
            string rawDelta = args[1];
            string outputFileName = args[2];
            double parsedDelta;

            if (!File.Exists(inputFileName))
            {
                Console.WriteLine("Input file {0} does not exist", inputFileName);
                return;
            }
            if (!double.TryParse(rawDelta, out parsedDelta))
            {
                Console.WriteLine("Could not parse delta {0}", rawDelta);
                return;
            }

            TimeSpan deltaTime = TimeSpan.FromMilliseconds(parsedDelta);

            using (FileStream inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            using (FileStream outputFileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
            {
                OpusOggReadStream readStream = new OpusOggReadStream(null, inputFileStream);
                OpusRawWriteStream writeStream = new OpusRawWriteStream(outputFileStream, readStream.Tags, 48000, 2);

                if (deltaTime < TimeSpan.Zero)
                {
                    // Subtract time from the track by deleting packets
                    while (deltaTime < TimeSpan.Zero)
                    {
                        byte[] discardPacket = readStream.ReadNextRawPacket();
                        if (discardPacket == null)
                        {
                            break;
                        }
                        else
                        {
                            int samplesInThisPacket = OpusPacketInfo.GetNumSamplesPerFrame(discardPacket.AsSpan(), 48000);
                            //Console.WriteLine("Discarding packet, data length {0}, sample length {1}, cut time remaining {2}ms", discardPacket.Length, samplesInThisPacket, deltaTime.TotalMilliseconds);
                            deltaTime += TimeSpan.FromSeconds((double)samplesInThisPacket / 48000.0);
                        }
                    }
                }
                else
                {
                    // Add time to the track using silence padding
                    while (deltaTime > TimeSpan.Zero)
                    {
                        //Console.WriteLine("Inserting silence packet, pad time remaining {0}ms", deltaTime.TotalMilliseconds);
                        writeStream.WriteSinglePacket(SILENCE_FRAME_10MS_48K, 0, SILENCE_FRAME_10MS_48K.Length);
                        deltaTime -= TimeSpan.FromMilliseconds(10);
                    }
                }

                // Now copy the rest of the stream straight across
                while (readStream.HasNextPacket)
                {
                    byte[] copyPacket = readStream.ReadNextRawPacket();
                    if (copyPacket != null)
                    {
                        //Console.WriteLine("Copying packet, data length {0}, sample length {1}", copyPacket.Length, OpusPacketInfo.GetNumSamplesPerFrame(copyPacket, 0, 48000));
                        writeStream.WriteSinglePacket(copyPacket, 0, copyPacket.Length);
                    }
                }

                writeStream.Finish();
            }
        }
    }
}