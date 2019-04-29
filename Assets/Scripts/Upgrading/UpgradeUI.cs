using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UpgradeSettings upgrade;
    
    public GameObject tooltip;
    public Text upgradeName;
    public Text description;
    public Text soulsCost;
    public Text healthCost;
    public Text manaCost;
    
    private void Start()
    {
        GetComponent<Image>().sprite = upgrade.icon;

        upgradeName.text = upgrade.upgradeName;
        description.text = "";
        foreach (string s in upgrade.description)
        {
            description.text += s + "\n";
        }
        soulsCost.text = "Souls: -" + upgrade.soulsCost;
        healthCost.text = "HP: -" + upgrade.healthCost;
        manaCost.text = "Mana: -" + upgrade.manaCost;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }
}
