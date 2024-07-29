using System;

public class TetrisBoard
{
    public const int Width = 10;
    public const int Height = 20;
    private int[,] board = new int[Width, Height];

    public void Initialize()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                board[x, y] = 0;
            }
        }
    }

    public bool CheckCollision(TetrisPiece piece, int posX, int posY)
    {
        for (int y = 0; y < piece.Shape.GetLength(0); y++)
        {
            for (int x = 0; x < piece.Shape.GetLength(1); x++)
            {
                if (piece.Shape[y, x] == 1)
                {
                    int boardX = posX + x;
                    int boardY = posY + y;

                    if (boardX < 0 || boardX >= Width || boardY < 0 || boardY >= Height)
                    {
                        return true;
                    }

                    if (board[boardX, boardY] == 1)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void PlacePiece(TetrisPiece piece, int posX, int posY)
    {
        for (int y = 0; y < piece.Shape.GetLength(0); y++)
        {
            for (int x = 0; x < piece.Shape.GetLength(1); x++)
            {
                if (piece.Shape[y, x] == 1)
                {
                    board[posX + x, posY + y] = 1;
                }
            }
        }
    }

    public int ClearLines()
    {
        int linesCleared = 0;
        for (int y = 0; y < Height; y++)
        {
            bool isLineComplete = true;
            for (int x = 0; x < Width; x++)
            {
                if (board[x, y] == 0)
                {
                    isLineComplete = false;
                    break;
                }
            }

            if (isLineComplete)
            {
                ClearLine(y);
                ShiftLinesDown(y);
                linesCleared++;
                y--;
            }
        }
        return linesCleared;
    }

    private void ClearLine(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            board[x, y] = 0;
        }
    }

    private void ShiftLinesDown(int fromLine)
    {
        for (int y = fromLine; y > 0; y--)
        {
            for (int x = 0; x < Width; x++)
            {
                board[x, y] = board[x, y - 1];
            }
        }

        for (int x = 0; x < Width; x++)
        {
            board[x, 0] = 0;
        }
    }

    public void Display()
    {
        Console.SetCursorPosition(0, 0);
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Console.Write(board[x, y] == 0 ? ". " : "# ");
            }
            Console.WriteLine();
        }
    }
}
