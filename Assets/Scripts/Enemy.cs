using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [field: SerializeField] public int Health { get; private set; }

    public void TakeDamage()
    {

    }
}
