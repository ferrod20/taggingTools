using System.Collections.Generic;
using System.IO;

public class TranslationWriter: MatrixWriter
{
    public TranslationWriter(string outputFile, ConfusionMatrix confusionMatrix) : base(outputFile, confusionMatrix)
    {
    }

    public void Write()
    {
        var included = new List<string>();
        TextWriter output = new StreamWriter(outputFile);

        confusionMatrix.OrderByError();

        foreach (var cell in confusionMatrix)
            if (!included.Contains(cell.GoldStandardTag))
            {
                output.WriteLine(cell.GoldStandardTag + " " + cell.TryTag + " " + cell.TotalWordsOcurrencies);
                included.Add(cell.GoldStandardTag);
            }

        output.Close();
    }
}        