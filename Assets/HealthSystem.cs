using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;

    public GameObject deathEffect;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (deathEffect)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        // Disable the object visually
        if (TryGetComponent<Renderer>(out Renderer rend))
            rend.enabled = false;

        Debug.Log($"{gameObject.name} has died.");
    }
}
