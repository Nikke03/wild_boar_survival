  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthcollectible : MonoBehaviour
{
    [SerializeField] private float healtvalue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {
            collision.GetComponent<Health>().addHealth(healtvalue);
            gameObject.SetActive(false);
        }
    }
}
