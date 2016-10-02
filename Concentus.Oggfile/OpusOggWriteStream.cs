using Concentus.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concentus.Oggfile
{
    public class OpusOggWriteStream
    {
        private OpusEncoder _encoder;
        private Stream _outputStream;

        public OpusOggWriteStream(OpusEncoder encoder, Stream outputStream)
        {
            _encoder = encoder;
            _outputStream = outputStream;
        }

        public void WriteSamples(short[] data, int offset, int count)
        {

        }

        private void EncodeFrameIfPossible()
        {

        }

        public void Finish()
        {
            // See how much data we need to write to fill in the next packet

            // Write it as silence

            //Then finish the ogg file

        }
    }
}
