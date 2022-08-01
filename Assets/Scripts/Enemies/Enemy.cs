using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [field: SerializeField] public int Health { get; protected set; }

    public abstract void TakeDamage();
}