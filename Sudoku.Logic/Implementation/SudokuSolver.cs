using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Sudoku.Infrastructure.Configuration;
using Sudoku.Logic.Abstract;

namespace Sudoku.Logic.Implementation
{
    public class SudokuSolver : ISudokuSolver
    {
        private readonly ISudokuValidator _sudokuValidator;

        public SudokuSolver(ISudokuValidator sudokuValidator)
        {
            _sudokuValidator = sudokuValidator;
        }

        public IList<byte?[,]> Solve(byte?[,] board, bool solveAll = false)
        {
            var clonnedBoard = (byte?[,]) board.Clone();
            var result = new List<byte?[,]>();
            var combinedIndex = 0;
            var isReversed = false;
            var completed = false;
            var horizontalSize = board.GetUpperBound(0) + 1;
            var verticalSize = board.GetUpperBound(1) + 1;
            var cellCount = horizontalSize * verticalSize;

            while (!completed)
            {
                if (combinedIndex < 0)
                {
                    completed = true;
                    continue;
                }

                if (combinedIndex >= cellCount)
                {
                    result.Add((byte?[,]) clonnedBoard.Clone());

                    if (solveAll)
                    {
                        combinedIndex = cellCount - 1;
                        isReversed = true;
                    }
                    else
                    {
                        completed = true;
                        continue;
                    }
                }

                var i = combinedIndex / horizontalSize;
                var j = combinedIndex % verticalSize;

                if (board[i, j].HasValue)
                {
                    if (isReversed)
                        combinedIndex--;
                    else
                        combinedIndex++;

                    continue;
                }

                clonnedBoard[i, j] = clonnedBoard[i, j] ?? 0;
                clonnedBoard[i, j]++;

                while (!_sudokuValidator.Validate(clonnedBoard, i, j) && clonnedBoard[i, j] < 10)
                    clonnedBoard[i, j]++;

                if (clonnedBoard[i, j] == 10)
                {
                    clonnedBoard[i, j] = null;
                    combinedIndex--;
                    isReversed = true;
                }
                else
                {
                    combinedIndex++;
                    isReversed = false;
                }
            }
            return result;
        }
    }
}