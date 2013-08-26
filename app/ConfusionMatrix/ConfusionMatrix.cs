using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConfusionMatrix : Matrix, IEnumerable<Cell>
{
    public ConfusionMatrix(List<Cell> cells, int tagsQuantity, int differentQuantity)
    {
        this.cells = cells;
        TagsQuantity = tagsQuantity;            
        DifferentQuantity = differentQuantity;
        EqualQuantity = tagsQuantity - differentQuantity;
    }

    public int EqualQuantity { get; private set; }
    public int DifferentQuantity { get; private set; }
    public int TagsQuantity{get; private set; }

    public double EqualPercentage
    {
        get { return (double)EqualQuantity / (double)TagsQuantity * 100; }            
    }

    public int GetDifferences(string tryTag, string goldStandardTag)
    {
        foreach (var çell in cells)
            if (çell.TryTag == tryTag && çell.GoldStandardTag == goldStandardTag)
                return çell.TotalWordsOcurrencies;

        return 0;
    }

    public List<int> GetDifferences(int howMany, List<string> rowTags, List<string> columnTags)        
    {
        var errors = new List<int>();
        if (rowTags != null && columnTags != null)
        {
            foreach (var cell in cells)
                if (columnTags.Contains(cell.TryTag) && rowTags.Contains(cell.GoldStandardTag))
                {
                    errors.Add(cell.TotalWordsOcurrencies);
                    if (errors.Count == howMany)
                        break;
                }
        }
        else
            errors = cells.Select(tags => tags.TotalWordsOcurrencies).Take(howMany).ToList();

        return errors;
    }

    public List<Cell> GetCells(List<string> rowTags, List<string> columnTags)
    {
        var cellsToReturn = new List<Cell>();
        var specificCellsDefined = rowTags != null && columnTags != null;
        foreach (var cell in cells)
            if (!specificCellsDefined || (columnTags.Contains(cell.TryTag) && rowTags.Contains(cell.GoldStandardTag)))
                cellsToReturn.Add(cell);                    

        return cellsToReturn;
    }

    public Tuple<IEnumerable<string>, IEnumerable<string>> GetTags(int howMany)
    {
        var rowTags = new List<string>();
        var columnTags = new List<string>();
        foreach (var cell in cells)
        {
            if (!columnTags.Contains(cell.TryTag))
                columnTags.Add(cell.TryTag);
            if (!rowTags.Contains(cell.GoldStandardTag))
                rowTags.Add(cell.GoldStandardTag);
        }

        return new Tuple<IEnumerable<string>, IEnumerable<string>>(rowTags.Take(howMany), columnTags.Take(howMany));
    }

    public ConfusionMatrix OrderByError()
    {
        cells = cells.OrderByDescending(s => s.TotalWordsOcurrencies).ToList();
        return this;
    }
        
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<Cell> GetEnumerator()
    {
        return cells.GetEnumerator();
    }
        
    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var tag in cells)
            sb.AppendLine(tag.ToString());
        return sb.ToString();
    }
}