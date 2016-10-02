using Concentus.Enums;
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
            using (FileStream fileOut = new FileStream(@"C:\Users\Logan Stromberg\Desktop\Prisencolinensinainciusol.opus", FileMode.Create))
            {
                OpusEncoder encoder = OpusEncoder.Create(48000, 2, OpusApplication.OPUS_APPLICATION_AUDIO);
                encoder.Bitrate = 96000;

                OpusTags tags = new OpusTags();
                tags.Comment = "Encoded by Logan";
                tags.Fields[OpusTagName.Title] = "Prisencolinensinainciusol";
                tags.Fields[OpusTagName.Artist] = "Adriano Celetano";
                OpusOggWriteStream oggOut = new OpusOggWriteStream(encoder, 48000, true, fileOut, tags);

                byte[] allInput = File.ReadAllBytes(@"C:\Users\Logan Stromberg\Desktop\Prisencolinensinainciusol.raw");
                short[] samples = BytesToShorts(allInput);

                oggOut.WriteSamples(samples, 0, samples.Length);
                oggOut.Finish();
            }
        }

        /// <summary>
        /// Converts interleaved byte samples (such as what you get from a capture device)
        /// into linear short samples (that are much easier to work with)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static short[] BytesToShorts(byte[] input)
        {
            return BytesToShorts(input, 0, input.Length);
        }

        /// <summary>
        /// Converts interleaved byte samples (such as what you get from a capture device)
        /// into linear short samples (that are much easier to work with)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static short[] BytesToShorts(byte[] input, int offset, int length)
        {
            short[] processedValues = new short[length / 2];
            for (int c = 0; c < processedValues.Length; c++)
            {
                processedValues[c] = (short)(((int)input[(c * 2) + offset]) << 0);
                processedValues[c] += (short)(((int)input[(c * 2) + 1 + offset]) << 8);
            }

            return processedValues;
        }
    }
}
