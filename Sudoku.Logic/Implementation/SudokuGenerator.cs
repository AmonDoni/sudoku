using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Sudoku.Infrastructure.Configuration;
using Sudoku.Logic.Abstract;

namespace Sudoku.Logic.Implementation
{
    public class SudokuGenerator : ISudokuGenerator
    {
        // ReSharper disable once InconsistentNaming
        private const byte MIN_STRATEGY_COUNT = 10;

        // ReSharper disable once InconsistentNaming
        private const byte MAX_STRATEGY_COUNT = 20;

        private readonly byte _blockSize;
        private readonly byte _boardSize;
        private readonly List<Func<byte?[,], byte, byte, byte, byte?[,]>> _mixStrategies;
        private readonly Random _random;
        private readonly ISudokuSolver _sudokuSolver;

        public SudokuGenerator(IOptions<BoardConfiguration> options, ISudokuSolver sudokuSolver)
        {
            _sudokuSolver = sudokuSolver;
            _boardSize = options.Value.BoardSize;
            _blockSize = options.Value.BlockSize;
            _mixStrategies = new List<Func<byte?[,], byte, byte, byte, byte?[,]>>
            {
                BoardExtensions.Transpose,
                BoardExtensions.SwapRow,
                BoardExtensions.SwapRowBlock,
                BoardExtensions.SwapColumn,
                BoardExtensions.SwapColumnBlock
            };
            _random = new Random();
        }

        public byte?[,] Generate()
        {
            var applyStrategyCount = _random.Next(MIN_STRATEGY_COUNT, MAX_STRATEGY_COUNT);

            var board = GetInitialBoard();
            
            var mixedInitialBoard = Enumerable
                .Range(1, applyStrategyCount)
                .Select(c => _mixStrategies[_random.Next(1, _mixStrategies.Count)])
                .Aggregate(board,
                    (currentBoard, strategy) => strategy.Invoke(currentBoard, _blockSize,
                        (byte) _random.Next(0, _boardSize - 1),
                        (byte) _random.Next(0, _boardSize - 1)));

            return GetPreparedBoard(mixedInitialBoard);
        }

        private byte?[,] GetPreparedBoard(byte?[,] mixedInitialBoard)
        {
            var cellsReadMatrix = new byte[_boardSize, _boardSize];
            var processedCellCount = 0;

            while (processedCellCount < mixedInitialBoard.Length)
            {
                var i = _random.Next(0, _boardSize);
                var j = _random.Next(0, _boardSize);

                if (cellsReadMatrix[i, j] == 0)
                {
                    processedCellCount++;
                    cellsReadMatrix[i, j] = 1;

                    var temp = mixedInitialBoard[i, j];
                    mixedInitialBoard[i, j] = null;

                    var solve = _sudokuSolver.Solve(mixedInitialBoard, true);
                    if (solve.Count != 1)
                        mixedInitialBoard[i, j] = temp;
                }
            }

            return mixedInitialBoard;
        }

        /// <summary>
        ///     Sudoku's valid initial matrix creator method
        ///     https://habr.com/post/192102/
        /// </summary>
        /// <returns>Initial matrix</returns>
        private byte?[,] GetInitialBoard()
        {
            var initialBoard = new byte?[_boardSize, _boardSize];
            for (var i = 0; i < _boardSize; i++)
            for (var j = 0; j < _boardSize; j++)
            {
                //position offset (in increments of block size) to the left on each line
                var lineOffset = i * _blockSize + j;
                //position offset (in increments of one) to the left on each block
                var blockOffset = i / _blockSize;
                var zeroOrderValue = (lineOffset + blockOffset) % _boardSize;
                initialBoard[i, j] = (byte) (zeroOrderValue + 1);
            }

            return initialBoard;
        }
    }
}