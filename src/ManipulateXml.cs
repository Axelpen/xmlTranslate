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
                if (node == null) continue;
                node.Name = RenameNodes(node, true);
            }
            return inputXml;
        }

        private static bool ContainsKeyWord(XElement node)
        {
            return node.Name.LocalName.Contains(XmlTranslate.keyWord);
        }

        public static string RenameNodes(XElement node, bool alreadyTransformed)
        {
            string newName = alreadyTransformed
              ? node.Name.ToString()[..^XmlTranslate.keyWord.Length]
              : node.Name.ToString() + XmlTranslate.keyWord;

            return newName;
        }
    }
}
