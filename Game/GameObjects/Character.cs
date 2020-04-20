﻿using System;

namespace Game.GameObjects
{
    [Serializable]
    public class Character : GamePerson
    {
        public Character(string name, bool team) : base(name, team)
        {
        }

        public override Position Move(string direction)
        {
            World.Refresh();
            Position currentPos = World.GetPersonPosition(this);
            Cell currentCell = World.GetCell(currentPos);

            if (currentPos == null)
            {
                Console.WriteLine("Can't find {0}", Name);
                return null;
            }

            switch (direction)
            {
                case "w":
                    if (currentPos.Pos1 >= 1)
                        currentPos.Pos1--;
                    break;
                case "s":
                    if (currentPos.Pos1 <= World.WorldHeight - 2)
                        currentPos.Pos1++;
                    break;
                case "d":
                    if (currentPos.Pos2 <= World.WorldWidth - 2)
                        currentPos.Pos2++;
                    break;
                case "a":
                    if (currentPos.Pos2 >= 1)
                        currentPos.Pos2--;
                    break;
                default:
                    break;
            }
            Cell wantedCell = World.GetCell(currentPos);

            if (wantedCell.IsEmpty())
            {
                currentCell.GameObject = null;
                wantedCell.GameObject = this;
            }
            else
            {
                wantedCell.GameObject.Interaction(this);
                World.Refresh();
            }
            return currentPos;
        }
    }
}
