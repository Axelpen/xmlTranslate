using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using Cobus.Ncad.AdvancedDialogs;
//XDocument scheint besser zu funktionieren, hat auch eine 
//Methode um Node.Name zu renamen. Das macht das ganze sehr 
//viel einfacher als der Hacky Weg mit XmlDocument. 

namespace XmlTranslate.src
{
    internal class XmlTranslate
    {
        //In C-Sharp geht "~" nicht für home Directory auf Linux, unten ist ein Workaround..
        static string userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        private static List<string> usedTags = new();
        private static List<Tuple<string, string>> oldNewTrans = new();
        public static readonly string keyWord = "xztzt";


        private static readonly string xPathExpression = @"/*/*[not(name() = 'DateiName' or name() = 'DateiDatum' or name() = 'BitmapPfad' or name() = 'AltBitmapPfad' or name() = 'MenueBitmap')]";


        public static async Task<int> Main(string[] args)
        {
            string inputPathh = Path.Combine(userHome, "Documents", "testtt.xml");
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("input path please");
                // return 1;
            }
            await ProcessPath(inputPathh);
            ///  WriteCsv(inputPathh, oldNewTrans);
            return 0;
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


            foreach (XElement? node in nodes)
            {
                if (newDic.TryGetValue(node.Value, out string? value))
                {

                    node.Name = ManipulateXml.RenameNodes(node, false);
                    usedTags.Add(node.Name.ToString());
                }
            }
            _ = await TranslateXml.TranslateDocument(inputPath, outputPath, usedTags);
            //node.Name = Tag, InnerText = Text
            //xml.Save(Path.Combine(userHome, "Documents", "test.xml")); TranslateDocument speichert die Xml schon!
        }

        private static void WriteCsv(string filePath, List<Tuple<string, string>> translations)
        {
            string csvPath = Path.Combine(filePath, "output.csv");
            /*string csvPath = "C:\\Users\\alexander.penck.INTERN\\Desktop\\testHTML";*/
            if (File.Exists(csvPath))
            {
                File.Delete(csvPath);
            }
            using var sw = new StreamWriter(csvPath, false, Encoding.GetEncoding("iso-8859-1"));
            foreach (Tuple<string, string> trans in translations)
            {
                sw.WriteLine(string.Join(";", trans));
            }
            return;
            //TODO for future use
        }

        private static bool IsPath(XElement node)
        {
            return node.Value.Contains("/") || node.Value.Contains("\\");
        }
        private static string SearchTerminology(string nodeText)
        {
            ccNcadAdvancedDialogInfo dialogInfo = new ccNcadAdvancedDialogInfo();
            dialogInfo = new ccNcadAdvancedDialogInfo(nodeText, 0, true);
            return dialogInfo.ToString();
        }
    }
}

