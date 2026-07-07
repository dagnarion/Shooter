using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour,IDamageable
{
    [SerializeField] private int maxHealh;
    private HealthSystem health;

    private void Awake()
    {
        health = new HealthSystem(maxHealh);
    }

    public void TakeDamage(DamageContext damageContext)
    {
        health.Detuc(damageContext.Damage);
    }
}
