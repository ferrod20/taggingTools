using System;
using System.IO;
using System.Linq;
using System.Text;

public class FilesJoiner : FilesManager
{
    private string generatedFile;
    private string finalFile;

    public FilesJoiner(string originalFile, string generatedFile, string finalFile):base(originalFile)
    {
        this.generatedFile = generatedFile;
        this.finalFile = finalFile;
    }

    private void Execute(Action<string, string, string[]> action)
    {
        var originalText = new StreamReader(originalFile);
        var generatedText = new StreamReader(generatedFile);

        output = new StreamWriter(finalFile, false, Encoding.Default);

        var extractedLine = originalText.ReadLine();
        var generatedTag = generatedText.ReadLine();

        while (extractedLine != null && generatedTag != null)
        {
            if (extractedLine != string.Empty)
            {
                var extractedParts = extractedLine.Split();
                output.Write(extractedParts[0]);
                var extractedTag = extractedParts.Last();
                action(extractedTag, generatedTag, extractedParts);
            }

            output.WriteLine();

            extractedLine = originalText.ReadLine();
            generatedTag = generatedText.ReadLine();
        }

        output.Close();
        originalText.Close();
        generatedText.Close();
    }

    private void Join(string originalTag, string generatedTag, string[] extractedParts)
    {
        output.Write("\t");

        if (extractedParts.Count() > 1 && !string.IsNullOrEmpty(originalTag))
            output.Write(originalTag);
        else
            output.Write(generatedTag.Split().Last());
    }


    public void Execute()
    {
        Execute(Join);
    }
}