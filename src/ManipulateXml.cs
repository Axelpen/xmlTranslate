using System.Xml;
using System.Xml.Linq;
namespace XmlTranslate.src

{
    internal class ManipulateXml
    {
        public static XmlDocument RestoreNodes(XmlDocument inputXml)
        {
            var nodesToChange = inputXml.SelectNodes("//*")?
              .Cast<XmlNode>()
              .Where(ContainsKeyWord);
            foreach (var node in nodesToChange)
            {
            }
            return inputXml;
        }

        private static bool ContainsKeyWord(XmlNode node)
        {
            return node.Name.Contains("skibidi");
        }

        public static void RenameNodes(XElement node, bool alreadyTransformed)
        {
            // AlreadyTransformed = true, remove das das Keyword, else adde das Keyword

        }
    }
}
