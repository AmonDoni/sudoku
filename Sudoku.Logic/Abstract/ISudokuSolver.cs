using System.Collections.Generic;

namespace Sudoku.Logic.Abstract
{
    /// <summary>
    ///     Sudoku solver based on backtracking algorithm
    /// </summary>
    public interface ISudokuSolver
    {
        /// <summary>
        ///     Sudoku solver method
        /// </summary>
        /// <param name="board">Sudoku oard size</param>
        /// <param name="solveAll">Solve all possible solutions (optional)</param>
        /// <returns></returns>
        IList<byte?[,]> Solve(byte?[,] board, bool solveAll = false);
    }
}