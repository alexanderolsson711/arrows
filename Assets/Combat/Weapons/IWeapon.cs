using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arrows.Combat.Weapons
{
    public interface IWeapon
    {
        void Attack(Vector3 attackPoint);
    }
}