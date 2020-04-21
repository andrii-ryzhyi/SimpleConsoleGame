using System;
using System.Threading.Tasks;
using Game.Common;
using Game.Main;

namespace Game
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                BaseGame game = new BaseGame(16, 14);
                var opt = Menu.Show();
                switch(opt)
                {
                    case "1":
                        await game.StartAsync();
                        break;
                    case "2":
                        await game.LoadAsync();
                        break;
                    default:
                        System.Environment.Exit(0);
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
