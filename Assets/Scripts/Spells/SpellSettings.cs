using UnityEngine;

[CreateAssetMenu]
public class SpellSettings : ScriptableObject
{
    public int minDamage;
    public int maxDamage;

    public int manaCost;
    public float cooldown;

    public float lifeTime;
    public float projectileForce;
    public GameObject projectile;
}
