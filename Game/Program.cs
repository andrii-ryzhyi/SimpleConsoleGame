using System;
using Game.Common;
using Game.Main;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                BaseGame game = new BaseGame(16, 14);
                var opt = SaveGame.Menu();
                switch(opt)
                {
                    case 1:
                        game.Start();
                        break;
                    case 2:
                        game.Load();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }            
        }
    }
}
