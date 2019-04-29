using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public Text randomUpgradeText;
    
    public UpgradeSettings bash;
    public UpgradeSettings melee;
    public UpgradeSettings shield;
    
    public UpgradeSettings lightning;
    public UpgradeSettings fireball;
    public UpgradeSettings freeze;
    
    public UpgradeSettings killHealth;
    public UpgradeSettings killMana;
    
    public UpgradeSettings plus2Bullets;
    public UpgradeSettings plus4Bullets;
    
    public UpgradeSettings plusBurn;
    public UpgradeSettings plusExplosive;
    public UpgradeSettings plusFreeze;
    
    public UpgradeSettings plusHealth;
    public UpgradeSettings plusMana;
    
    public UpgradeSettings revive;

    public void EnableBash()
    {
        if (CanAfford(bash))
        {
            Purchase(bash);
            PlayerManager.stats.abilities.dash = true;
            UpdateUI();
        }
    }

    public void EnableMelee()
    {
        if (CanAfford(bash))
        {
            Purchase(bash);
            PlayerManager.stats.abilities.melee = true;
            UpdateUI();
        }
    }

    public void EnableShield()
    {
        if (CanAfford(bash))
        {
            Purchase(bash);
            PlayerManager.stats.abilities.shield = true;
            UpdateUI();
        }
    }

    public void AddLightning(int amount)
    {
        if (CanAfford(lightning))
        {
            Purchase(lightning);
            PlayerManager.stats.powers.lightning += amount;
            UpdateUI();
        }
    }

    public void AddFireball(int amount)
    {
        if (CanAfford(fireball))
        {
            Purchase(fireball);
            PlayerManager.stats.powers.fireball += amount;
            UpdateUI();
        }
    }

    public void AddFreeze(int amount)
    {
        if (CanAfford(freeze))
        {
            Purchase(freeze);
            PlayerManager.stats.powers.freeze += amount;
            UpdateUI();
        }
    }

    public void KillHealth(float chance)
    {
        if (CanAfford(killHealth))
        {
            Purchase(killHealth);
            PlayerManager.stats.dropChance.health += chance;
            UpdateUI();
        }
    }

    public void KillMana(float chance)
    {
        if (CanAfford(killMana))
        {
            Purchase(killMana);
            PlayerManager.stats.dropChance.mana += chance;
            UpdateUI();
        }
    }
    
    public void Plus2Bullets()
    {
        if (CanAfford(plus2Bullets))
        {
            Purchase(plus2Bullets);
            PlayerManager.stats.weapon.bullets += 2;
            UpdateUI();
        }
    }
    
    public void Plus4Bullets()
    {
        if (CanAfford(plus4Bullets))
        {
            Purchase(plus4Bullets);
            PlayerManager.stats.weapon.bullets += 4;
            UpdateUI();
        }
    }

    public void PlusBurn(float chance)
    {
        if (CanAfford(plusBurn))
        {
            Purchase(plusBurn);
            PlayerManager.stats.elementalChance.burn += chance;
            UpdateUI();
        }
    }
    
    public void PlusExplosive(float chance)
    {
        if (CanAfford(plusExplosive))
        {
            Purchase(plusExplosive);
            PlayerManager.stats.elementalChance.explosive += chance;
            UpdateUI();
        }
    }
    
    public void PlusFreeze(float chance)
    {
        if (CanAfford(plusFreeze))
        {
            Purchase(plusFreeze);
            PlayerManager.stats.elementalChance.freeze += chance;
            UpdateUI();
        }
    }
    
    public void PlusHealth(int amount)
    {
        if (CanAfford(plusHealth))
        {
            Purchase(plusHealth);
            PlayerManager.stats.vitals.maxHealth += amount;
            PlayerManager.stats.vitals.currentHealth += amount;
            UpdateUI();
        }
    }
    
    public void PlusMana(int amount)
    {
        if (CanAfford(plusMana))
        {
            Purchase(plusMana);
            PlayerManager.stats.vitals.maxMana += amount;
            PlayerManager.stats.vitals.currentMana += amount;
            UpdateUI();
        }
    }

    public void AddRevive(int amount)
    {
        if (CanAfford(revive))
        {
            Purchase(revive);
            PlayerManager.stats.powers.revive += amount;
            UpdateUI();
        }
    }

    public void RandomStatBoost()
    {
        int rand = Random.Range(0, 11);
        switch (rand)
        {
            case 0:
                PlayerManager.stats.vitals.currentHealth += 5;
                ShowRandomStatBoost("+5 health");
                break;
            case 2:
                PlayerManager.stats.vitals.manaRechargeRate += .05f;
                ShowRandomStatBoost("+5% mana recharge rate");
                break;
            case 3:
                PlayerManager.stats.weapon.bulletSpread -= 10f;
                ShowRandomStatBoost("-10 degrees bullet spread");
                break;
            case 4:
                PlayerManager.stats.weapon.manaCostModifier -= .05f;
                ShowRandomStatBoost("-5% mana cost");
                break;
            case 5:
                PlayerManager.stats.weapon.cooldownModifier -= .05f;
                ShowRandomStatBoost("-5% cooldown");
                break;
            case 6:
                PlayerManager.stats.weapon.minDamageModifier += .05f;
                ShowRandomStatBoost("+5% min damage");
                break;
            case 7:
                PlayerManager.stats.weapon.maxDamageModifier += .05f;
                ShowRandomStatBoost("+5% max damage");
                break;
            case 8:
                PlayerManager.stats.weapon.projectileForceModifier += .05f;
                ShowRandomStatBoost("+5% projectile speed");
                break;
            default:
                ShowRandomStatBoost("Nothing");
                break;
        }
        
        UpdateUI();
    }

    private void ShowRandomStatBoost(string s)
    {
        randomUpgradeText.text = "You gained: " + s + "!";
        randomUpgradeText.gameObject.SetActive(true);
        StartCoroutine("Disable");
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSeconds(2);
        randomUpgradeText.gameObject.SetActive(false);
    }

    private static bool CanAfford(UpgradeSettings upgrade)
    {
        return PlayerManager.stats.vitals.maxHealth >= upgrade.healthCost &&
               PlayerManager.stats.vitals.maxMana >= upgrade.manaCost &&
               PlayerManager.stats.vitals.souls >= upgrade.soulsCost;
    }

    private static void Purchase(UpgradeSettings upgrade)
    {
         PlayerManager.stats.vitals.maxHealth -= upgrade.healthCost;
         PlayerManager.stats.vitals.maxMana -= upgrade.manaCost;
         PlayerManager.stats.vitals.souls -= upgrade.soulsCost;
    }

    private static void UpdateUI()
    {
        GameManager.instance.chooseUpgradePanel.SetActive(false);
        GameManager.instance.stageCompletePanel.SetActive(true);
    }
}
