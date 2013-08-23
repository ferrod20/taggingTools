public abstract class FilesSplitter : FilesManager
{
    protected int howManyParts;
    protected bool generateComplement;
    protected string complementPrefix;

    protected FilesSplitter(string originalFile, int howManyParts, bool generateComplement, string complementPrefix)
        : base(originalFile)
    {
        this.howManyParts = howManyParts;
        this.generateComplement = generateComplement;
        this.complementPrefix = complementPrefix;
    }

    public abstract void Execute();
}