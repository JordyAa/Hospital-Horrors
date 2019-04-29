using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.isPaused = true;
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCount - 2)
            {
                // final level completed
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                GameManager.instance.chooseUpgradePanel.SetActive(true);
            }
        }
    }
}
