using System;
using System.Threading;

public class TetrisGame
{
    private TetrisBoard board;
    private TetrisPiece currentPiece;
    private int posX, posY;
    private int score;
    private DateTime lastFallTime;
    private bool pieceSettled;

    private static readonly TetrisPiece[] pieces = new TetrisPiece[]
    {
        new TetrisPiece(new int[,] { { 1, 1, 1, 1 } }), // I piece
        new TetrisPiece(new int[,] { { 1, 1 }, { 1, 1 } }), // O piece
        new TetrisPiece(new int[,] { { 0, 1, 0 }, { 1, 1, 1 } }), // T piece
        new TetrisPiece(new int[,] { { 1, 1, 0 }, { 0, 1, 1 } }), // S piece
        new TetrisPiece(new int[,] { { 0, 1, 1 }, { 1, 1, 0 } }), // Z piece
        new TetrisPiece(new int[,] { { 1, 1, 1 }, { 1, 0, 0 } }), // L piece
        new TetrisPiece(new int[,] { { 1, 1, 1 }, { 0, 0, 1 } })  // J piece
    };

    public TetrisGame()
    {
        board = new TetrisBoard();
        score = 0;
        lastFallTime = DateTime.Now;
    }

    public void Start()
    {
        board.Initialize();
        if (!StartNewPiece())
        {
            Console.Clear();
            Console.WriteLine("Game Over!");
            return;
        }

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                HandleInput(key);
            }

            if ((DateTime.Now - lastFallTime).TotalMilliseconds >= 500)
            {
                lastFallTime = DateTime.Now;
                AutoFall();
            }

            if (pieceSettled)
            {
                pieceSettled = false;
                score += board.ClearLines() * 100;
                if (!StartNewPiece())
                {
                    Console.Clear();
                    Console.WriteLine("Game Over!");
                    break;
                }
                Redraw();
            }
        }
    }

    private void HandleInput(ConsoleKey key)
    {
        int newX = posX, newY = posY;
        TetrisPiece newPiece = currentPiece;

        switch (key)
        {
            case ConsoleKey.LeftArrow:
                newX--;
                break;
            case ConsoleKey.RightArrow:
                newX++;
                break;
            case ConsoleKey.DownArrow:
                newY++;
                break;
            case ConsoleKey.UpArrow:
                newPiece = currentPiece.Rotate();
                break;
        }

        if (!board.CheckCollision(newPiece, newX, newY))
        {
            currentPiece = newPiece;
            posX = newX;
            posY = newY;
            Redraw();
        }
    }

    private void AutoFall()
    {
        int newY = posY + 1;

        if (board.CheckCollision(currentPiece, posX, newY))
        {
            board.PlacePiece(currentPiece, posX, posY);
            pieceSettled = true;
        }
        else
        {
            posY = newY;
            Redraw();
        }
    }

    private bool StartNewPiece()
    {
        currentPiece = pieces[new Random().Next(pieces.Length)];
        posX = 3;
        posY = 0;

        if (board.CheckCollision(currentPiece, posX, posY))
        {
            return false;
        }

        Redraw();
        return true;
    }

    private void Redraw()
    {
        Console.Clear();
        board.Display();
        DisplayPiece(currentPiece, posX, posY);
        DisplayScore();
    }

    private void DisplayPiece(TetrisPiece piece, int posX, int posY)
    {
        for (int y = 0; y < piece.Shape.GetLength(0); y++)
        {
            for (int x = 0; x < piece.Shape.GetLength(1); x++)
            {
                if (piece.Shape[y, x] == 1)
                {
                    Console.SetCursorPosition((posX + x) * 2, posY + y);
                    Console.Write("# ");
                }
            }
        }
    }

    private void DisplayScore()
    {
        Console.SetCursorPosition(TetrisBoard.Width * 2 + 2, 0);
        Console.Write($"Score: {score}");
    }
}
