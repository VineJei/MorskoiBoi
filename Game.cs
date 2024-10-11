using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShipGame
{
    public class Game
    {
        public Board PlayerBoard { get; set; }
        public Board ComputerBoard { get; set; }

        public Game(int boardSize)
        {
            PlayerBoard = new Board(boardSize);
            ComputerBoard = new Board(boardSize);

            PlaceShips(PlayerBoard);
            PlaceShips(ComputerBoard);
        }

        private void PlaceShips(Board board)
        {
            PlaceShip(board, 4);  // Корабль длиной 4
            PlaceShip(board, 3);  // Два корабля длиной 3
            PlaceShip(board, 3);
            PlaceShip(board, 2);  // три корабля длиной 2
            PlaceShip(board, 2);  
            PlaceShip(board, 2);
            PlaceShip(board, 1);  // Четыре корабля длиной 1
            PlaceShip(board, 1);
            PlaceShip(board, 1);
            PlaceShip(board, 1);
        }

        private void PlaceShip(Board board, int shipSize)
        {
            var rand = new Random();
            bool isPlaced = false;

            while (!isPlaced)
            {
                int x = rand.Next(0, board.Size);
                int y = rand.Next(0, board.Size);
                bool horizontal = rand.Next(0, 2) == 0;

                var shipCells = new List<Cell>();
                for (int i = 0; i < shipSize; i++)
                {
                    int newX = horizontal ? x + i : x;
                    int newY = horizontal ? y : y + i;

                    if (!IsCellValid(board, newX, newY)) break;

                    var cell = board.GetCell(newX, newY);
                    shipCells.Add(cell);
                }

                if (shipCells.Count == shipSize && CanPlaceShip(board, shipCells))
                {
                    foreach (var cell in shipCells)
                    {
                        cell.HasShip = true;
                    }
                    board.Ships.Add(new Ship(shipCells));
                    isPlaced = true;
                }
            }
        }

        private bool CanPlaceShip(Board board, List<Cell> shipCells)
        {
            foreach (var cell in shipCells)
            {
                if (cell.HasShip || !IsCellValid(board, cell.X, cell.Y))
                    return false;

                foreach (var neighbor in GetNeighboringCells(board, cell.X, cell.Y))
                {
                    if (neighbor.HasShip)
                        return false;
                }
            }
            return true;
        }

        private List<Cell> GetNeighboringCells(Board board, int x, int y)
        {
            var neighbors = new List<Cell>();

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    var neighbor = board.GetCell(x + dx, y + dy);
                    if (neighbor != null)
                        neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        private bool IsCellValid(Board board, int x, int y) =>
            x >= 0 && x < board.Size && y >= 0 && y < board.Size;

        public void SurroundWithCrosses(Board board, Ship ship)
        {
            foreach (var cell in ship.Cells)
            {
                foreach (var neighbor in GetNeighboringCells(board, cell.X, cell.Y))
                {
                    if (!neighbor.IsHit && !neighbor.HasShip)
                    {
                        neighbor.IsHit = true;
                        neighbor.IsMarkedAsCross = true;
                    }
                }
            }
        }

        public void PlayerMove()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Ваш ход! Введите координаты для выстрела (например, 2 3):");
            Console.ResetColor();

            // Ввод координат (требует, чтобы были разделены пробелом)
            string[] input = Console.ReadLine()?.Split();
            if (input?.Length == 2 && int.TryParse(input[0], out int x) && int.TryParse(input[1], out int y))
            {
                x -= 1; // Корректировка, чтобы игрок видел координаты с 1
                y -= 1;

                var cell = ComputerBoard.GetCell(x, y);
                if (cell != null && !cell.IsHit)
                {
                    cell.IsHit = true;
                    Console.WriteLine();

                    if (cell.HasShip)
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Красный цвет для попадания
                        Console.WriteLine($"Попадание в ({x + 1}, {y + 1})!");
                        Console.ResetColor();
                        var hitShip = ComputerBoard.Ships.FirstOrDefault(ship => ship.Cells.Contains(cell));
                        if (hitShip != null && hitShip.IsSunk)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Корабль уничтожен!");
                            Console.ResetColor();
                            SurroundWithCrosses(ComputerBoard, hitShip);
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue; // Синий цвет для промаха
                        Console.WriteLine($"Мимо в ({x + 1}, {y + 1}).");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Некорректный ход или клетка уже обстреливалась.");
                    Console.ResetColor();
                }

                // Показать поле компьютера после хода игрока
                Console.WriteLine("Поле компьютера после вашего хода:");
                DisplayBoard(ComputerBoard, false);

                // Пауза перед ходом компьютера
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ResetColor();
                Console.ReadKey();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Неправильный ввод координат.");
                Console.ResetColor();
            }
        }

        public void ComputerMove()
        {
            var rand = new Random();
            int x, y;
            Cell cell;
            do
            {
                x = rand.Next(0, PlayerBoard.Size);
                y = rand.Next(0, PlayerBoard.Size);
                cell = PlayerBoard.GetCell(x, y);
            } while (cell.IsHit);

            cell.IsHit = true;

            Console.WriteLine();
            if (cell.HasShip)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Компьютер попал в ({x + 1}, {y + 1})!");
                Console.ResetColor();
                var hitShip = PlayerBoard.Ships.FirstOrDefault(ship => ship.Cells.Contains(cell));
                if (hitShip != null && hitShip.IsSunk)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Компьютер потопил ваш корабль!");
                    Console.ResetColor();
                    SurroundWithCrosses(PlayerBoard, hitShip);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Компьютер промахнулся в ({x + 1}, {y + 1}).");
                Console.ResetColor();
            }

            // Показать поле игрока после хода компьютера
            Console.WriteLine("Ваше поле после хода компьютера:");
            DisplayBoard(PlayerBoard, true);

            // Пауза перед следующим ходом
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ResetColor();
            Console.ReadKey();
        }


        public void DisplayBoard(Board board, bool showShips)
        {
            // Отображение верхних индикаторов для столбцов (от 1 до размера поля)
            Console.Write("   "); // Отступ для левого верхнего угла
            for (int x = 0; x < board.Size; x++)
            {
                Console.Write($"{x + 1} "); // Печать номеров столбцов
            }
            Console.WriteLine();

            for (int y = 0; y < board.Size; y++)
            {
                // Печать номера строки слева (выравнивание для корректного отображения)
                Console.Write($"{y + 1,2} ");

                for (int x = 0; x < board.Size; x++)
                {
                    var cell = board.GetCell(x, y);

                    if (cell.IsHit)
                    {
                        // Если в клетке было попадание
                        if (cell.HasShip)
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Красный цвет для попадания
                            Console.Write("X ");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Blue; // Синий цвет для промаха
                            Console.Write("o ");
                        }
                    }
                    else
                    {
                        // Отображение скрытых кораблей, если showShips = true
                        if (showShips && cell.HasShip)
                        {
                            Console.ForegroundColor = ConsoleColor.Green; // Зеленый цвет для кораблей
                            Console.Write("S ");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White; // Белый цвет для пустых клеток
                            Console.Write(". ");
                        }
                    }

                    // Сброс цвета после каждой клетки
                    Console.ResetColor();
                }
                Console.WriteLine(); // Переход на следующую строку
            }
        }




        public bool CheckVictory(Board board) => board.Ships.All(ship => ship.IsSunk);

        public void StartGame()
        {
            while (true)
            {
                // Ход игрока
                PlayerMove();
                if (CheckVictory(ComputerBoard))
                {
                    Console.WriteLine("Поздравляем! Вы победили!");
                    break;
                }

                // Ход компьютера
                ComputerMove();
                if (CheckVictory(PlayerBoard))
                {
                    Console.WriteLine("К сожалению, вы проиграли.");
                    break;
                }
            }
        }
    }
}
