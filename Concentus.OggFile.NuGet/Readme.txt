This is an experimental package for implementing Oggfile parsing support to decode .opus files using Concentus.

TO ENCODE:

using (FileStream fileOut = new FileStream(opusfile, FileMode.Create))
{
    OpusEncoder encoder = OpusEncoder.Create(48000, 2, OpusApplication.OPUS_APPLICATION_AUDIO);
    encoder.Bitrate = 96000;

    OpusTags tags = new OpusTags();
    tags.Fields[OpusTagName.Title] = "Song Title";
    tags.Fields[OpusTagName.Artist] = "Artist";
    OpusOggWriteStream oggOut = new OpusOggWriteStream(encoder, 48000, true, fileOut, tags);

	while (HasMoreAudio())
	{
		short[] packet = ReadSomeAudio();
		oggOut.WriteSamples(packet, 0, packet.Length);
	}
    
    oggOut.Finish();
}

TO DECODE:

using (FileStream fileIn = new FileStream(opusfile, FileMode.Open))
{
    OpusOggReadStream oggIn = new OpusOggReadStream(fileIn, 48000, true);
    while (oggIn.HasNextPacket)
    {
        short[] packet = oggIn.DecodeNextPacket();
        if (packet != null)
        {
            DoSomethingWithAudio(packet);
        }
    }
}