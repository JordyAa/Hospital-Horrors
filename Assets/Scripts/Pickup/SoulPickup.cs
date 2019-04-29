using UnityEngine;

public class SoulPickup : MonoBehaviour, IPickup
{
    [SerializeField] private int amount = 1;
    
    public void Consume()
    {
        PlayerManager.stats.vitals.souls += amount;
        PlayerManager.instance.soulsText.text = "SOULS: " + PlayerManager.stats.vitals.souls;
        
        Destroy(gameObject);
    }
}
