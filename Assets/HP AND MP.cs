using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public int maxMana = 100;
    public int currentMana;

    void Start()
    {
        currentHP = maxHP;
        currentMana = maxMana;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            // Trigger death logic
        }
    }

    public bool UseMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            return true;
        }
        return false;
    }
}

