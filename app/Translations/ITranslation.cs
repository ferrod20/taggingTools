using System.Collections.Generic;

public interface ITranslation
{
    List<string> GetTranslationFor(string tag2);
    bool ContainsTranslationFor(string tag2);
    string GetInverseTranslationFor(string tag);
    IEnumerable<string> GetTranslationsForTagsStartingWith(string possibleTag);
}

public class EmptyTranslation: ITranslation
{
    public List<string> GetTranslationFor(string tag)
    {
        return new List<string>{tag};
    }

    public bool ContainsTranslationFor(string tag)
    {
        return true;
    }

    public string GetInverseTranslationFor(string tag)
    {
        return tag;
    }

    public IEnumerable<string> GetTranslationsForTagsStartingWith(string possibleTag)
    {
        return GetTranslationFor(possibleTag);
    }
}