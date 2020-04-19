using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public interface IWeapon
    {
        bool Taken { get; set; }
        int Damage { get; set; }
        void SpecialAttack(GamePerson enemy);
    }
}
