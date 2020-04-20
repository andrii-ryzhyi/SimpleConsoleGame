using System;
using System.Collections.Generic;
using System.Text;
using Game.GameObjects;

namespace Game
{
    [Serializable]
    public enum CellState
    {
        HasShip,
        HadShip,
        IsEmpty, 
        IsShooted
    }
    [Serializable]
    public class Cell
    {
        public GameObject GameObject { get; set; }

        public Cell()
        {
        }

        public Cell(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public bool IsEmpty()
        {
            return GameObject == null;
        }
    }
}
