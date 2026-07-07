using UnityEngine;

public class HealthSystem
{
    private int currentHealth;
    private int maxHealth;

    public HealthSystem(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public bool IsDead()
    {
        if(currentHealth == 0) Debug.Log("Dead");
      return  currentHealth <= 0;
    }

    public void Detuc(int amount)
    {
        if(IsDead()) return;
        currentHealth -= amount;
    }

    public void Heal(int amount)
    {
        if(IsDead()) return;
        currentHealth = Mathf.Clamp(currentHealth + amount,0,maxHealth);
    }

    public void Reborn()
    {
        currentHealth = maxHealth;
    }

}
