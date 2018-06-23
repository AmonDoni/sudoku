namespace Sudoku.Logic
{
    /// <summary>
    ///     Board valid mutation extension methods
    /// </summary>
    public static class BoardExtensions
    {
        /// <summary>
        ///     Transpose board
        /// </summary>
        /// <param name="board">Board</param>
        /// <param name="blockSize">Block size</param>
        /// <param name="first">First row/column number</param>
        /// <param name="second">Second row/column number</param>
        /// <returns>Board</returns>
        public static byte?[,] Transpose(this byte?[,] board, byte blockSize, byte first, byte second)
        {
            var horizontalSize = board.GetUpperBound(0) + 1;
            var verticalSize = board.GetUpperBound(1) + 1;
            var temp = new byte?[verticalSize, horizontalSize];

            for (var i = 0; i < horizontalSize; i++)
            for (var j = 0; j < verticalSize; j++)
                temp[j, i] = board[i, j];

            return temp;
        }

        /// <summary>
        ///     Swap board's rows
        /// </summary>
        /// <param name="board">Board</param>
        /// <param name="blockSize">Block size</param>
        /// <param name="first">First row number</param>
        /// <param name="second">Second row number</param>
        /// <returns>Board</returns>
        public static byte?[,] SwapRow(this byte?[,] board, byte blockSize, byte first, byte second)
        {
            if (first == second) return board;

            if (first / blockSize != second / blockSize) return board;

            var swappedBoard = (byte?[,]) board.Clone();

            for (var j = 0; j <= swappedBoard.GetUpperBound(1); j++)
            {
                var temp = swappedBoard[first, j];
                swappedBoard[first, j] = swappedBoard[second, j];
                swappedBoard[second, j] = temp;
            }


            return swappedBoard;
        }

        /// <summary>
        ///     Swap board's columns
        /// </summary>
        /// <param name="board">Board</param>
        /// <param name="blockSize">Block size</param>
        /// <param name="first">First column number</param>
        /// <param name="second">Second column number</param>
        /// <returns>Board</returns>
        public static byte?[,] SwapColumn(this byte?[,] board, byte blockSize, byte first, byte second)
        {
            if (first == second) return board;

            if (first / blockSize != second / blockSize) return board;

            var swappedBoard = (byte?[,]) board.Clone();

            for (var i = 0; i <= swappedBoard.GetUpperBound(0); i++)
            {
                var temp = swappedBoard[i, first];
                swappedBoard[i, first] = swappedBoard[i, second];
                swappedBoard[i, second] = temp;
            }

            return swappedBoard;
        }

        /// <summary>
        ///     Swap board's horizontal blocks
        /// </summary>
        /// <param name="board">Board</param>
        /// <param name="blockSize">Block size</param>
        /// <param name="first">First horizontal block number</param>
        /// <param name="second">Second horizontal block number</param>
        /// <returns>Board</returns>
        public static byte?[,] SwapRowBlock(this byte?[,] board, byte blockSize, byte first, byte second)
        {
            first = first >= blockSize ? (byte) (first + 1 / blockSize) : first;
            second = second >= blockSize ? (byte) (second + 1 / blockSize) : second;

            if (first == second) return board;

            if (first / blockSize == second / blockSize) return board;

            var swappedBoard = (byte?[,]) board.Clone();

            var startIndexOfFirstBlock = blockSize * (first / blockSize);
            var startIndexOfSecondBlock = blockSize * (second / blockSize);

            for (var b = 0; b < blockSize; b++)
            for (var j = 0; j <= swappedBoard.GetUpperBound(1); j++)
            {
                var temp = swappedBoard[startIndexOfFirstBlock + b, j];
                swappedBoard[startIndexOfFirstBlock + b, j] = swappedBoard[startIndexOfSecondBlock + b, j];
                swappedBoard[startIndexOfSecondBlock + b, j] = temp;
            }

            return swappedBoard;
        }

        /// <summary>
        ///     Swap board's vertical blocks
        /// </summary>
        /// <param name="board">Board</param>
        /// <param name="blockSize">Block size</param>
        /// <param name="first">First vertical block number</param>
        /// <param name="second">Second vertical block number</param>
        /// <returns>Board</returns>
        public static byte?[,] SwapColumnBlock(this byte?[,] board, byte blockSize, byte first, byte second)
        {
            first = first >= blockSize ? (byte) (first + 1 / blockSize) : first;
            second = second >= blockSize ? (byte) (second + 1 / blockSize) : second;

            if (first == second) return board;

            if (first / blockSize == second / blockSize) return board;

            var swappedBoard = (byte?[,]) board.Clone();

            var startIndexOfFirstBlock = blockSize * (first / blockSize);
            var startIndexOfSecondBlock = blockSize * (second / blockSize);

            for (var b = 0; b < blockSize; b++)
            for (var i = 0; i <= swappedBoard.GetUpperBound(0); i++)
            {
                var temp = swappedBoard[i, startIndexOfFirstBlock + b];
                swappedBoard[i, startIndexOfFirstBlock + b] = swappedBoard[i, startIndexOfSecondBlock + b];
                swappedBoard[i, startIndexOfSecondBlock + b] = temp;
            }
            
            return swappedBoard;
        }

        //public static int Count<T>(this T[,] board, Func<T, bool> func)
        //{
        //    var count = 0;
        //    for (var i = 0; i <= board.GetUpperBound(0); i++)
        //    for (var j = 0; j <= board.GetUpperBound(1); j++)
        //        if (func(board[i, j]))
        //            count++;

        //    return count;
        //}

        //public static bool IsEqualTo<T>(this T[,] leftBoard, T[,] rightBoard)
        //{
        //    if (leftBoard.Length != rightBoard.Length)
        //        return false;

        //    var equal = true;

        //    for (var i = 0; i <= leftBoard.GetUpperBound(0); i++)
        //    for (var j = 0; j <= leftBoard.GetUpperBound(1); j++)
        //        equal = equal && leftBoard[i, j].Equals(rightBoard[i, j]);

        //    return equal;
        //}
    }
}