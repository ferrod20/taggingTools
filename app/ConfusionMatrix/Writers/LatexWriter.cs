using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class LatexWriter : ConfusionMatrixWriter
{
    public LatexWriter(string outputFile, ConfusionMatrix confusionMatrix, Configuration conf)
        : base(outputFile, confusionMatrix, conf)
    {
    }

    private static void WriteHeader(string title, string rowTitle, string columnTitle, IEnumerable<string> columnTags, TextWriter output)
    {
        output.Write(
            @"\begin{center}
\begin{table}[H]
\caption{" + title + @"}
\begin{tabular}{| l | ");
        for (var i = 0; i < columnTags.Count(); i++)
            output.Write("c | ");

        output.Write(
            @"}
\hline
\backslashbox{\scriptsize{" + rowTitle + @"}\kern-1em}{\kern-1em \scriptsize{" +
            columnTitle + @"}}  &	");

        var rowTags = new List<string>(columnTags);
        var rowTagsCount = rowTags.Count();
        for (var i = 0; i < rowTagsCount; i++)
        {
            var tag = rowTags[i];
            output.Write("\\textbf{" + tag );

            if (i == rowTagsCount - 1)
                output.WriteLine("}\\\\  ");
            else
                output.Write("}	&   ");
        }
        output.WriteLine(@"\hline");
    }

    public override void WriteMatrix()
    {
        TextWriter output = new StreamWriter(outputFile);
            
        var tags = conf.GetTags(confusionMatrix);  
        var rowTags = tags.Item1;
        var columnTags = tags.Item2;
        var errors = confusionMatrix.GetDifferences(conf.MatrixOutputSize, conf.RowTags, conf.ColumnTags);

        WriteHeader(output);
        WriteHeader(conf.Title, conf.RowTitle, conf.ColumnTitle, columnTags, output);

        var totalDifference = 0;
        foreach (var rowTag in rowTags)
        {
            var rowTagToShow = conf.GetTagFor(rowTag);

            output.Write(@"\textbf{" + rowTagToShow + "}");
            foreach (var columnTag in columnTags)
            {
                output.Write(" & ");
                var difference = confusionMatrix.GetDifferences(columnTag, rowTag);//tag columna es tag de prueba, tagFila es tag gold standard
                totalDifference += difference;
                if (difference == 0)
                    output.Write("-");
                else
                {
                    if (errors.Contains(difference))
                        output.Write("\\textbf{" + difference + "}");//bold
                    else
                        output.Write(difference);
                }

            }
            output.WriteLine(@"\\");
        }


        output.Write(@"\hline
\end{tabular}
\end{table}
\end{center}");

        output.WriteLine();
        output.Close();
    }   
}