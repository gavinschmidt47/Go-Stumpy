using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCam : MonoBehaviour
{
    public float targetSize = 10.0f; // The target position for the camera
    public float duration = 2.0f; // Duration of the camera move

    void Start()
    {
        // Start the camera move coroutine
        StartCoroutine(MoveCamera());
    }

    private IEnumerator MoveCamera()
    {
        // Wait for 1 second before starting the camera move
        yield return new WaitForSeconds(1.0f);
        
        // Store the initial size of the camera
        float startSize = Camera.main.orthographicSize;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            Camera.main.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera reaches the exact target position and rotation
        Camera.main.orthographicSize = targetSize;
    }
}
