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
            using (FileStream fileIn = new FileStream(@"C:\Users\logan\Documents\Visual Studio 2015\Projects\concentus.oggfile\OggTest\Rocket.opus", FileMode.Open))
            {
                

                Console.WriteLine("Stream ended");
            }
        }
    }
}
