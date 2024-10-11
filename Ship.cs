using System.Collections.Generic;
using System.Linq;

namespace BattleShipGame
{
    public class Ship
    {
        public List<Cell> Cells { get; set; }

        public Ship(List<Cell> cells)
        {
            Cells = cells;
        }

        public bool IsSunk => Cells.All(c => c.IsHit);
    }
}
