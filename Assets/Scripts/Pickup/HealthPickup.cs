using UnityEngine;

public class HealthPickup : MonoBehaviour, IPickup
{
    [SerializeField] private int amount = 10;
    
    public void Consume()
    {
        PlayerManager.stats.vitals.currentHealth = Mathf.Min(
            PlayerManager.stats.vitals.currentHealth + amount,
            PlayerManager.stats.vitals.maxHealth);
        PlayerManager.instance.UpdateHealthUI();
        
        Destroy(gameObject);
    }
}
