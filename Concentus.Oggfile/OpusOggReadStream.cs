using Concentus.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concentus.Oggfile
{
    /// <summary>
    /// Provides functionality to decode a basic .opus Ogg file, decoding the audio packets individually and returning them. Tags are also parsed if present.
    /// Note that this currently assumes the input file only has 1 elementary stream; anything more advanced than that will probably not work.
    /// </summary>
    public class OpusOggReadStream
    {
        private Stream _inputStream;
        private byte[] _nextDataPacket;
        private OpusDecoder _decoder;
        private OpusTags _tags;
        private IPacketProvider _packetProvider;
        private bool _endOfStream;
        private int _decoderChannels;
        private string _lastError;

        /// <summary>
        /// Builds an Ogg file reader that decodes Opus packets from the given input stream, using a 
        /// specified output sample rate and channel count. This will implicitly build an OpusDecoder object
        /// and return the decoded PCM buffers directly.
        /// </summary>
        /// <param name="oggFileInput">The input stream for an Ogg formatted .opus file</param>
        /// <param name="outputSampleRate">The sample rate to decode to. This is not necessarily the sample
        /// rate the audio was encoded at. Generally there is no reason to change this from the default 48K.
        /// This must be a valid Opus sample rate (48K / 24K / 16K / 12K / 8K)</param>
        /// <param name="decodeStereo">If true, decode each packet into interleaved stereo PCM format</param>
        public OpusOggReadStream(Stream oggFileInput, int outputSampleRate = 48000, bool decodeStereo = true)
        {
            _inputStream = oggFileInput;
            _endOfStream = false;
            _decoderChannels = decodeStereo ? 2 : 1;
            if (!Initialize(outputSampleRate, _decoderChannels))
            {
                _endOfStream = true;
            }
        }

        /// <summary>
        /// Gets the tags that were parsed from the OpusTags Ogg packet, or NULL if no such packet was found.
        /// </summary>
        public OpusTags Tags
        {
            get
            {
                return _tags;
            }
        }

        /// <summary>
        /// Returns true if there is still another data packet to be decoded from the current Ogg stream.
        /// Note that this decoder currently only assumes that the input has 1 elementary stream with no splices
        /// or other fancy things.
        /// </summary>
        public bool HasNextPacket
        {
            get
            {
                return !_endOfStream;
            }
        }

        /// <summary>
        /// If an error happened either in stream initialization, reading, or decoding, the message will appear here.
        /// </summary>
        public string LastError
        {
            get
            {
                return _lastError;
            }
        }

        /// <summary>
        /// Reads the next packet from the Ogg stream and decodes it, returning the decoded PCM buffer.
        /// If there are no more packets to decode, this returns NULL. If an error occurs, this also returns
        /// NULL and puts the error message into the LastError field
        /// </summary>
        /// <returns>The decoded audio for the next packet in the stream, or NULL</returns>
        public short[] DecodeNextPacket()
        {
            if (_endOfStream)
            {
                return null;
            }

            try
            {
                int numSamples = OpusPacketInfo.GetNumSamples(_nextDataPacket, 0, _nextDataPacket.Length, 48000);
                short[] output = new short[numSamples * _decoderChannels];
                _decoder.Decode(_nextDataPacket, 0, _nextDataPacket.Length, output, 0, numSamples, false);
                QueueNextPacket();
                return output;
            }
            catch (OpusException e)
            {
                _lastError = "Opus decoder threw exception: " + e.Message;
                return null;
            }
        }

        /// <summary>
        /// Creates an opus decoder and reads from the ogg stream until a data packet is encountered,
        /// queuing it up for future decoding. Tags are also parsed if they are encountered.
        /// </summary>
        /// <param name="outputSampleRate">The sample rate to decode to. Must be a valid Opus sample rate</param>
        /// <param name="numChannels">The number of channels to decode to (1 or 2)</param>
        /// <returns>True if the stream is valid and ready to be decoded</returns>
        private bool Initialize(int outputSampleRate, int numChannels)
        {
            try
            {
                _decoder = OpusDecoder.Create(outputSampleRate, numChannels);
            
                OggContainerReader reader = new OggContainerReader(_inputStream, true);
                reader.FindNextStream();
                if (reader.StreamSerials.Length == 0)
                {
                    _lastError = "Initialization failed: No elementary streams found in input file";
                    return false;
                }

                int streamSerial = reader.StreamSerials[0];
                _packetProvider = reader.GetStream(streamSerial);
                QueueNextPacket();

                return true;
            }
            catch (OpusException e)
            {
                _lastError = "Initialization failed: Could not create Opus decoder: " + e.Message;
                return false;
            }
            catch (Exception e)
            {
                _lastError = "Unknown initialization error: " + e.Message;
                return false;
            }
        }

        /// <summary>
        /// Looks for the next opus data packet in the Ogg stream and queues it up.
        /// If the end of stream has been reached, this does nothing.
        /// </summary>
        private void QueueNextPacket()
        {
            if (_endOfStream)
            {
                return;
            }

            DataPacket packet = _packetProvider.GetNextPacket();
            if (packet == null)
            {
                _endOfStream = true;
                _nextDataPacket = null;
                return;
            }

            byte[] buf = new byte[packet.Length];
            packet.Read(buf, 0, packet.Length);
            packet.Done();

            if (buf.Length > 8 && "OpusHead".Equals(Encoding.UTF8.GetString(buf, 0, 8)))
            {
                QueueNextPacket();
            }
            else if (buf.Length > 8 && "OpusTags".Equals(Encoding.UTF8.GetString(buf, 0, 8)))
            {
                _tags = OpusTags.ParsePacket(buf, buf.Length);
                QueueNextPacket();
            }
            else
            {
                _nextDataPacket = buf;
            }
        }
    }
}
