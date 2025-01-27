using System.IO;
using System.Text;
using System.Xml;
using DeepL;

namespace XmlTranslate.src
{
    internal class XmlTranslate
    {
        public static void Main(string[] args)
        {

        }
        private static void ProcessPath(string inputPath)
        {
            if (File.Exists(inputPath))
            {

            }
        }
        private static void TransformXML(string inputPath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(inputPath);
            // Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            xml.Load(inputPath);

        }

    }
}
