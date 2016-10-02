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
    /// A class for writing audio data as an .opus Ogg stream, using an Opus encoder provided in the constructor.
    /// This will handle all of the buffering, packetization and Ogg container work in order to output standard-compliant
    /// .opus files that can be played universally.
    /// </summary>
    public class OpusOggWriteStream
    {
        private OpusEncoder _encoder;
        private Stream _outputStream;
        private Crc crc;

        private short[] _opusFrame;
        private int _opusFrameSamples;
        private int _opusFrameIndex;
        private byte[] _currentHeader = new byte[4000];
        private byte[] _currentPayload = new byte[65536];
        private int _headerIndex = 0;
        private int _payloadIndex = 0;
        private int _pageCounter = 0;
        private int _logicalStreamId = 1;
        private long _granulePosition = 0;
        private byte _segmentCount = 0;
        private const int PAGE_FLAGS_POS = 5;
        private const int CHECKSUM_HEADER_POS = 22;
        private const int SEGMENT_COUNT_POS = 26;
        private bool _finalized = false;

        /// <summary>
        /// Constructs a stream that will accept PCM audio input, and automatically encode it to Opus and packetize it using Ogg,
        /// writing the output pages to an underlying stream (usually a file stream).
        /// You are allowed to change the encoding parameters mid-stream using the properties of the OpusEncoder; the only thing you
        /// cannot change is the sample rate and num# of channels.
        /// </summary>
        /// <param name="encoder">An opus encoder to use for output</param>
        /// <param name="inputSampleRate">The sample rate to interpret the input as</param>
        /// <param name="stereoEncoding">If true, assume input PCM is interleaved stereo</param>
        /// <param name="outputStream">A base stream to accept the encoded ogg file output</param>
        /// <param name="fileTags">A set of tags to include in the encoded file</param>
        public OpusOggWriteStream(OpusEncoder encoder, int inputSampleRate, bool stereoEncoding, Stream outputStream, OpusTags fileTags = null)
        {
            _encoder = encoder;
            _outputStream = outputStream;
            _opusFrameIndex = 0;
            _granulePosition = 0;
            _opusFrameSamples = (int)((long)inputSampleRate * 20 / 1000);
            _opusFrame = new short[_opusFrameSamples * (stereoEncoding ? 2 : 1)];
            crc = new Crc();
            BeginNewPage();
            WriteOpusHeadPage();
            WriteOpusTagsPage(fileTags);
        }

        /// <summary>
        /// Writes a buffer of PCM audio samples to the encoder and packetizer. Runs Opus encoding and potentially outputs one or more pages to the underlying Ogg stream.
        /// </summary>
        /// <param name="data">The audio samples to write</param>
        /// <param name="offset">The offset to use when reading data</param>
        /// <param name="count">The amount of PCM data to write (if data is stereo, remember that this is 2x the number of samples per frame)</param>
        public void WriteSamples(short[] data, int offset, int count)
        {
            if (_finalized)
            {
                throw new InvalidOperationException("Cannot write new samples to Oggfile, the output stream is already closed!");
            }

            // Try and fill the opus frame
            int inputCursor = 0;
            int amountToWrite = Math.Min(_opusFrame.Length - _opusFrameIndex, count - inputCursor);
            while (amountToWrite > 0)
            {
                Array.Copy(data, offset + inputCursor, _opusFrame, _opusFrameIndex, amountToWrite);
                _opusFrameIndex += amountToWrite;
                inputCursor += amountToWrite;
                if (_opusFrameIndex == _opusFrame.Length)
                {
                    // Frame is finished. Encode it
                    int packetSize = _encoder.Encode(_opusFrame, 0, _opusFrameSamples, _currentPayload, _payloadIndex, 1275);
                    _payloadIndex += packetSize;
                    _granulePosition += _opusFrameSamples; // fixme: convert from 48Khz if using another timecode

                    // And update the lacing values in the header
                    int segmentLength = packetSize;
                    while (segmentLength > 255)
                    {
                        segmentLength -= 255;
                        _currentHeader[_headerIndex++] = 0xFF;
                    }
                    _currentHeader[_headerIndex++] = (byte)segmentLength;

                    // Now increment segment count
                    _segmentCount++;

                    // And finalize the page if we need
                    if (_segmentCount == 255)
                    {
                        FinalizePage();
                    }

                    _opusFrameIndex = 0;
                }

                amountToWrite = Math.Min(_opusFrame.Length - _opusFrameIndex, count - inputCursor);
            }
        }

        /// <summary>
        /// Call when you are finished encoding your file. This operation will close the underlying stream as well.
        /// </summary>
        public void Finish()
        {
            // Just see how many samples we need to fill in the opus buffer, and write silence.
            int samplesToWrite = _opusFrame.Length - _opusFrameIndex;
            short[] paddingSamples = new short[samplesToWrite];
            WriteSamples(paddingSamples, 0, samplesToWrite);

            // TODO: Set page flag to end of logical stream
            // _currentHeader[PAGE_FLAGS_POS] = (byte)PageFlags.EndOfStream;

            // Finalize the page if it was not just finalized right then
            FinalizePage();
            // Now close our output
            _outputStream.Flush();
            _outputStream.Dispose();
            _finalized = true;
        }

        /// <summary>
        /// Writes the Ogg page for OpusHead, containing encoder information
        /// </summary>
        private void WriteOpusHeadPage()
        {
            byte[] opusHead = Encoding.UTF8.GetBytes("OpusHead");
            Array.Copy(opusHead, 0, _currentPayload, _payloadIndex, opusHead.Length);
            _payloadIndex += opusHead.Length;
            // FIXME hardcoded values for now
            _currentPayload[_payloadIndex++] = 0x01;
            _currentPayload[_payloadIndex++] = 0x02;
            _currentPayload[_payloadIndex++] = 0x38;
            _currentPayload[_payloadIndex++] = 0x01;
            _currentPayload[_payloadIndex++] = 0x80;
            _currentPayload[_payloadIndex++] = 0xBB;
            _currentPayload[_payloadIndex++] = 0x00;
            _currentPayload[_payloadIndex++] = 0x00;
            _currentPayload[_payloadIndex++] = 0x00;
            _currentPayload[_payloadIndex++] = 0x00;
            _currentPayload[_payloadIndex++] = 0x00;
            // Write segment data
            _currentHeader[_headerIndex++] = (byte)_payloadIndex;
            _segmentCount++;
            // Set page flag to start of logical stream
            _currentHeader[PAGE_FLAGS_POS] = (byte)PageFlags.BeginningOfStream;
            FinalizePage();
        }

        /// <summary>
        /// Writes an Ogg page for the OpusTags, given an input tag set
        /// </summary>
        /// <param name="tags"></param>
        private void WriteOpusTagsPage(OpusTags tags = null)
        {
            if (tags == null)
            {
                tags = new OpusTags();
            }

            byte[] opusHead = Encoding.UTF8.GetBytes("OpusTags");
            Array.Copy(opusHead, 0, _currentPayload, _payloadIndex, opusHead.Length);
            _payloadIndex += opusHead.Length;

            // Empty tags for now (implement later)
            for (int c = 0; c < 8; c++)
            {
                _currentPayload[_payloadIndex++] = 0x00;
            }

            // Write segment data
            _currentHeader[_headerIndex++] = (byte)_payloadIndex;
            _segmentCount++;
            FinalizePage();
        }

        /// <summary>
        /// Clears all buffers and prepares a new page with an empty header
        /// </summary>
        private void BeginNewPage()
        {
            _headerIndex = 0;
            _payloadIndex = 0;
            _segmentCount = 0;

            // "OggS"
            _currentHeader[_headerIndex++] = 0x4f;
            _currentHeader[_headerIndex++] = 0x67;
            _currentHeader[_headerIndex++] = 0x67;
            _currentHeader[_headerIndex++] = 0x53;
            // Stream version 0
            _currentHeader[_headerIndex++] = 0x0;
            // Header flags
            _currentHeader[_headerIndex++] = (byte)PageFlags.None;
            // Granule position (for opus, it is the number of 48Khz pcm samples encoded)
            _headerIndex += WriteValueUsingOggEndianness(_granulePosition, _currentHeader, _headerIndex);
            // Logical stream serial number
            _headerIndex += WriteValueUsingOggEndianness(_logicalStreamId, _currentHeader, _headerIndex);
            // Page sequence number
            _headerIndex += WriteValueUsingOggEndianness(_pageCounter, _currentHeader, _headerIndex);
            // Checksum is initially zero
            _currentHeader[_headerIndex++] = 0x0;
            _currentHeader[_headerIndex++] = 0x0;
            _currentHeader[_headerIndex++] = 0x0;
            _currentHeader[_headerIndex++] = 0x0;
            // Number of segments, initially zero
            _currentHeader[_headerIndex++] = _segmentCount;
            // Segment table goes after this point, once we have packets in this page

            _pageCounter++;
        }

        /// <summary>
        /// If the number of segments is nonzero, finalizes the page into a contiguous buffer, calculates CRC, and writes the page to the output stream
        /// </summary>
        private void FinalizePage()
        {
            if (_finalized)
            {
                throw new InvalidOperationException("Cannot finalize page, the output stream is already closed!");
            }

            if (_segmentCount != 0)
            {
                // Write the final segment count to the header
                _currentHeader[SEGMENT_COUNT_POS] = _segmentCount;
                // Build the complete page from the separate buffers
                int pageLength = _headerIndex + _payloadIndex;
                byte[] newPage = new byte[pageLength];
                Array.Copy(_currentHeader, 0, newPage, 0, _headerIndex);
                Array.Copy(_currentPayload, 0, newPage, _headerIndex, _payloadIndex);
                // Calculate CRC and update the header
                crc.Reset();
                for (int c = 0; c < pageLength; c++)
                {
                    crc.Update(newPage[c]);
                }
                WriteValueUsingOggEndianness(crc.Value, newPage, CHECKSUM_HEADER_POS);
                // Write the page to the stream (TODO: Make sure this operation does not overflow any target stream buffers?)
                _outputStream.Write(newPage, 0, pageLength);
                // And reset the page
                BeginNewPage();
            }
        }

        private static int WriteValueUsingOggEndianness(int val, byte[] target, int targetOffset)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Copy(bytes, 0, target, targetOffset, 4);
            return 4;
        }

        private static int WriteValueUsingOggEndianness(long val, byte[] target, int targetOffset)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Copy(bytes, 0, target, targetOffset, 8);
            return 8;
        }

        private static int WriteValueUsingOggEndianness(uint val, byte[] target, int targetOffset)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Copy(bytes, 0, target, targetOffset, 4);
            return 4;
        }
    }
}
