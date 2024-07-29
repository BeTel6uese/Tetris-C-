public class TetrisPiece
{
    public int[,] Shape { get; private set; }

    public TetrisPiece(int[,] shape)
    {
        Shape = shape;
    }

    public TetrisPiece Rotate()
    {
        int height = Shape.GetLength(0);
        int width = Shape.GetLength(1);
        int[,] newShape = new int[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                newShape[x, height - 1 - y] = Shape[y, x];
            }
        }

        return new TetrisPiece(newShape);
    }
}
