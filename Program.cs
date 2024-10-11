using System;

namespace BattleShipGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(10);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Ваше поле:");
                game.PlayerBoard.Display();
                Console.WriteLine("Поле компьютера:");
                game.ComputerBoard.Display();

                game.PlayerMove();
                if (game.CheckVictory(game.ComputerBoard))
                {
                    Console.WriteLine("Вы победили!");
                    break;
                }

                game.ComputerMove();
                if (game.CheckVictory(game.PlayerBoard))
                {
                    Console.WriteLine("Компьютер победил!");
                    break;
                }
            }

            Console.WriteLine("Игра завершена. Нажмите любую клавишу для выхода.");
            Console.ReadKey();
        }
    }
}
