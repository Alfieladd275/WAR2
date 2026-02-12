using UnityEngine;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour
{
    [Header("Settings")]
    public Transform cameraPoint;
    public float stayTime = 3f;
    public float panSpeed = 2f;
    public bool triggerOnlyOnce = true;

    private bool hasTriggered = false;
    private Camera mainCam;

    // Arrays to store scripts so we can wake them up later
    private MonoBehaviour[] disabledCamScripts;
    private MonoBehaviour[] disabledPlayerScripts;

    void Start()
    {
        mainCam = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            if (triggerOnlyOnce) hasTriggered = true;
            StartCoroutine(RunFullLockdownCutscene(other.gameObject));
        }
    }

    IEnumerator RunFullLockdownCutscene(GameObject player)
    {
        // 1. DISABLE CAMERA BRAIN
        disabledCamScripts = mainCam.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in disabledCamScripts)
        {
            if (script != this) script.enabled = false;
        }

        // 2. DISABLE PLAYER BRAIN (Movement, Combat, Input, etc.)
        disabledPlayerScripts = player.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in disabledPlayerScripts)
        {
            script.enabled = false;
        }

        // 3. KILL VELOCITY (Stops the "gliding" bug)
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0;
            rb.bodyType = RigidbodyType2D.Kinematic; // Makes him a "ghost" so physics can't move him
        }

        // 4. PAN TO TARGET
        Vector3 startPos = mainCam.transform.position;
        Vector3 targetPos = new Vector3(cameraPoint.position.x, cameraPoint.position.y, -10f);

        float t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime * panSpeed;
            mainCam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        mainCam.transform.position = targetPos;

        // 5. STAY
        yield return new WaitForSeconds(stayTime);

        // 6. PAN BACK
        t = 0;
        Vector3 returnStartPos = mainCam.transform.position;
        while (t < 1.0f)
        {
            t += Time.deltaTime * panSpeed;
            Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
            mainCam.transform.position = Vector3.Lerp(returnStartPos, playerPos, t);
            yield return null;
        }

        // 7. WAKE EVERYTHING UP
        if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic; // Reset physics

        foreach (MonoBehaviour script in disabledCamScripts) script.enabled = true;
        foreach (MonoBehaviour script in disabledPlayerScripts) script.enabled = true;
    }
}