using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace map2agblib.Imaging.JASCPAL
{

    public class JASCPALImport
    {

        /// <summary>
        /// Imports a palette from a jasc-pal file
        /// </summary>
        /// <param name="filePath">The source file path</param>
        /// <returns>The palette</returns>
        public static Color[] Import(string filePath)
        {
            string[] fileContent = File.ReadAllLines(filePath);
            if (fileContent[0] != "JASC-PAL") throw new InvalidDataException("No JASC-PAL file");
            int numColors = Convert.ToInt32(fileContent[2]);
            return fileContent.Skip(3).Take(numColors).Select(p => p.Split(' ').Select(k => Convert.ToByte(k)).ToArray()).Select(p => Color.FromArgb(p[0], p[1], p[2])).ToArray();
        }

    }

}
