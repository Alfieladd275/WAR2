using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    [Header("Interactions")]
    public KeyCode interactKey = KeyCode.E;
    public float interactDistance = 2.5f;
    public GameObject interactUI;
    private Transform playerTransform;

    [Header("Chest Settings")]
    public Animator animator;
    public string openTriggerName = "Open";
    public float destroyDelay = 3f;

    [Header("Loot Settings")]
    public List<GameObject> lootPrefabs;
    public int lootAmount = 3;
    public Transform spawnPoint;
    public float spawnForce = 5f;

    private bool isOpened = false;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
        if (interactUI != null) interactUI.SetActive(false);
    }

    void Update()
    {
        // Safety check to prevent MissingReferenceException
        if (isOpened || playerTransform == null || (interactUI == null && !isOpened)) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance <= interactDistance)
        {
            if (interactUI != null) interactUI.SetActive(true);
            if (Input.GetKeyDown(interactKey)) OpenChest();
        }
        else
        {
            if (interactUI != null) interactUI.SetActive(false);
        }
    }

    void OpenChest()
    {
        isOpened = true;
        if (interactUI != null) interactUI.SetActive(false);
        if (animator != null) animator.SetTrigger(openTriggerName); // Ensure trigger is 'Open'

        SpawnLoot();
        Destroy(gameObject, destroyDelay);
    }

    void SpawnLoot()
    {
        if (lootPrefabs == null || lootPrefabs.Count == 0) return;

        for (int i = 0; i < lootAmount; i++)
        {
            GameObject prefabToSpawn = lootPrefabs[Random.Range(0, lootPrefabs.Count)];

            if (prefabToSpawn == null) continue;

            Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;
            spawnPos += new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.1f, 0.1f), 0);

            GameObject loot = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
            loot.transform.parent = null; // Unparent so loot stays after chest is gone

            Rigidbody2D rb = loot.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(new Vector2(Random.Range(-4f, 4f), Random.Range(6f, 8f)), ForceMode2D.Impulse);
            }
        }
    }
}