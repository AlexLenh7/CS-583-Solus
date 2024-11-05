using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerupType
    {
        SpeedBoost,
        DamageBoost,
        HealthBoost
    }

    public PowerupType type;
    public float speedIncrease = 0.1f;
    public int damageIncrease = 10;
    public int maxHealthIncrease = 20;
    [SerializeField] private AudioClip PowerUpSoundClip;

    private void Start()
    {
        // Add a collider if it doesn't exist
        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"Trigger entered by {other.gameObject.name}");
        
        if (other.CompareTag("Player"))
        {
            //Debug.Log($"Player collected powerup of type: {type}");
            ApplyPowerup(other.gameObject);
            SoundFXManager.instance.PlaySoundFXClip(PowerUpSoundClip, transform, .3f);
            Destroy(gameObject);
        }
    }

    private void ApplyPowerup(GameObject player)
    {
        switch (type)
        {
            case PowerupType.SpeedBoost:
                PlayerMovement movement = player.GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    if (movement.moveSpeed < 1f)
                    {
                        movement.moveSpeed += speedIncrease;
                        Debug.Log($"Speed increased by {speedIncrease}. New speed: {movement.moveSpeed}");
                    }
                }
                break;

            case PowerupType.DamageBoost:
                PlayerAttack attack = player.GetComponent<PlayerAttack>();
                if (attack != null)
                {
                    if (attack.attackDamage < 50)
                    {
                        attack.attackDamage += damageIncrease;
                        Debug.Log($"Attack damage increased by {damageIncrease}. New damage: {attack.attackDamage}");
                    }
                }
                break;

            case PowerupType.HealthBoost:
                PlayerStats stats = player.GetComponent<PlayerStats>();
                if (stats != null)
                {
                // int newMaxHealth = Mathf.Min(stats.maxHealth + maxHealthIncrease, 200);
                // int newCurrentHealth = Mathf.Min(stats.currentHealth + maxHealthIncrease, newMaxHealth);
                stats.maxHealth = Mathf.Min(stats.maxHealth + maxHealthIncrease, 200);

                // Increase current health up to the maxHealth limit
                stats.currentHealth = Mathf.Min(stats.currentHealth + maxHealthIncrease, stats.maxHealth);
                }
                break;
        }
    }
}

