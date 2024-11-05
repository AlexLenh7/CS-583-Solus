using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainchunks;
    public GameObject player;
    public float checkerRadius;
    Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    public GameObject currentChunk;
    PlayerMovement pm;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject lastestChunks;
    public float maxOpDist; //greater than l w of tilemap
    float opDist;
    float optimizerCooldown;
    public float optimizerCooldownDuration;

    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    void ChunkChecker()
    {
        if(!currentChunk)
        {
            return;
        }
        // right
        if(pm.moveDirection.x > 0 && pm.moveDirection.y == 0)
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right").position;
                SpawnChunk();
            }
        }
        // left
        else if(pm.moveDirection.x < 0 && pm.moveDirection.y == 0)
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left").position;
                SpawnChunk();
            }
        }
        // up
        else if(pm.moveDirection.x == 0 && pm.moveDirection.y > 0)
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Up").position;
                SpawnChunk();
            }
        }
        // down
        else if(pm.moveDirection.x == 0 && pm.moveDirection.y < 0)
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Down").position;
                SpawnChunk();
            }
        }
        // right up
        else if(pm.moveDirection.x > 0 && pm.moveDirection.y > 0)
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right Up").position;
                SpawnChunk();
            }
        }
        // right down
        else if(pm.moveDirection.x > 0 && pm.moveDirection.y < 0)
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right Down").position;
                SpawnChunk();
            }
        }
        // left up
        else if(pm.moveDirection.x < 0 && pm.moveDirection.y > 0)
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left Up").position;
                SpawnChunk();
            }
        }
        // left down
        else if(pm.moveDirection.x < 0 && pm.moveDirection.y < 0)
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left Down").position;
                SpawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        int rand = Random.Range(0, terrainchunks.Count);
        lastestChunks = Instantiate(terrainchunks[rand], noTerrainPosition, Quaternion.identity);
        spawnedChunks.Add(lastestChunks);
    }

    void ChunkOptimizer()
    {
        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            bool shouldBeActive = opDist <= maxOpDist;
            
            if (chunk.activeSelf != shouldBeActive)
            {
                // Add a small delay to prevent rapid toggling
                StartCoroutine(ToggleChunkWithDelay(chunk, shouldBeActive));
            }
        }
    }

    IEnumerator ToggleChunkWithDelay(GameObject chunk, bool active)
    {
        yield return new WaitForSeconds(0.1f);
        if (chunk != null)  // Safety check in case chunk was destroyed
        {
            chunk.SetActive(active);
        }
    }
}
