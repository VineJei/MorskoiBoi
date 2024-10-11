using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShipGame
{
    public class Board
    {
        public int Size { get; }
        public List<Cell> Cells { get; }
        public List<Ship> Ships { get; set; } = new List<Ship>();

        public Board(int size)
        {
            Size = size;
            Cells = new List<Cell>();
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Cells.Add(new Cell(x, y));
                }
            }
        }

        public Cell GetCell(int x, int y) => Cells.FirstOrDefault(c => c.X == x && c.Y == y);

        public void Display()
        {
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    Console.Write(GetCell(x, y) + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
