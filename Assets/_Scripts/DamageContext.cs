using UnityEngine;

public class DamageContext
{
    public GameObject DamageOwner { get; }
    public int Damage { get; }

    public DamageContext(GameObject owner,int damage)
    {
        this.Damage = damage;
        DamageOwner = owner;
    }
}
