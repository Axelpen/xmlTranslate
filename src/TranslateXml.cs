//Translate Documents 
using DeepL;

namespace XmlTranslate.src

{
    internal class TranslateXml
    {
        public static async Task<string> TranslateDocument(string inputPath, List<string> tbIgnored)
        {
            var authKey = "";
            var translator = new Translator(authKey);
            var options = new DocumentTranslateOptions{
              TagHandling = "xml"};
            options.IgnoreTags.AddRange(tbIgnored);

            try
            {
              await translator.TranslateDocumentAsync(
                  new FileInfo(inputPath),
                  new FileInfo(Path.Combine(inputPath, "testo.xml")),
                    "DE",
                    "POL",
                    options
                  );
            }
            catch(DocumentTranslationException e)
            {
              Console.WriteLine($"SADGE {e}");
            }
            //gotta figure this out. 
        }
    }
}
