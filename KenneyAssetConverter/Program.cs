using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KenneyAssetConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Error: must specify path of xml file to convert.");
                return;
            }

            //Load the xml
            string filePath = args[0];
            Console.WriteLine($"Loading XML from: {filePath}");
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            //Begin processing data
            StringBuilder outputAtlas = new StringBuilder();
            string output = args[1];

            XmlNode fileSpecifications = doc.SelectSingleNode("//*[local-name()='TextureAtlas']");
            string fileName = fileSpecifications.Attributes["imagePath"].Value;
            outputAtlas.AppendLine(fileName);
            outputAtlas.AppendLine("format: RGBA8888");
            outputAtlas.AppendLine("filter: Linear,Linear");
            outputAtlas.AppendLine("repeat: none");

            XmlNodeList subTextures = fileSpecifications.SelectNodes(".//*[local-name()='SubTexture']");
            foreach (XmlNode subTexture in subTextures)
            {
                string name = subTexture.Attributes["name"].Value;
                string x = subTexture.Attributes["x"].Value;
                string y = subTexture.Attributes["y"].Value;
                string width = subTexture.Attributes["width"].Value;
                string height = subTexture.Attributes["height"].Value;

                name = name.Substring(0, name.LastIndexOf('.'));
                outputAtlas.AppendLine($"{name}\n  rotate: false\n  xy: {x}, {y}\n  size: {width}, {height}\n  orig: {width}, {height}\n  offset: 0, 0\n  index: -1");
            }
            File.WriteAllText(output, outputAtlas.ToString());
        }
    }
}
