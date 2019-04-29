using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject chooseUpgradePanel;
    public GameObject stageCompletePanel;

    [HideInInspector] public bool isPaused = true;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialise the player manager to reset UI and find the player.
        PlayerManager.instance.Init();
        isPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
    }

    public void Continue()
    {
        pausePanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void NextLevel()
    {
        PlayerManager.stats.vitals.startHealth = PlayerManager.stats.vitals.currentHealth;
        PlayerManager.stats.powers.currentLightning = PlayerManager.stats.powers.startLightning;
        PlayerManager.stats.powers.currentFireball = PlayerManager.stats.powers.startFireball;
        PlayerManager.stats.powers.currentFreeze = PlayerManager.stats.powers.startFreeze;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        Destroy(PlayerManager.instance);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
