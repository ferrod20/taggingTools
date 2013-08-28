# Tagging Tools
Useful set of tools for POST (part of speech tagging) common tasks like confusion matrix generation and file split.

## Confusion matrix generator
Define 2 files to compare (gold standard and tagged file) and build the confusion matrix. The output result can be set as LaTex or plain text. 

You can see below an example of a pdf confusion matrix output. By default cells with biggest differences are showed.

###Pdf output:
![pdf confusion matrix example](https://raw.github.com/ferrod20/taggingTools/gh-pages/images/confusionMatrixPdf.png)

In the next example you can see a plain text confusion matrix output. Notice that for each cell appears the words which it's tags differs.

###Plain text output:
![plain text confusion matrix example](https://raw.github.com/ferrod20/taggingTools/gh-pages/images/plainTextConfusionMatrixComparision.png)

###Tags translation:
You can also use a tags translation file in order to convert tags from one tagset into another before the comparision occurs. Penn Treebank-C5 translation is included as example.

Command:
```dos
tt -compare <goldStandard> <fileToCompare> <output> [options]
```

Compares `goldStandard` file against `fileToCompare` generating a confusion matrix as output.

`[options]` is one or more of the followings:

`-l`: Set latex output, default is text plain output.

`-t=<matrixTitle>`: Matrix title to be written on output file.

`-rt=<rowTitle>`: Row title to be written on output file.

`-ct=<columnTitle>`: Column title to be written on output file.

`-SC=<specificCellsFile>`: Use a custom file to set specific cells to be showed.

`-T=<translationFile>`: Specify a custom tags translation file to be used on comparision. The 
translation is applied to `fileToCompare` tags in order to convert them to goldStandard tags.

`-s=<size>`: If latex output is set, size sets matrix size. If text plain output is set, size sets quantity of different words for each cell.

##Files splitter:
This tool splits a file in several parts, preserving sentences.
Optionally generates the complementary file for each extracted part.

Command:
```dos
tt -split <file> <parts> [options]
```
`[options]` is one or more of the followings:

`-c[=complementPrefix]`: Generate a complement file for each part. If specified, complement file names will be generated using `complementPrefix` as the base name. Default complementPrefix is 'Comp'

`-r`: Random selection of senteneces.

##File format:
Sentences are composed of tokens (words and symbols).

Each line should contains a token. Empty lines will be used to denote sentence break.

For comparision operatios, each line must contain the token followed by a tab and the POS tag.

Example:
![file format example](https://raw.github.com/ferrod20/taggingTools/gh-pages/images/fileFormat.png)

##Binaries:
Linux and Windows binaries are availables here: https://github.com/ferrod20/taggingTools/tree/binaries
