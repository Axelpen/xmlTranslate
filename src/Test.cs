//oh god extracting every text from every node doesnt work,
//the deepL APi is too slow and limits requests after
//a while, especially when running them all at the same time.
//Next idea would be to just let DeepL API translate the whole
//document, change the Tags of the already translated paragraphs
//(using terminology) to something like "xexampletag", then 
//use the IgnoreTags Option from DeepL, 
//at the end removing the extra character from the tag so MSXSL works....
//god this sucks

using System.Xml;
using DeepL;

namespace XmlTranslate.src
{
    internal class TestTranslate
    {
        public static async Task<int> Main(string[] args)
        {
            return 0;
        }

        public static async Task transformXml(string inputPath)
        {
            XmlDocument xml = new();
            xml.Load(inputPath);
            string authKey = " "; //TBC
            var translator = new Translator(authKey);
            
        }

        public static bool isPath(string inputText)
        { //Paths sollten nicht translated werden
            return inputText.Contains("/") || inputText.Contains("\\");
        }
    }
}
