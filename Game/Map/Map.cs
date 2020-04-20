﻿using System;

using Game.GameObjects;
using Game.Weapons;

namespace Game
{
    [Serializable]
    public enum Season
    {
        None,
        Winter,
        Spring,
        Summer,
        Autumn
    }

    [Serializable]
    public class Map
    {
        public int WorldHeight { get; set; }
        public int WorldWidth { get; set; }
        public Cell[,] Cells { get; set; }
        public Season Season { get; set; }
        public Map()
        {
        }
        public Map(int height, int width, Season season)
        {
            Cells = new Cell[height, width];
            WorldHeight = height;
            WorldWidth = width;
            Season = season;
        }

        public void GenerateMap()
        {
            for (int i = 0; i < WorldHeight; i++)
            {
                for (int k = 0; k < WorldWidth; k++)
                {
                    Cells[i, k] = new Cell();
                }
            }
        }

        public bool InitGameObject(GameObject gameObject, int position1, int position2)
        {
            return InitGameObject(gameObject, new Position(position1, position2));
        }
        public bool InitGameObject(GameObject gameObject, Position position)
        {
            if (position.Pos1 >= 0 && position.Pos2 >= 0 &&
                position.Pos1 < WorldHeight && position.Pos2 < WorldWidth)
            {
                Cell wantedCell = Cells[position.Pos1, position.Pos2];
                if (wantedCell.GameObject == null)
                {
                    wantedCell.GameObject = gameObject;
                    if (gameObject is GamePerson person)
                        person.World = this;
                    return true;
                }
            }
            return false;
        }
        public void SetGameObjects(GameObject[] objects)
        {
            foreach (var item in objects)
            {
                int pos1, pos2;
                do
                {
                    Random rand1 = new Random();
                    Random rand2 = new Random();
                    pos1 = rand1.Next(0, WorldHeight);
                    pos2 = rand1.Next(0, WorldWidth);
                } while (!InitGameObject(item, pos1, pos2));

            }
        }
        public Position GetPersonPosition(GamePerson person)
        {
            for (int i = 0; i < WorldHeight; i++)
            {
                for (int k = 0; k < WorldWidth; k++)
                {
                    if (Cells[i, k].GameObject is GamePerson origin &&
                        origin.Name.Equals(person.Name))
                        return new Position(i, k);
                }
            }
            return null;
        }

        public bool IsEmpty(Position position)
        {
            return Cells[position.Pos1, position.Pos2].IsEmpty();
        }

        public Cell GetCell(Position position)
        {
            return Cells[position.Pos1, position.Pos2];
        }

        public void Show(GamePerson player1, GamePerson player2, int turn, bool? phase = null)
        {
            Console.Clear();
            player1.ShowInfo(phase);
            player2.ShowInfo(phase);
            Refresh();
            Console.WriteLine();
            for (int i = 0; i < WorldHeight; i++)
            {
                for (int k = 0; k < WorldWidth; k++)
                {
                    Extensions.ToConsoleWrite("|", ConsoleColor.DarkGreen, Season);
                    if (Cells[i, k].GameObject != null)
                    {
                        if (Cells[i, k].GameObject is Character character)
                            Extensions.ToConsoleWrite($"☺{(character.Weapon == null ? "_" : "\u2694")}", 
                                character.PlayerFriend ? ConsoleColor.Yellow : ConsoleColor.Red, Season);
                        else if (Cells[i, k].GameObject is Bot enemy)
                            Extensions.ToConsoleWrite($"☻{(enemy.Weapon == null ? "_" : "\u2694")}",
                                enemy.PlayerFriend ? ConsoleColor.Yellow : ConsoleColor.Red, Season);
                        else if (Cells[i, k].GameObject is Heart)
                            Extensions.ToConsoleWrite("♥", ConsoleColor.Green, Season);
                        else if (Cells[i, k].GameObject is Sword)
                            Extensions.ToConsoleWrite("_\u2694", ConsoleColor.Cyan, Season);
                        else if (Cells[i, k].GameObject is Knife)
                            Extensions.ToConsoleWrite("_\u2694", ConsoleColor.Magenta, Season);
                    }
                    else
                        Extensions.ToConsoleWrite("__", ConsoleColor.DarkGreen, Season);
                }
                Extensions.ToConsole("|", ConsoleColor.DarkGreen, Season);
            }
            Console.WriteLine();
            Console.WriteLine($"Turn: {turn}");
        }

        public void Refresh()
        {
            for (int i = 0; i < WorldHeight; i++)
            {
                for (int k = 0; k < WorldWidth; k++)
                {
                    if (Cells[i, k].GameObject != null)
                    {
                        if (Cells[i, k].GameObject is Heart heart && heart.Used ||
                            Cells[i, k].GameObject is GamePerson person && !person.Alive ||
                            Cells[i, k].GameObject is CommonWeapon weapon && weapon.Taken)
                            Cells[i, k].GameObject = null;

                    }
                }
            }
        }

        public bool HasWinner()
        {
            int count = 0;
            for (int i = 0; i < WorldHeight; i++)
            {
                for (int k = 0; k < WorldWidth; k++)
                {
                    if (Cells[i, k].GameObject != null && Cells[i, k].GameObject is Character)
                        count++;
                }
            }
            return count == 1;
        }

        public Character GetWinner()
        {
            if (HasWinner())
            {
                foreach (var cell in Cells)
                {
                    if (cell.GameObject != null && cell.GameObject is Character winner)
                        return winner;
                }
            }
            return null;
        }
    }
}
