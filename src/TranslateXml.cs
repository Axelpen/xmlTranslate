//Translate Documents 
using DeepL;
using System.Text;

namespace XmlTranslate.src

{
    internal class TranslateXml
    {
        public static async Task<int> TranslateDocument(string inputPath, string outputPath, List<string> tbIgnored)
        {
            var authKey = "";
            var translator = new Translator(authKey);
            var options = new TextTranslateOptions
            {
                TagHandling = "xml"
            };
            options.IgnoreTags.AddRange(tbIgnored);

            try
            {
                string xmlContent = File.ReadAllText(inputPath);
                var result = await translator.TranslateTextAsync(
                    xmlContent,
                    LanguageCode.German,
                    LanguageCode.Polish,
                    options
                    );
                File.WriteAllText(outputPath, result.Text, Encoding.UTF8);
                Console.WriteLine(outputPath);
                return 0;
            }
            catch (DocumentTranslationException e)
            {
                Console.WriteLine($"SADGE {e}");
            }
            return 1;
            //gotta figure this out. 
        }
    }
}
