public class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsHit { get; set; } = false; // Ударена ли клетка
    public bool HasShip { get; set; } = false; // Есть ли корабль
    public bool IsMarkedAsCross { get; set; } = false; // Пометка крестом при обводке

    public Cell(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        if (IsMarkedAsCross) return "o"; // Клетка, окруженная крестом
        if (IsHit)
            return HasShip ? "X" : "o"; // Если клетка была ударена, и там был корабль - "X", иначе "o"
        return "."; // Неизвестная (не стреляли)
    }
}
