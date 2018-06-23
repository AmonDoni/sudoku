namespace Sudoku.DAL.Domain
{
    /// <summary>
    /// A cell on a Sudoku board
    /// </summary>
    public class Cell
    {
        public int I { get; set; }
        public int J { get; set; }
        public byte? Value { get; set; }
    }
}