using System.Xml.Linq;
using System.Xml.XPath;
using DeepL;
//XDocument scheint besser zu funktionieren, hat auch eine 
//Methode um Node.Name zu renamen. Das macht das ganze sehr 
//viel einfacher als der Hacky Weg mit XmlDocument. 

namespace XmlTranslate.src
{
    internal class XmlTranslate
    {
        //In C-Sharp geht "~" nicht für home Directory auf Linux, unten ist ein Workaround..
        static string userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        private static List<Tuple<string, string>> oldNewTrans = new();
        private static string xPathExpression = @"/*/*[not(name() = 'DateiName' or name() = 'DateiDatum' or name() = 'BitmapPfad' or name() = 'AltBitmapPfad' or name() = 'MenueBitmap')]";
        public static async Task<int> Main(string[] args)
        {
            string inputPathh = Path.Combine(userHome, "Documents", "testtt.xml");
            Console.WriteLine("ayay");
            Console.WriteLine("Captain ayay");
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("input path please");
                // return 1;
            }
            /*Console.WriteLine(args[0]);*/
            await ProcessPath(inputPathh);
            ///  WriteCsv(inputPathh, oldNewTrans);
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
            /*if (!Directory.Exists(outputPath))*/
            /*{*/
            /*    Directory.CreateDirectory(outputPath);*/
            /*}*/

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
            XDocument xml = XDocument.Load(inputPath);
            // Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            var nodes = xml.XPathSelectElements(xPathExpression)
                       .Where(node => !IsPath(node));

            if (nodes == null) return;

            var newDic = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            //Dictionary wird gefilled von Terminologie Datenbank!


            foreach (var node in nodes)
            {
                Console.WriteLine($"{node.Name} {node.Value}");
            }

            //node.Name = Tag, InnerText = Text
            xml.Save(Path.Combine(userHome, "Documents", "test.xml"));
        }

        private static void WriteCsv(string filePath, List<Tuple<string, string>> translations)
        {
            string csvPath = Path.Combine(filePath, "output.csv");
            /*string csvPath = "C:\\Users\\alexander.penck.INTERN\\Desktop\\testHTML";*/
            if (File.Exists(csvPath))
            {
                File.Delete(csvPath);
            }
            bool ja = true;
            using var sw = new StreamWriter(csvPath);
            //dumb ah ah streamwriter doesnt accept csvpath,false,encoding as parameters?
            foreach (Tuple<string, string> trans in translations)
            {
                sw.WriteLine(String.Join(";", trans));
            }
            return;
            //TODO for future use
        }

        private static bool IsPath(XElement node)
        {
            return node.Value.Contains("/") || node.Value.Contains("\\");
        }
    }
}

