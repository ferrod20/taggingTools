using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Translation: ITranslation
{
    private static Dictionary<string, List<string>> translationDic;
    private const string separator = "->";

    public Translation(string translationFile)
    {
        translationDic = new Dictionary<string, List<string>>();

        var translationText = new StreamReader(translationFile);

        var extractedLine = translationText.ReadLine();        
        while (extractedLine != null)
        {
            if (extractedLine.Contains(separator))
            {
                var parts = extractedLine.Split(new[] { separator }, StringSplitOptions.None);
                if (parts.Length > 1)
                {
                    var key = parts[0].Trim();    
                    var values = parts[1].Split('|').Select(v=>v.Trim());                    

                    if(translationDic.ContainsKey(key))
                        translationDic[key].AddRange(values);
                    else
                        translationDic[key] = new List<string>(values);
                }
            }
            extractedLine = translationText.ReadLine();            
        }
    }

    public List<string> GetTranslationFor(string tag)
    {
        return translationDic[tag];
    }

    public bool ContainsTranslationFor(string tag)
    {
        return translationDic.ContainsKey(tag);
    }

    public string GetInverseTranslationFor(string tag)
    {
        var result = tag;
        var translation = translationDic.FirstOrDefault(trad => trad.Value.Contains(tag));

        if( !string.IsNullOrWhiteSpace(translation.Key))
            result = translation.Key;
            
        return result;
    }

    public IEnumerable<string> GetTranslationsForTagsStartingWith(string possibleTag)
    {
        return translationDic.Where(t => t.Key.StartsWith(possibleTag)).SelectMany(translation => translation.Value);
    }
}