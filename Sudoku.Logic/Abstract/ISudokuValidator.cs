namespace Sudoku.Logic.Abstract
{
    public interface ISudokuValidator
    {
        bool Validate(byte?[,] board);
        bool Validate(byte?[,] board, int i, int j);
    }
}