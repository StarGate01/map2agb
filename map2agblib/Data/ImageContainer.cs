using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;

namespace map2agblib.Data
{

    [DataContract]
    public class ImageContainer : IImportExport<ImageContainer>
    {

        public Image Image { get; set; }

        ImageContainer IImportExport<ImageContainer>.ImportFromFile(string filePath)
        {
            return new ImageContainer(Image.FromFile(filePath));
        }

        public void ExportToFile(ImageContainer data, string filePath)
        {
            data.Image.Save(filePath);
        }

        public ImageContainer(Image image)
        {
            Image = image;
        }

        public ImageContainer()
        {

        }

    }

}
