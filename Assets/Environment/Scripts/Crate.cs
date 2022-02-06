using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IHurtable
{
    public void Hurt(float damage)
    {
        Destroy(gameObject);
    }
}
