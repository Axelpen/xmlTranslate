using System.Xml.Linq;
using System.Xml.XPath;

namespace XmlTranslate.src

{
    internal class ManipulateXml
    {
        public static XDocument RestoreNodes(XDocument inputXml)
        {
            var nodesToChange = inputXml.XPathSelectElements("//*")?
              .Where(node => ContainsKeyWord(node));

            foreach (var node in nodesToChange)
            {
                node.Name = node.Name.ToString()[..^7]; //Removes last 7 Chars, aka "Skibidi":P
            }
            return inputXml;
        }

        private static bool ContainsKeyWord(XElement node) => node.Name.LocalName.Contains("skibidi");

        public static void RenameNodes(XElement node, bool alreadyTransformed)
        {
            // AlreadyTransformed = true, remove das das Keyword, else adde das Keyword

        }
    }
}
