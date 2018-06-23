using System.Collections.Generic;

namespace Sudoku.DAL.Domain
{
    /// <summary>
    /// A Sudoku board
    /// </summary>
    public class Board
    {
        public Board()
        {
            Cells = new HashSet<Cell>();
        }

        public ICollection<Cell> Cells { get; set; }
    }
}