using Concentus.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concentus.Oggfile
{
    public class OpusOggReadStream
    {
        private Stream _inputStream;
        private byte[] _nextDataPacket;
        private OpusDecoder _decoder;
        private OpusTagsPacket _tags;
        private IPacketProvider _packetProvider;
        private bool _endOfStream;

        public OpusOggReadStream(Stream oggFileInput, int outputSampleRate, bool decodeStereo)
        {
            _inputStream = oggFileInput;
            _endOfStream = false;
            if (!Initialize(outputSampleRate, decodeStereo))
            {
                _endOfStream = true;
            }
        }

        public OpusTagsPacket Tags
        {
            get
            {
                return _tags;
            }
        }

        public bool HasNextPacket
        {
            get
            {
                return !_endOfStream;
            }
        }

        public short[] DecodeNextPacket()
        {
            OpusPacketInfo info = OpusPacketInfo.ParseOpusPacket(_nextDataPacket, 0, _nextDataPacket.Length);
            int numSamples = OpusPacketInfo.GetNumSamples(_nextDataPacket, 0, _nextDataPacket.Length, 48000);
            short[] output = new short[numSamples * 2];
            _decoder.Decode(_nextDataPacket, 0, _nextDataPacket.Length, output, 0, numSamples, false);
            QueueNextPacket();
            return output;
        }

        private bool Initialize(int outputSampleRate, bool decodeStereo)
        {
            _decoder = OpusDecoder.Create(outputSampleRate, decodeStereo ? 2 : 1);

            OggContainerReader reader = new OggContainerReader(_inputStream, true);
            reader.FindNextStream();
            if (reader.StreamSerials.Length == 0)
            {
                return false;
            }

            int streamSerial = reader.StreamSerials[0];
            _packetProvider = reader.GetStream(streamSerial);
            QueueNextPacket();

            return true;
        }

        private void QueueNextPacket()
        {
            if (!_endOfStream)
            {
                DataPacket packet = _packetProvider.GetNextPacket();
                if (packet == null)
                {
                    _endOfStream = true;
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
                    _tags = OpusTagsPacket.Parse(buf, buf.Length);
                    QueueNextPacket();
                }
                else
                {
                    _nextDataPacket = buf;
                }
            }
        }
    }
}
