using System.IO;

public class FilesManager
{
    protected TextWriter output;
    protected string originalFile;

    public FilesManager(string originalFile)
    {
        this.originalFile = originalFile;
    }
}