using Game.Main;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Game.Common
{
    public static class SaveGame
    {
        public static void Save(string path, BaseGame game)
        {
            using (var fs = new FileStream($"{path}.json", FileMode.OpenOrCreate))
            {
                string strObj = JsonConvert.SerializeObject(game);
                byte[] data = strObj
                    .Select(x => (byte)x)
                    .ToArray();
                fs.Write(data, 0, data.Length);
            }
        }

        public static void Load(string path, BaseGame originGame)
        {
            BaseGame restore = null;

            using (var streamReader = new StreamReader($"{path}.json"))
            {
                string dataStr = streamReader.ReadToEnd();
                restore = JsonConvert.DeserializeObject<BaseGame>(dataStr);
            }
            
            originGame.World = restore.World;
            originGame.Character1 = restore.Character1;
            originGame.Character1.World = restore.World;
            originGame.Character2 = restore.Character2;
            originGame.Character2.World = restore.World;
        }
    }
}
