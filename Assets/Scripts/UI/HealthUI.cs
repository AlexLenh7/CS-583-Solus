using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("References")]
    public PlayerStats playerStats;
    public GameObject heartPrefab;
    public Transform heartsParent;

    [Header("Settings")]
    public int healthPerHeart = 20;
    public int maxHearts = 10;
    
    private Image[] heartImages;

    void Awake()
    {
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats reference is missing in HealthUI.");
            return;
        }

        InitializeHearts();
    }


    void Start()
    {
        UpdateHeartDisplay();
    }

    void Update()
    {
        UpdateHeartDisplay();
    }

    void InitializeHearts()
    {
        // Calculate initial number of hearts based on max health
        int initialHearts = Mathf.Min(playerStats.maxHealth / healthPerHeart, maxHearts);
        heartImages = new Image[maxHearts];

        // Create heart objects
        for (int i = 0; i < maxHearts; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartsParent);
            heartImages[i] = newHeart.GetComponent<Image>();
            
            // Initially disable hearts beyond the player's current maximum
            heartImages[i].gameObject.SetActive(i < initialHearts);
        }
    }

    void UpdateHeartDisplay()
    {
        // Calculate how many hearts should be shown based on current health
        int heartsToShow = Mathf.CeilToInt(playerStats.currentHealth / (float)healthPerHeart);
        
        // Ensure we don't exceed the maximum number of hearts
        heartsToShow = Mathf.Min(heartsToShow, maxHearts);

        // Update heart visibility
        for (int i = 0; i < maxHearts; i++)
        {
            if (heartImages[i] != null) // Null-check to avoid errors
            {
                heartImages[i].gameObject.SetActive(i < heartsToShow);
            }
        }
    }

    // Call this method whenever the player's health changes
    public void OnHealthChanged()
    {
        if (heartImages == null || heartImages.Length == 0)
        {
            Debug.LogWarning("HealthUI hearts are not initialized yet.");
            return;
        }
        UpdateHeartDisplay();
    }
}

