using Game.Main;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Game.Common
{
    public static class SaveGame
    {
        public static void Save<T>(string path, T obj)
        {
            using (var fs = new FileStream($"{path}.json", FileMode.OpenOrCreate))
            {
                string jsonData = JsonConvert.SerializeObject(obj, Formatting.Indented, settings: new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects, TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple });
                byte[] data = jsonData
                    .Select(x => (byte)x)
                    .ToArray();
                fs.SetLength(0);
                fs.Write(data, 0, data.Length);
            }
        }

        public static T Load<T>(string path)
        {
            using (var streamReader = new StreamReader($"{path}.json"))
            {
                string jsonData = streamReader.ReadToEnd();
                var restore = JsonConvert.DeserializeObject<T>(jsonData, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                return restore;             
            }
        }        

        public static int Menu()
        {
            Console.WriteLine("1. New Game\n2. Continue\n3. Quit");
            Console.Write("Chose option: ");
            string opt = Console.ReadKey().KeyChar.ToString();
            Console.ReadLine();
            Console.WriteLine();
            switch (opt)
            {
                case "1":
                    return 1;
                case "2":
                    return 2;
                default:
                    System.Environment.Exit(0);
                    return 0;
            }
        }
    }
}
