using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class RandomFilesSplitter : FilesSplitter
{
    private Dictionary<int, List<string>> originalSentences;
    private SortedSet<int> usedSentences;
    private Random randomGenerator;
    private int until;
    public RandomFilesSplitter(string originalFile, int howManyParts, bool generateComplement, string complementPrefix)
        : base(originalFile, howManyParts, generateComplement, complementPrefix)
    {
    }

    public override void Execute()
    {
        randomGenerator = new Random(DateTime.Now.Millisecond);

        var originalLines = ExtractSentences();
        until = originalSentences.Count;
        usedSentences = new SortedSet<int>();

        var partSize = originalLines.Length/howManyParts;
        var part = 1;

        while (part <= howManyParts)
        {
            Split(partSize, part);
            part++;
        }
    }

    private string[] ExtractSentences()
    {
        var originalLines = File.ReadAllLines(originalFile);
        originalSentences = new Dictionary<int, List<string>>();

        var sentenceCount = 0;
        var index = 0;
        while (index < originalLines.Length)
        {
            var sentence = new List<string>();
            while (index < originalLines.Length && !string.IsNullOrWhiteSpace(originalLines[index]))
            {
                sentence.Add(originalLines[index]);
                index++;
            }
            originalSentences[sentenceCount] = sentence;
            sentenceCount++;
            index++;
        }
        return originalLines;
    }

    private void Split(int partSize, int part)
    {
        var name = Path.GetFileNameWithoutExtension(originalFile);
        var extension = Path.GetExtension(originalFile);
        var fileName = Path.GetFileName(originalFile);
        var partName = originalFile.Replace(fileName, name + part + extension);

        var selectedSentences = new SortedSet<int>();

        CreatePart(partSize, selectedSentences, partName);

        if (generateComplement)
            CreateComplement(part, fileName, name, extension, selectedSentences);
    }

    private void CreateComplement(int part, string fileName, string name, string extension,
        ICollection<int> selectedSentences)
    {
        var complementName = originalFile.Replace(fileName, name + complementPrefix + part + extension);
        var partComplement = new StreamWriter(complementName, false, Encoding.Default);

        for (var i = 0; i < originalSentences.Count; i++)
            if (!selectedSentences.Contains(i))
            {
                var sentence = originalSentences[i];

                foreach (var word in sentence)
                    partComplement.WriteLine(word);

                partComplement.WriteLine();
            }
        partComplement.Close();
    }

    private void CreatePart(int partSize, ICollection<int> selectedSentences, string partName)
    {
        var partFile = new StreamWriter(partName, false, Encoding.Default);
        var size = 0;

        while (size < partSize && usedSentences.Count < originalSentences.Count)
        {
            var sentenceIndex = SelectSentenceIndex();
            usedSentences.Add(sentenceIndex);
            selectedSentences.Add(sentenceIndex);
            var sentence = originalSentences[sentenceIndex];

            foreach (var word in sentence)
                partFile.WriteLine(word);

            partFile.WriteLine();
            size += sentence.Count;
        }
        partFile.Close();
    }

    private int SelectSentenceIndex()
    {                
        int i;

        do i = randomGenerator.Next(0, until); while (usedSentences.Contains(i));

        return i;
    }
}
