This is an experimental package for implementing Oggfile parsing support to decode .opus files using Concentus. Only the decoder path is implemented, and the sample code is thus:

using (FileStream fileIn = new FileStream("file.opus", FileMode.Open))
{
    OpusOggReadStream reader = new OpusOggReadStream(fileIn, 48000, false);
    OpusTags tags = reader.Tags;
    while (reader.HasNextPacket)
    {
        short[] packet = reader.DecodeNextPacket();
        Console.WriteLine(packet.Length + " samples decoded");
    }
    Console.WriteLine("Stream ended");
}