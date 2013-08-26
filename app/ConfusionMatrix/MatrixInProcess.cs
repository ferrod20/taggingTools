using System.Collections.Generic;
using System.Linq;

public class Matrix
{
    public Matrix()
    {
        cells = new List<Cell>();
    }

    protected List<Cell> cells;        
}
    
public class MatrixInProcess: Matrix
{
    public MatrixInProcess()
    {
        tagsCount = 0;
    }

    private int tagsCount;

    public void AddError(string tagGoldStandard, string tagDePrueba, string palabra)
    {
        var tags = new Cell(tagGoldStandard, tagDePrueba, palabra);

        if (cells.Contains(tags))
        {
            var i = cells.IndexOf(tags);
            cells[i].AddWord(palabra);
        }
        else
            cells.Add(tags);
    }
        
    public void AddTag()
    {
        tagsCount++;
    }

    private int ErrorsCount()
    {
        return cells.Sum(s => s.TotalWordsOcurrencies);
    }        
        
    public ConfusionMatrix FinalizeProcces()
    {
        var cantidadDeErrores = ErrorsCount();
        return new ConfusionMatrix(cells, tagsCount, cantidadDeErrores);
    }
}