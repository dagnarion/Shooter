using System;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private int damage;
    private DamageContext spikeDamage;

    private void Awake()
    {
        spikeDamage = new DamageContext(this.gameObject,damage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(spikeDamage);
        }
    }
}
