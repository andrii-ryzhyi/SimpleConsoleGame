using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Common
{
    public static class Menu
    {
        public static string Show()
        {
            Console.WriteLine("1. New Game\n2. Continue\n3. Quit");
            Console.Write("Chose option: ");
            return Console.ReadKey().KeyChar.ToString();
        }
    }
}
