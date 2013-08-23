using System;
using System.Collections.Generic;
using System.IO;

public class Configuration
{
    public Configuration(string title, string rowTitle, string ColumnTitle, ITranslation translation, string specificCellsFile, int matrixOutputSize)
    {
        Title = title;
        RowTitle = rowTitle;
        this.ColumnTitle = ColumnTitle;
        Translation = translation;
        MatrixOutputSize = matrixOutputSize;
        GetSpecificRowsAndColumns(specificCellsFile);
    }
        
    public string Title { get; private set; }
    public string RowTitle { get; private set; }
    public string ColumnTitle { get; private set; }
    public ITranslation Translation { get; private set; }
    public List<string> RowTags{ get; private set; }
    public List<string> ColumnTags{ get; private set; }
    public int MatrixOutputSize{ get; private set; }
    public bool UseSpecificTags { get; private set; }
        
    private void GetSpecificRowsAndColumns(string specificCellsFile)
    {
        if (!string.IsNullOrWhiteSpace(specificCellsFile))
        {
            var specificRowsAndColumns = File.ReadAllText(specificCellsFile).Split('\n');
            ColumnTags = new List<string>();
            RowTags = new List<string>();

            foreach (var specificColumn in specificRowsAndColumns[0].Split())
                if (!string.IsNullOrWhiteSpace(specificColumn))
                    ColumnTags.Add(specificColumn);

            for (var i = 1; i < specificRowsAndColumns.Length; i++)
            {
                var specificRow = specificRowsAndColumns[i].Split()[0];
                if (!string.IsNullOrWhiteSpace(specificRow))
                    RowTags.Add(specificRow);
            }
                    
            UseSpecificTags = true;
        }
    }

    public bool UseTranslation
    {
        get
        {
            return Translation != null && Translation.GetType() != typeof(EmptyTranslation);
        }
    }

    public string GetTagFor(string rowTag)
    {
        var tagResult = rowTag;
        if (UseTranslation)
            tagResult = Translation.GetInverseTranslationFor(rowTag);
        return tagResult;
    }

    public Tuple<IEnumerable<string>, IEnumerable<string>> GetTags(ConfusionMatrix confusionMatrix)
    {
        return UseSpecificTags ? 
            new Tuple<IEnumerable<string>, IEnumerable<string>>(RowTags, ColumnTags) : 
            confusionMatrix.OrderByError().GetTags(MatrixOutputSize);    
    }
}