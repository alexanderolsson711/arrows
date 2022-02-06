using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arrows.Combat.Projectiles
{
    public interface IProjectile
    {
        void Fire(Vector3 position, Vector3 direction, float damage);
    }
}