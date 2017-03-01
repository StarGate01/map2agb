using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace map2agblib.Imaging.IO
{

    public class JASCPAL
    {

        /// <summary>
        /// Imports a palette from a jasc-pal file
        /// </summary>
        /// <param name="filePath">The source file path</param>
        /// <returns>The palette</returns>
        public static Palette Import(string filePath)
        {
            string[] fileContent = File.ReadAllLines(filePath);
            if (fileContent[0] != "JASC-PAL") throw new InvalidDataException("No JASC-PAL file");
            int numColors = Convert.ToInt32(fileContent[2]);
            if (numColors != 16) throw new InvalidDataException("Palette has not 16 colors");
            return new Palette(fileContent.Skip(3).Take(16).Select(p => p.Split(' ').Select(k => Convert.ToByte(k)).ToArray()).Select(p => new ShortColor((byte)(p[0] >> 3), (byte)(p[1] >> 3), (byte)(p[2] >> 3))).ToArray());
        }

        /// <summary>
        /// Exports a palette to a jasc-pal file
        /// </summary>
        /// <param name="palette">The source palette</param>
        /// <param name="filePath">The target file path</param>
        public static void Export(Palette palette, string filePath)
        {
            if (palette.Colors.Length != 16) throw new InvalidDataException("Palette has not 16 colors");
            string[] header = new string[] { "JASC-PAL", "0100", "16" };
            IEnumerable<string> fileContent = header.Concat(palette.Colors.Select(p => (p.Red << 3) + " " + (p.Green << 3) + " " + (p.Blue << 3)));
            File.WriteAllLines(filePath, fileContent);
        }

    }

}
