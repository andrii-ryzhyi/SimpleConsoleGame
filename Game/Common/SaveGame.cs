using Game.Main;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Common
{
    public static class SaveGame
    {
        public static async Task Save<T>(string path, T obj)
        {
            using (var asyncfs = new FileStream($"{path}.json", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, 4096, true))
            {
                string jsonData = JsonConvert.SerializeObject(obj, Formatting.Indented, settings: new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects, TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple });
                byte[] data = jsonData
                    .Select(x => (byte)x)
                    .ToArray();
                asyncfs.SetLength(0);
                await asyncfs.WriteAsync(data, 0, data.Length);
            }
        }

        public static async Task AutoSaveAsync(BaseGame game)
        {
            string jsonData = JsonConvert.SerializeObject(game, Formatting.Indented, settings: new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects, TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple });
            byte[] data = jsonData
                .Select(x => (byte)x)
                .ToArray();
            using (var asyncfs = new FileStream("autosave.sav", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, 4096, true))
            {
                //await largeJson.WriteToAsync(new JsonTextWriter(new StreamWriter(asyncFileStream)));
                await asyncfs.WriteAsync(data, 0, data.Length);
                Console.WriteLine("AutoSave method end");
            }
        }

        public static async Task<T> Load<T>(string path)
        {
            using (var streamReader = new StreamReader($"{path}.json"))
            {
                string jsonData = await streamReader.ReadToEndAsync();
                var restore = JsonConvert.DeserializeObject<T>(jsonData, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                return restore;             
            }
        }        

        public static string Menu()
        {
            Console.WriteLine("1. New Game\n2. Continue\n3. Quit");
            Console.Write("Chose option: ");
            return Console.ReadKey().KeyChar.ToString();               
        }
    }
}
