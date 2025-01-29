using System.Xml;
using DeepL;

namespace XmlTranslate.src
{
    internal class XmlTranslate
    {
        private static List<Tuple<string, string>> oldNewTrans = new();

        public static async Task<int> Main(string[] args)
        {
            string inputPathh = @"C:\Users\alexander.penck.INTERN\Desktop\testHTML\superTest";
            Console.WriteLine("ayay");
            Console.WriteLine("Captain ayay");
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("input path please");
                // return 1;
            }
            /*Console.WriteLine(args[0]);*/
            await ProcessPath(inputPathh);
            WriteCsv(inputPathh, oldNewTrans);
            return 0;
        }

        private static async Task<string> TranslateText(string nodeText)
        {
            var authKey = "";
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

        private static async Task ProcessPath(string inputPath)
        {
            string outputPath = Path.Combine(inputPath, "output");
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            if (File.Exists(inputPath))
            {
                await TransformXML(inputPath, outputPath);
            }
            if (Directory.Exists(inputPath))
            {
                string[] xmlFiles = Directory.GetFiles(inputPath, "*.xml", SearchOption.TopDirectoryOnly);
                foreach (string xmlFile in xmlFiles)
                {
                    await TransformXML(xmlFile, outputPath);
                }
            }
        }

        private static async Task TransformXML(string inputPath, string outputPath)
        {
            XmlDocument xml = new();
            xml.Load(inputPath);
            // Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            var nodes = xml.SelectNodes("//*")?
              .Cast<XmlNode>()
              .Where(node => !IsPath(node));
            if (nodes == null) return;

            var newDic = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            //Dictionary wird gefilled von Terminologie Datenbank!
            IEnumerable<Task> translationTasks = nodes.Select(async node =>
            {
                if (!string.IsNullOrEmpty(node.InnerText.ToString()))
                {
                    string placeHolder = node.InnerText.ToString();
                    node.InnerText = newDic.TryGetValue(node.InnerText, out string? value) && value != null ? value : await TranslateText(node.InnerText);
                    oldNewTrans.Add(Tuple.Create(placeHolder, node.InnerText));
                    Console.WriteLine($"{node.InnerText} +++++ {placeHolder}");
                }
            });
            //Falls nicht in Terminologie, evtl in extra Dictionary speichern?
            // "Ressources" oder in NewDic für weitere API Calls
            await Task.WhenAll(translationTasks);
            xml.Save(Path.Combine(outputPath, "test.xml"));
        }

        private static void WriteCsv(string filePath, List<Tuple<string, string>> translations)
        {
            string csvPath = Path.Combine(filePath, "output.csv");
            /*string csvPath = "C:\\Users\\alexander.penck.INTERN\\Desktop\\testHTML";*/
            if (File.Exists(csvPath))
            {
                File.Delete(csvPath);
            }
            using var sw = new StreamWriter(csvPath); //dumb ah ah streamwriter doesnt accept csvpath,false,encoding as parameters?
            foreach (Tuple<string, string> trans in translations)
            {
                sw.WriteLine(String.Join(";", trans));
            }
            return;
            //TODO for future use
        }

        private static bool IsPath(XmlNode node)
        {
            return false;
            //return node.InnerText.Contains("/") || node.InnerText.Contains("\\");
        }
    }
}

