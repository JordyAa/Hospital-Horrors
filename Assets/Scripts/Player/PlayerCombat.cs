using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float projectileSpawnOffset;
    public SpellSettings equippedSpell;
    private float spellCooldownTimer;

    public GameObject shield;
    public float shieldEnabledTime;
    public float shieldCooldown;
    private float shieldCooldownTimer;

    public GameObject lightningEffect;
    public GameObject fireballEffect;
    public GameObject freezeEffect;
    
    public Stats stats;
    private Camera cam;
    private CameraFollow camFollow;
    
    private void Start()
    {
        stats = PlayerManager.stats;
        cam = Camera.main;
        if (cam != null)
        {
            camFollow = cam.GetComponent<CameraFollow>();
        }
    }

    private void Update()
    {
        if (GameManager.instance.isPaused) return;

        SelectSpell();

        if (Input.GetMouseButtonDown(1))
        {
            UseSpell();
        }
        
        if (spellCooldownTimer <= 0)
        {
            if (Input.GetMouseButtonDown(0) && PlayerManager.stats.vitals.currentMana >= equippedSpell.manaCost)
            {
                spellCooldownTimer = equippedSpell.cooldown * PlayerManager.stats.weapon.cooldownModifier;

                float manaCost = equippedSpell.manaCost * PlayerManager.stats.weapon.manaCostModifier;
                PlayerManager.stats.vitals.currentMana -= manaCost;
                PlayerManager.instance.UpdateManaUI();
                Shoot();
            }
        }
        else
        {
            spellCooldownTimer -= Time.deltaTime;
        }

        if (shieldCooldownTimer <= 0)
        {
            if (PlayerManager.stats.abilities.shield && Input.GetKeyDown(KeyCode.Q))
            {
                shieldCooldownTimer = shieldCooldown + shieldEnabledTime;
                shield.SetActive(true);
                StartCoroutine("DisableShield");
            }
        }
        else
        {
            shieldCooldownTimer -= Time.deltaTime;
        }
    }

    private static void SelectSpell()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (PlayerManager.instance.selectedPower == 1)
            {
                PlayerManager.instance.selectedPower = 0;
                PlayerManager.instance.UpdateIconUI();
            }
            else if (PlayerManager.stats.powers.currentLightning > 0)
            {
                PlayerManager.instance.selectedPower = 1;
                PlayerManager.instance.UpdateIconUI();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (PlayerManager.instance.selectedPower == 2)
            {
                PlayerManager.instance.selectedPower = 0;
                PlayerManager.instance.UpdateIconUI();
            }
            else if (PlayerManager.stats.powers.currentFireball > 0)
            {
                PlayerManager.instance.selectedPower = 2;
                PlayerManager.instance.UpdateIconUI();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (PlayerManager.instance.selectedPower == 3)
            {
                PlayerManager.instance.selectedPower = 0;
                PlayerManager.instance.UpdateIconUI();
            }
            else if (PlayerManager.stats.powers.currentFreeze > 0)
            {
                PlayerManager.instance.selectedPower = 3;
                PlayerManager.instance.UpdateIconUI();
            }
        }
    }

    private void UseSpell()
    {
        switch (PlayerManager.instance.selectedPower)
        {
            case 1:
                Debug.LogError("Not yet implemented!");
                PlayerManager.stats.powers.currentLightning--;
                camFollow.Shake();
                if (PlayerManager.stats.powers.currentLightning == 0)
                {
                    PlayerManager.instance.selectedPower = 0;
                    PlayerManager.instance.UpdateIconUI();
                }
                break;
            case 2:
                Instantiate(fireballEffect, cam.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                PlayerManager.stats.powers.currentFireball--;
                camFollow.Shake();
                if (PlayerManager.stats.powers.currentFireball == 0)
                {
                    PlayerManager.instance.selectedPower = 0;
                    PlayerManager.instance.UpdateIconUI();
                }
                break;
            case 3:
                Instantiate(freezeEffect, cam.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                PlayerManager.stats.powers.currentFreeze--;
                camFollow.Shake();
                if (PlayerManager.stats.powers.currentFreeze == 0)
                {
                    PlayerManager.instance.selectedPower = 0;
                    PlayerManager.instance.UpdateIconUI();
                }
                break;
        }
    }

    private IEnumerator DisableShield()
    {
        yield return new WaitForSeconds(shieldEnabledTime);
        shield.SetActive(false);
    }

    private void Shoot()
    {
        for (int i = 1; i < stats.weapon.bullets + 1; i++)
        {
            Vector2 position = transform.position;
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - position).normalized;
            
            float angleOffset = (float) i / (stats.weapon.bullets + 1) * stats.weapon.bulletSpread - stats.weapon.bulletSpread / 2f;
            direction = Rotate(direction, angleOffset);
            
            GameObject spell = Instantiate(
                equippedSpell.projectile,
                position + projectileSpawnOffset * direction,
                Quaternion.identity);

            spell.GetComponent<Projectile>().Initialise(equippedSpell, direction, tag, stats);
        }
    }

    private static Vector2 Rotate(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }
}
