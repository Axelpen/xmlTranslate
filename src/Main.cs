using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using DeepL;

namespace XmlTranslate.src
{
    internal class XmlTranslate
    {
        public static void Main(string[] args)
        {
            if (args == null)
            {
                Console.WriteLine("input path please");
            }
            Directory.CreateDirectory(Path.Combine(args[0], "output"));
            ProcessPath(args[0]);

        }




        private static async Task<string> TranslateText(string nodeText)
        {
            var authKey = ""; //to be changed
            var translator = new Translator(authKey);

            try
            {
                var translatedText = await translator.TranslateTextAsync(
                    nodeText,
                    LanguageCode.German,
                    LanguageCode.Polish
                    );

                return translatedText.Text;
            }
            catch (DeepLException e)
            {
                Console.WriteLine($"oh noo {e}");
                return nodeText;
            }
        }

        private static void ProcessPath(string inputPath)
        {
            if (File.Exists(inputPath))
            {
                TransformXML(inputPath, Path.Combine(inputPath, "output"));
            }
            if (Directory.Exists(inputPath))
            {
                string[] xmlFiles = Directory.GetFiles(inputPath, "*.xml", SearchOption.TopDirectoryOnly);
                foreach (string xmlFile in xmlFiles)
                {
                    TransformXML(xmlFile, Path.Combine(inputPath, "output")); ;
                }
            }
        }

        private static async void TransformXML(string inputPath, string outputPath)
        {
            XmlDocument xml = new();
            xml.Load(inputPath);
            // Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            var nodes = xml.SelectNodes("//*")?
              .Cast<XmlNode>()
              .Where(node => !IsPath(node))
              .ToList();
            if (nodes == null) return;

            var newDic = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            //Dictionary wird gefilled von Terminologie Datenbank!
            IEnumerable<Task> translationTasks = nodes.Select(async node =>
            {
                node.InnerText = newDic.TryGetValue(node.InnerText, out string? value) && value != null ? value : await TranslateText(node.InnerText);
            });
            //Falls nicht in Terminologie, evtl in extra Dictionary speichern? "Ressources" oder in NewDic für andere API Calls
            await Task.WhenAll(translationTasks);
            xml.Save(Path.Combine(outputPath, Path.GetFileName(inputPath))); //EVTL NOCH ENCODING!
        }

        private static bool IsPath(XmlNode node)
        {
            return node.InnerText.Contains("/") || node.InnerText.Contains("\\");
        }
    }
}

