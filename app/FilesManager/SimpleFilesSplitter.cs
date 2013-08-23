using System;
using System.IO;
using System.Text;


public class SimpleFilesSplitter : FilesSplitter 
{
    private string[] originalLines;

    public SimpleFilesSplitter(string originalFile, int howManyParts, bool generateComplement, string complementPrefix)
        : base(originalFile, howManyParts, generateComplement, complementPrefix)
    {
    }

    public override void Execute()
    {
        var part = 1;
        originalLines = File.ReadAllLines(originalFile);
        var partSize = originalLines.Length/howManyParts;

        var from = 0;
        var until = partSize;

        while (part <= howManyParts)
        {
            while (until < originalLines.Length && !string.IsNullOrWhiteSpace(originalLines[until]) )
                until++;
            
            Split(from, until, part);
            part++;
            from = until + 1;
            until = Math.Min(until + partSize, originalLines.Length - 1);
        }
    }

    private void Split(int from, int until, int part)
    {
        var name = Path.GetFileNameWithoutExtension(originalFile);
        var extension = Path.GetExtension(originalFile);
        var fileName = Path.GetFileName(originalFile);
        var partName = originalFile.Replace(fileName, name + part + extension);

        var partFile = new StreamWriter( partName, false, Encoding.Default);
        
        var index = 0;

        StreamWriter partComplement = null;
        
        if (generateComplement)
        {
            var complementName = originalFile.Replace(fileName, name + complementPrefix + part + extension);
            partComplement = new StreamWriter(complementName, false, Encoding.Default);
            while (index < from)
            {
                partComplement.WriteLine(originalLines[index]);
                index++;
            }    
        }
        else
            index = from;

        while (index <= until && index < originalLines.Length)
        {
            partFile.WriteLine(originalLines[index]);
            index++;
        }

        if (generateComplement)
            while (index < originalLines.Length)
            {
                partComplement.WriteLine(originalLines[index]);
                index++;
            }

        partFile.Close();

        if (generateComplement)
            partComplement.Close();
    }
}