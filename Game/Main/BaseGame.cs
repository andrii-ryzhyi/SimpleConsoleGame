﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Common;
using Game.Exceptions;
using Game.GameObjects;
using Game.Weapons;

namespace Game.Main
{
    [Serializable]
    public class BaseGame
    {
        public Character Character1 { get; set; }
        public Character Character2 { get; set; }
        public int Turn { get; set; }
        public int MapSize1 { get; set; }
        public int MapSize2 { get; set; }
        public Map World { get; set; }
        public List<GameObject> GameObjects { get; set; } = new List<GameObject>();

        public BaseGame(int mapSize1, int mapSize2)
        {
            MapSize1 = mapSize1;
            MapSize2 = mapSize2;
        }

        public Character InitPlayer(int id, bool team)
        {
            Console.Write("Enter player name: ");
            string name = Console.ReadLine();
            return new Character(name, team);
        }

        public Map InitWorld()
        {
            Random randWeapon = new Random();
            GameObjects.AddRange(Enumerable.Range(10, 7).Select(x => new Bot($"Friend {x}", true)));
            GameObjects.AddRange(Enumerable.Range(2, 8).Select(x => new Bot($"Enemy {x}", false)));
            GameObjects.AddRange(Enumerable.Range(0, 15).Select(x => new Heart()));
            GameObjects.AddRange(Enumerable.Range(0, 9).Select(x => randWeapon.Next(0, 2) == 0
                        ? new Knife() as GameObject
                        : new Sword() as GameObject));
            Map world = new Map(MapSize1, MapSize2, Season.None);

            world.GenerateMap();
            world.InitGameObject(Character1, 1, 1);
            world.InitGameObject(Character2, MapSize1 - 2, MapSize2 - 2);
            world.SetGameObjects(GameObjects.ToArray());
            return world;
        }
        public async Task Load()
        {
            BaseGame restore = SaveGame.Load<BaseGame>("save.json");
            await Start(restore);
        }

        public async Task Start(BaseGame restore = null)
        {
            int v = 0;
            if (restore != null)
            {
                World = restore.World;
                Character1 = restore.Character1;
                Character1.World = restore.World;
                Character2 = restore.Character2;
                Character2.World = restore.World;
            }
            else
            {
                Character1 = InitPlayer(0, true);
                Character2 = InitPlayer(1, false);
                World = InitWorld();
            }
            while (Character1.Alive && !World.HasWinner())
            {
                try
                {
                    Turn++;
                    World.Show(Character1, Character2, Turn, true);
                    Character1.Moving();
                    Console.WriteLine("Tap <Enter> to finish move...");
                    Console.ReadLine();
                    World.Show(Character1, Character2, Turn, false);
                    Task asyncSavePoint1 = SaveGame.AutoSaveAsync(this);
                    Console.WriteLine("Game autosave kicked off");
                    Character2.Moving();
                    Console.WriteLine("Tap <Enter> to finish move...");
                    Console.ReadLine();
                    World.Show(Character1, Character2, Turn);
                    await asyncSavePoint1;
                    Task asyncSavePoint2 = SaveGame.AutoSaveAsync(this);
                    foreach (Bot item in GameObjects.Where(x => x is Bot b && b.Alive))
                    {
                        item.Move("");
                    }
                    Console.ReadKey();
                    if (Turn == 2)
                    {
                        var a = 5 / v;
                        throw new GameException("Buy this game to continue");
                    }
                    await asyncSavePoint2;


                }
                catch (GameException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (DivideByZeroException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.ReadLine();
                    Console.WriteLine("End turn.");
                }
            }

            World.Show(Character1, Character2, Turn);
            Console.WriteLine($"Congrats {World.GetWinner().Name}!");
        }

    }
}
