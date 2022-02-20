using Concentus.Enums;
using Concentus.Oggfile;
using Concentus.Structs;
using EricOulashin;
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
            string opusfile = "C:\\Users\\ayaz\\Downloads\\speech2.opus"; // ogg
            string rawFile = "C:\\Users\\ayaz\\Downloads\\a2002011001-e02-8kHz.wav";
            string rawFile2 = "C:\\Users\\ayaz\\Downloads\\speech2.raw";
            string rawFile2Wav = "C:\\Users\\ayaz\\Downloads\\speech3.wav";
            using (FileStream fileOut = new FileStream(opusfile, FileMode.Create))
            {
                OpusEncoder encoder = new OpusEncoder(8000, 2, OpusApplication.OPUS_APPLICATION_AUDIO);
                encoder.Bitrate = 96000;

                OpusTags tags = new OpusTags();
                tags.Fields[OpusTagName.Title] = "Prisencolinensinainciusol";
                tags.Fields[OpusTagName.Artist] = "Adriano Celetano";
                OpusOggWriteStream oggOut = new OpusOggWriteStream(encoder, fileOut, tags);

                byte[] allInput = File.ReadAllBytes(rawFile);
                short[] samples = BytesToShorts(allInput);

                oggOut.WriteSamples(samples, 0, samples.Length);
                oggOut.Finish();
            }

            using (FileStream fileIn = new FileStream(opusfile, FileMode.Open))
            {
                using (FileStream fileOut = new FileStream(rawFile2, FileMode.Create))
                {
                    OpusDecoder decoder = new OpusDecoder(8000, 2);
                    OpusOggReadStream oggIn = new OpusOggReadStream(decoder, fileIn);

                    var wavfile = new WAVFile();
                    wavfile.Create(rawFile2Wav, false, 8000, 16);

                    while (oggIn.HasNextPacket)
                    {
                        short[] packet = oggIn.DecodeNextPacket();
                        if (packet != null)
                        {
                            byte[] binary = ShortsToBytes(packet);

                            // to raw
                            fileOut.Write(binary, 0, binary.Length);

                            // to wav
                            packet.Where(_=>_%6==0)48 // 48 = 6
                                .ToList().ForEach(_=>wavfile.AddSample_16bit(_));
                            // or old way:
                            //for (int c = 0; c < packet.Length; c += 6) // 48/8=6
                            //{
                            //    var g1 = (byte)(packet[c] & 0xFF);
                            //    var g2 = (byte)((packet[c] >> 8) & 0xFF);
                            //    f.AddSample_ByteArray(new[] { g1, g2 });
                            //}
                        }
                    }
                    wavfile.Close();
                }
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

        /// <summary>
        /// Converts linear short samples into interleaved byte samples, for writing to a file, waveout device, etc.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] ShortsToBytes(short[] input)
        {
            return ShortsToBytes(input, 0, input.Length);
        }

        /// <summary>
        /// Converts linear short samples into interleaved byte samples, for writing to a file, waveout device, etc.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] ShortsToBytes(short[] input, int in_offset, int samples)
        {
            byte[] processedValues = new byte[samples * 2];
            ShortsToBytes(input, in_offset, processedValues, 0, samples);
            return processedValues;
        }

        /// <summary>
        /// Converts linear short samples into interleaved byte samples, for writing to a file, waveout device, etc.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static void ShortsToBytes(short[] input, int in_offset, byte[] output, int out_offset, int samples)
        {
            for (int c = 0; c < samples; c++)
            {
                output[(c * 2) + out_offset] = (byte)(input[c + in_offset] & 0xFF);
                output[(c * 2) + out_offset + 1] = (byte)((input[c + in_offset] >> 8) & 0xFF);
            }
        }
    }
}
