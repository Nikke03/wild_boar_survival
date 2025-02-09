using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float V = 2f;
    [SerializeField] private Transform player;

    private void Update()
    {
       
        transform.position = new Vector3(player.position.x, player.position.y + V, transform.position.z);
    }
}
