using System;
using System.IO;

public class MatrixWriter
{
    protected string outputFile;
    protected ConfusionMatrix confusionMatrix;


    public MatrixWriter(string outputFile, ConfusionMatrix confusionMatrix)
    {
        this.outputFile = outputFile;
        this.confusionMatrix = confusionMatrix;
    }
}

public abstract class ConfusionMatrixWriter:MatrixWriter
{
    protected Configuration conf;
    protected ConfusionMatrixWriter(string outputFile, ConfusionMatrix confusionMatrix, Configuration configuration)
        : base(outputFile, confusionMatrix)
    {
        conf = configuration;
    }

    public abstract void WriteMatrix();

    protected void WriteHeader(TextWriter output)
    {
        var differentQuantity = confusionMatrix.DifferentQuantity;
        var equalQuantity = confusionMatrix.EqualQuantity;
        var equalPercentage = confusionMatrix.EqualPercentage;
        output.WriteLine("%Tagging Tools, {0:ddd MMM d HH:mm:ss yyyy}", DateTime.Now);
        output.WriteLine("%Equal\t\t: {0} ({1:0.00}%)", equalQuantity, equalPercentage);
        output.WriteLine("%Different\t: {0}", differentQuantity);
        output.WriteLine("%Fernando Rodriguez, ferrod20@gmail.com");
    }
}