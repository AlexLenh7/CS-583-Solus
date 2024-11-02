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

    void Start()
    {
        InitializeHearts();
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
            heartImages[i].gameObject.SetActive(i < heartsToShow);
        }
    }

    // Call this method whenever the player's health changes
    public void OnHealthChanged()
    {
        UpdateHeartDisplay();
    }
}

