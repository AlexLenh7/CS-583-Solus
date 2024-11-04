using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject speedPowerupPrefab;
    public GameObject damagePowerupPrefab;
    public GameObject healthPowerupPrefab;
    [Range(0f, 1f)]
    public float dropChance = 0.3f;     // 30% chance to drop

    public void TryDropPowerup(Vector3 position)
    {
        float roll = Random.value;

        Debug.Log($"Drop Roll: {roll}, Drop Chance: {dropChance}");

        if (roll > dropChance)
        {
            return;
        }

        float powerupRoll = Random.value;
        Debug.Log($"Powerup Type Roll: {powerupRoll}");

        if (powerupRoll <= .33f)
        {
            Instantiate(speedPowerupPrefab, position, Quaternion.identity);
            Debug.Log("Spawned Speed Power-Up");
        }
        else if (powerupRoll <= 0.66f)
        {
            Instantiate(damagePowerupPrefab, position, Quaternion.identity);
            Debug.Log("Spawned Damage Power-Up");
        }
        else
        {
            Instantiate(healthPowerupPrefab, position, Quaternion.identity);
            Debug.Log("Spawned Health Power-Up");
        }
    }
}