using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public enum ExplosionType { DEFAULT, FIREBALL, FREEZE }
    public ExplosionType type;
    public float damage;
    
    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth hp = other.GetComponent<EnemyHealth>();
        if (hp != null)
        {
            hp.TakeHit(damage);
            switch (type)
            {
                case ExplosionType.FIREBALL:
                    hp.Burn();
                    break;
                case ExplosionType.FREEZE:
                    hp.Freeze();
                    break;
            }
        }
    }
}
