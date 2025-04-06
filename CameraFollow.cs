using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Obiekt gracza
    public float followSpeed = 5f;
    public float stairOffset = 0.5f; // Ile kamera przesunie si� w pionie
    private PlayerController playerController;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>(); // Pobieramy PlayerController
    }

    void Update()
    {
        Vector3 targetPosition = player.position;

        // Sprawdzamy, czy gracz faktycznie porusza si� po schodach
        if (playerController.IsOnStairs && playerController.GetInput().x != 0)
        {
            targetPosition.y -= Mathf.Sign(playerController.GetInput().x) * stairOffset;
        }

        // P�ynne pod��anie kamery za graczem
        transform.position = Vector3.Lerp(transform.position,
            new Vector3(targetPosition.x, targetPosition.y, transform.position.z),
            followSpeed * Time.deltaTime);
    }
}
