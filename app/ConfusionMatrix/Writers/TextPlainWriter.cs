using System.IO;

public class TextPlainWriter : ConfusionMatrixWriter
{
    public TextPlainWriter(string outputFile, ConfusionMatrix confusionMatrix, Configuration conf)
        : base(outputFile, confusionMatrix, conf)
    {
    }

    public override void WriteMatrix()
    {
        TextWriter output = new StreamWriter(outputFile);
        WriteHeader(output);

        output.WriteLine();
        output.WriteLine(conf.Title);
        output.WriteLine();
        output.WriteLine("{0} | {1} | differences", conf.RowTitle, conf.ColumnTitle);
        output.WriteLine();

        var cells = confusionMatrix.GetCells(conf.RowTags, conf.ColumnTags);

        foreach (var cell in cells)
        {
            output.WriteLine("{0} {1} {2}", cell.GoldStandardTag, cell.TryTag , cell.TotalWords);
            foreach (var word in cell.GetWordsWithDifference(conf.MatrixOutputSize))
                output.WriteLine("\t{0} {1}", word.Key, word.Value);
        }

        output.Close();
    }
}