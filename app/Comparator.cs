using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Comparator
{
    private MatrixInProcess matrixInProcess;
    private Configuration configuration;
    private string goldStandardFile, fileToCompare, confusionMatrixOutput;
    private bool outputForLatex;


    public Comparator(string goldStandardFile, string fileToCompare, string confusionMatrixOutput)
    {
        this.goldStandardFile = goldStandardFile;
        this.fileToCompare = fileToCompare;
        this.confusionMatrixOutput = confusionMatrixOutput;
    }
    public void SetOptions(bool outputForLatex, string title, string rowTitle, string columnTitle, ITranslation translation, string specificCellsFile, int size)
    {
        this.outputForLatex = outputForLatex;
        configuration = new Configuration(title, rowTitle, columnTitle, translation, specificCellsFile, size);
    }
        
    private void FindNextLine(string[] one, string[] other, ref int i, ref int j)
    {
        var iii = 0;
        var dist = 100;
        for (var ii = 0; ii < 5 && i + ii + 1 < one.Length; ii++)
        {
            var distance = CalculateDistance(one[i + ii].Split('\t')[0], one[i + ii + 1].Split('\t')[0], other, j);

            if (distance + ii < dist + iii)
            {
                iii = ii;
                dist = distance;
            }
        }
        i = i + iii;
        j = j + dist;
    }
        
    private int CalculateDistance(string word, string nextWord, string[] lines, int lineIndex)
    {
        var distance = 0;
        while (lineIndex + 2 < lines.Length && lines[lineIndex].Split('\t')[0] != word && lines[lineIndex + 1].Split('\t')[0] != nextWord && distance < 16)
        {
            lineIndex++;
            distance++;
        }
        return distance;
    }

    private bool CompareTags(string tag1, string tag2)
    {
        var result = false;
                
        if (configuration.UseTranslation)
        {
            var translation = configuration.Translation;
            if (translation.ContainsTranslationFor(tag2))
            {
                var tags = tag1.Split('-');
                var translatedTags = translation.GetTranslationFor(tag2);
                result = translatedTags.Intersect(tags).Any();
            }
        }
        else
            result = tag1 == tag2;
                
        return result;
    }

    private void Compare(string word, string goldStandardLine, string generatedLine)
    {
        var generatedParts = generatedLine.TrimEnd().Split();
        var generatedTag = generatedParts.Length > 1 ? generatedParts.LastOrDefault() : "";

        var goldStandardParts = goldStandardLine.TrimEnd().Split();
        var goldStandardTag = goldStandardParts.Length > 1 ? goldStandardParts.LastOrDefault() : "";

        if (!string.IsNullOrEmpty(generatedTag) && !string.IsNullOrEmpty(goldStandardTag))
        {
            matrixInProcess.AddTag();
            var success = CompareTags(goldStandardTag, generatedTag);
                
            if (!success)
                matrixInProcess.AddError(goldStandardTag, generatedTag, word);
        }
    }
        
    /// <summary>
    ///   Lee los archivos y va generando la Matrix de confusión.
    /// </summary>
    private ConfusionMatrix BuildConfusionMatrix(string goldStandardFile, string fileToCompare)
    {
        matrixInProcess = new MatrixInProcess();
        int i = 0, j = 0, point = 1;
        var goldStandard = File.ReadAllText(goldStandardFile).Split(new [] { Environment.NewLine }, StringSplitOptions.None);
        var toCompare = File.ReadAllText(fileToCompare).Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        var goldStandardSize = goldStandard.Length;
        var sizeToCompare = toCompare.Length;
        var part = goldStandardSize/20;

        while (i < goldStandardSize && j < toCompare.Length)
        {
            var goldStandardWord = goldStandard[i].Split('\t')[0].TrimEnd();
            var toCompareWord = toCompare[j].Split('\t')[0].TrimEnd();
            if ( goldStandardWord == toCompareWord )                                    
                Compare(goldStandardWord, goldStandard[i], toCompare[j]);                    
                                    
            FindNewPositions(goldStandard, toCompare, goldStandardSize, sizeToCompare, ref i, ref j);

            if (i > part*point)
            {
                Console.Write(".");
                point++;
            }
        }

        return matrixInProcess.FinalizeProcces();
    }

    private void FindNewPositions(string[] goldStandard, string[] toCompare, int goldStandardSize, int toCompareSize, ref int i, ref int j)
    {
        int ii, iii, jj, jjj;
        i++;
        j++;
        if (i < goldStandardSize && j < toCompareSize && goldStandard[i].Split('\t')[0].TrimEnd() != toCompare[j].Split('\t')[0].TrimEnd())
        {
            jj = jjj = j;
            ii = iii = i;
            FindNextLine(goldStandard, toCompare, ref ii, ref jj);
            FindNextLine(toCompare, goldStandard, ref jjj, ref iii);

            if (iii + jjj < ii + jj)
            {
                i = iii;
                j = jjj;
            }
            else
            {
                i = ii;
                j = jj;
            }
        }
    }
 
    public void Compare()
    {
        var confusionMatrix = BuildConfusionMatrix(goldStandardFile, fileToCompare);        
        ConfusionMatrixWriter confusionMatrixWriter;
            
        if (outputForLatex)
            confusionMatrixWriter = new LatexWriter(confusionMatrixOutput, confusionMatrix, configuration);
        else
            confusionMatrixWriter = new TextPlainWriter(confusionMatrixOutput, confusionMatrix, configuration);
            
        confusionMatrixWriter.WriteMatrix();            
    }
}