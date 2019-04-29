using UnityEngine;

public class Elevator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.isPaused = true;
            GameManager.instance.chooseUpgradePanel.SetActive(true);
        }
    }
}
