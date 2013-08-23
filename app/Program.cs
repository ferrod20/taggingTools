using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

internal class Program
    {
        private static void Main(string[] args)
        {
            if(args.Length == 0)
                Console.Write("-help for information");
            else
            {
                var command = args[0];
                Console.WriteLine();
                args = args.Select(a => a.Replace("\"", "")).ToArray();
                switch (command)
                {                    
                    case "-compare":
                        Compare(args);
                        break;
                    case "-split":
                        Split(args);
                        break;
                    case "-help":
                        Help();
                        break;
                    
                    default:
                        Console.WriteLine("-help for information");
                        break;
                }
            }
        }

        private static void Help()
        {

            Console.Write(@"-help: show this
            
-split <file> <parts> [options]
            
Splits the specified file in several parts.
[options] is one or more of the followings:
    -c[=complementPrefix]: Generate complement file for each part. If specified, complement file names will be generated using complementPrefix as the base name. Default complementPrefix is 'Comp'
    -r: random selection for senteneces

-compare <goldStandard> <fileToCompare> <output> [options]

Compares goldStandard file against fileToCompare generating a confusion matrix as output.
[options] is one or more of the followings:
    -l: set latex output, default is text plain output
    -t=<matrixTitle>: matrix title to be written on output file
    -rt=<rowTitle>: row title to be written on output file
    -ct=<columnTitle>: column title to be written on output file
    -SC=<specificCellsFile>: use a custom file to set specific cells to be showed 
    -T=<translationFile>: specify a custom tags translation file to be used on comparision. The translation is applied to fileToCompare tags in order to convert them to goldStandard tags.
    -s=<size>: if latex output is set, size sets matrix size. If text plain output is set, size sets quantity of different words for each cell.");

        }

        private static void Split(IList<string> args)
        {
            if (args.Count < 3)
            {
                Console.WriteLine("Use -split <file> <parts> [options]");
                Console.Write("-help for more information");
            }
            else
            {
                var file = args[1];
                var parts = args[2];

                var randomSentencesChoose = args.Contains("-r");
                var complementPrefix = "Comp";
                var generateComplement = args.OptionPresent("c", ref complementPrefix);

                Console.WriteLine("Splitting: {0} in {1} parts", Path.GetFileName(file), parts);
                if(generateComplement)
                    Console.WriteLine("Generating complement for each part");
                Console.WriteLine();

                FilesSplitter filesSplitter;
                if(randomSentencesChoose)
                    filesSplitter = new RandomFilesSplitter(file, int.Parse(parts), generateComplement, complementPrefix);
                else
                    filesSplitter = new SimpleFilesSplitter(file, int.Parse(parts), generateComplement, complementPrefix);
                
                filesSplitter.Execute();
            }
        }


    private static void Compare(IList<string> args)
        {
            if (args.Count < 3)
            {
                Console.WriteLine("Use -compare <goldStandard> <fileToCompare> <output> [options]");
                Console.WriteLine("-help for more information");
            }
            else
            {
                var goldStandardFile = args[1];
                var fileToCompare = args[2];
                var outputFile = args[3];

                var goldStandardFileName = Path.GetFileName(goldStandardFile);
                var fileToCompareName = Path.GetFileName(fileToCompare);
                var outputFileName = Path.GetFileName(outputFile);

                string title = outputFileName, rowTitle = goldStandardFileName, columnTitle = fileToCompareName, specificCellsFile = "", translationFile = "";
                var outputForLatex = args.Contains("-l");
                var size = outputForLatex ? 10 : 30;

                args.OptionPresent("t", ref title);
                args.OptionPresent("rt", ref rowTitle);
                args.OptionPresent("ct", ref columnTitle);
                args.OptionPresent("SC", ref specificCellsFile);
                args.OptionPresent("T", ref translationFile);
                args.OptionPresent("s", ref size);

                Console.WriteLine((outputForLatex ? "(latex)" : "") + "Comparing: {0}(gold standard) against {1}", goldStandardFileName , fileToCompareName);
                Console.WriteLine("output: {0}", outputFileName);
                Console.WriteLine();

                var comparator = new Comparator(goldStandardFile, fileToCompare, outputFile);
                var translation = string.IsNullOrWhiteSpace(translationFile) ? new EmptyTranslation():(ITranslation)new Translation(translationFile);
                comparator.SetOptions(outputForLatex, title, rowTitle, columnTitle, translation, specificCellsFile, size);
                comparator.Compare();
            }
        }        
    }