using System.Collections.Generic;
using System.Linq;

public class Cell : IComparer<Cell>
{
    public string TryTag { get; private set; }
    public string GoldStandardTag {get; private set;}
    private Dictionary<string, int> words;
    public Cell(string goldStandardTag, string tryTag, string palabra)
    {
        TryTag = tryTag;
        GoldStandardTag = goldStandardTag;
        words = new Dictionary<string, int> {{palabra, 1}};
    }

    public bool Equals(Cell otherCell)
    {
        if (ReferenceEquals(null, otherCell))
            return false;
        if (ReferenceEquals(this, otherCell))
            return true;
        return Equals(otherCell.TryTag, TryTag) && Equals(otherCell.GoldStandardTag, GoldStandardTag);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != typeof (Cell))
            return false;
        return Equals((Cell) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((TryTag != null ? TryTag.GetHashCode() : 0)*397) ^ (GoldStandardTag != null ? GoldStandardTag.GetHashCode() : 0);
        }
    }

    public override string ToString()
    {
        return string.Format("{0}(gold)-{1}(prueba): {2}", GoldStandardTag, TryTag, TotalWords);
    }

    public int Compare(Cell x, Cell y)
    {
        return 1;
    }
 
    public IEnumerable<KeyValuePair<string, int>> GetWordsWithDifference(int howMany)
    {
        return words.OrderByDescending(s => s.Value).Take(howMany);
    }
    
    public int TotalWords
    {
        get
        {
            return words.Sum(p => p.Value);
        }
    }
    
    public void AddWord(string word)
    {
        if (words.AddIfDoesntExists(word, 1))
            words[word]++;
    }
}