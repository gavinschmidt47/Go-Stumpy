using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -5);
    public Vector2 leftBottom = new Vector2(-4, -1);
    public Vector2 rightTop = new Vector2(10, 10);

    private Vector3 targetPosition;
    private Vector3 currPosition;

    // Update is called once per frame
    void LateUpdate()
    {
        targetPosition = player.position + offset;
        currPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        currPosition.x = Mathf.Clamp(currPosition.x, leftBottom.x, rightTop.x);
        currPosition.y = Mathf.Clamp(currPosition.y, leftBottom.y, rightTop.y);
        transform.position = currPosition;
    }
}
