using UnityEngine;

[CreateAssetMenu]
public class UpgradeSettings : ScriptableObject
{
    public Sprite icon;
    
    public string upgradeName = "Name";
    public string[] description = { "Description" };
    
    public int soulsCost = 999;
    public int healthCost = 999;
    public int manaCost = 999;
}
