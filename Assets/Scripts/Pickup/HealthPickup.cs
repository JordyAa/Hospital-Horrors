using UnityEngine;

public class HealthPickup : MonoBehaviour, IPickup
{
    [SerializeField] private int amount = 5;
    
    public void Consume()
    {
        int hp = Random.Range(amount, amount + 5);
        PlayerManager.stats.vitals.currentHealth = Mathf.Min(
            PlayerManager.stats.vitals.currentHealth + hp,
            PlayerManager.stats.vitals.maxHealth);
        PlayerManager.instance.UpdateHealthUI();
        
        Destroy(gameObject);
    }
}
