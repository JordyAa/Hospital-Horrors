using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }
    public static Stats stats { get; private set; }
    
    public GameObject hitEffect;
    public Slider healthSlider;
    public Text healthText;
    public Slider manaSlider;
    public Text manaText;
    public Text soulsText;

    public Image icon;
    public Sprite face1;
    public Sprite face2;
    public Sprite face3;
    public Sprite powerFire;
    public Sprite powerFreeze;
    public Sprite powerLightning;

    public int selectedPower;

    private CameraFollow cam;
    private GameObject player;
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            stats = new Stats();
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        player = GameObject.FindWithTag("Player");
        if (Camera.main != null)
        {
            cam = Camera.main.GetComponent<CameraFollow>();
        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            // Initialise stats if this is the first level.
            stats = new Stats();
        }
        else
        {
            stats.vitals.currentHealth = Mathf.Min(stats.vitals.currentHealth, stats.vitals.maxHealth);
            stats.vitals.currentMana = stats.vitals.maxMana;
        }
        UpdateHealthUI();
        UpdateManaUI();
    }

    private void Update()
    {
        if (GameManager.instance.isPaused) return;
        
        stats.vitals.currentMana = Mathf.Min(
            stats.vitals.currentMana + stats.vitals.manaRechargeRate * Time.deltaTime, stats.vitals.maxMana);
        UpdateManaUI();
    }

    public void Hit(int amount)
    {
        cam.Shake();
        
        stats.vitals.currentHealth = Mathf.Max(0, stats.vitals.currentHealth - amount);
        UpdateHealthUI();
        
        AudioManager.instance.PlayerHit();
        
        Instantiate(hitEffect, player.transform.position, Quaternion.identity);
        if (stats.vitals.currentHealth <= 0)
        {
            GameManager.instance.isPaused = true;
            player.GetComponent<Animator>().SetTrigger(IsDead);

            if (stats.powers.revive > 0)
            {
                stats.powers.revive--;
                stats.vitals.currentHealth = stats.vitals.startHealth;
                stats.powers.currentLightning = stats.powers.startLightning;
                stats.powers.currentFireball = stats.powers.startFireball;
                stats.powers.currentFreeze = stats.powers.startFreeze;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                GameManager.instance.gameOverPanel.SetActive(true);
            }
        }
    }

    public void UpdateIconUI()
    {
        switch (selectedPower)
        {
            case 0:
                if (stats.vitals.currentHealth > stats.vitals.maxHealth * .66f)
                {
                    icon.sprite = face1;
                }
                else if (stats.vitals.currentHealth > stats.vitals.maxHealth * .33f)
                {
                    icon.sprite = face2;
                }
                else
                {
                    icon.sprite = face3;
                }
                break;
            case 1:
                icon.sprite = powerLightning;
                break;
            case 2:
                icon.sprite = powerFire;
                break;
            case 3:
                icon.sprite = powerFreeze;
                break;
        }
    }

    public void UpdateHealthUI()
    {
        healthSlider.maxValue = stats.vitals.maxHealth;
        healthSlider.value =  stats.vitals.currentHealth;
        healthText.text = string.Format("{0:0}/{1:0}", stats.vitals.currentHealth, stats.vitals.maxHealth);
    }

    public void UpdateManaUI()
    {
        manaSlider.maxValue = stats.vitals.maxMana;
        manaSlider.value =  stats.vitals.currentMana;
        manaText.text = string.Format("{0:0}/{1:0}", stats.vitals.currentMana, stats.vitals.maxMana);
    }
}
