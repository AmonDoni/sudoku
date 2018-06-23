using System;
using Microsoft.Extensions.Options;
using Sudoku.Infrastructure.Configuration;
using Sudoku.Logic.Abstract;

namespace Sudoku.Logic.Implementation
{
    public class SudokuValidator : ISudokuValidator
    {
        private readonly IOptions<BoardConfiguration> _options;

        public SudokuValidator(IOptions<BoardConfiguration> options)
        {
            _options = options;
        }

        public bool Validate(byte?[,] board)
        {
            for (var i = 0; i <= board.GetUpperBound(0); i++)
            for (var j = 0; j <= board.GetUpperBound(1); j++)
            {
                if (board[i, j].HasValue && !Validate(board, i, j)) return false;
            }

            return true;
        }

        public bool Validate(byte?[,] board, int i, int j)
        {
            return board[i, j].HasValue && IsValidInRow(board, i, j) && IsValidInColumn(board, i, j) &&
                   IsValidInBlock(board, i, j);
        }

        private bool IsValidInRow(byte?[,] board, int i, int j)
        {
            var result = true;
            for (var column = 0; column <= board.GetUpperBound(0); column++)
                if (column != j && board[i, column] == board[i, j])
                    result = false;

            return result;
        }

        private bool IsValidInColumn(byte?[,] board, int i, int j)
        {
            var result = true;
            for (var row = 0; row <= board.GetUpperBound(1); row++)
                if (row != i && board[row, j] == board[i, j])
                    result = false;

            return result;
        }

        private bool IsValidInBlock(byte?[,] board, int i, int j)
        {
            var blockSize = _options.Value.BlockSize;
            var rowBlockOffset = i / blockSize * blockSize;
            var columnBlockOffset = j / blockSize * blockSize;
            var result = true;

            for (var k = rowBlockOffset; k < blockSize + rowBlockOffset; k++)
            for (var l = columnBlockOffset; l < blockSize + columnBlockOffset; l++)
                if (k != i && l != j && board[k, l] == board[i, j])
                    result = false;
            return result;
        }
    }
}