using UnityEngine;

public class ManaPickup : MonoBehaviour, IPickup
{
    [SerializeField] private int amount = 10;
    
    public void Consume()
    {
        PlayerManager.stats.vitals.currentMana = Mathf.Min(
            PlayerManager.stats.vitals.currentMana + amount,
            PlayerManager.stats.vitals.maxMana);
        PlayerManager.instance.UpdateManaUI();
        
        Destroy(gameObject);
    }
}
