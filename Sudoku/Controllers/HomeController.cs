using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sudoku.Infrastructure;
using Sudoku.Infrastructure.Configuration;
using Sudoku.Logic.Abstract;
using Sudoku.Models;

namespace Sudoku.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<BoardConfiguration> _options;
        private readonly ISudokuGenerator _sudokuGenerator;
        private readonly ISudokuSolver _sudokuSolver;
        private readonly ISudokuValidator _sudokuValidator;

        public HomeController(IOptions<BoardConfiguration> options, ISudokuGenerator sudokuGenerator,
            ISudokuValidator sudokuValidator, ISudokuSolver sudokuSolver)
        {
            _options = options;
            _sudokuGenerator = sudokuGenerator;
            _sudokuValidator = sudokuValidator;
            _sudokuSolver = sudokuSolver;
        }

        private void SetBlockSize()
        {
            ViewData["BlockSize"] = _options.Value.BlockSize;
        }

        private byte?[,] GenerateBoard()
        {
            var board = _sudokuGenerator.Generate();
            HttpContext.Session.SetObjectAsJson("lastGeneratedBoard", board.Clone());
            return board;
        }

        private static byte?[,] GetTwoDimentional(byte?[][] boardForm)
        {
            return new[] { new byte?[boardForm.Length, boardForm[0].Length] }
                .Select(_ => new
                {
                    x = _,
                    y = boardForm.Select((a, ia) => a.Select((b, ib) => _[ia, ib] = b).Count()).Count()
                })
                .Select(_ => _.x)
                .First();
        }

        public IActionResult Index()
        {
            SetBlockSize();

            var board = GenerateBoard();

            return View(board);
        }

        public IActionResult Update([FromBody] byte?[][] jsonBoard)
        {
            SetBlockSize();

            var board = GetTwoDimentional(jsonBoard);
            var isValid = _sudokuValidator.Validate(board);

            ViewData["IsBoardInvalid"] = !isValid;
            return Ok(isValid);
        }

        public IActionResult Generate()
        {
            SetBlockSize();

            var board = GenerateBoard();

            return View("Index", board);
        }
        
        public IActionResult Solve()
        {
            SetBlockSize();

            var lastGeneratedBoard = HttpContext.Session.GetObjectFromJson<byte?[,]>("lastGeneratedBoard");
            var solve = _sudokuSolver.Solve(lastGeneratedBoard);

            if (solve.Any())
            {
                return View("Index", solve.First());
            }

            ViewData["IsBoardInvalid"] = true;
            return View("Index", lastGeneratedBoard);
        }

        public IActionResult Reset()
        {
            SetBlockSize();

            ViewData["IsBoardInvalid"] = false;
            return View("Index", HttpContext.Session.GetObjectFromJson<byte?[,]>("lastGeneratedBoard"));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}