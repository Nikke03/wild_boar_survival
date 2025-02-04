using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting: MonoBehaviour {

	[SerializeField] private float Tempo;
public GameObject bullet; 
public Transform bulletPos;
private float timer;
private GameObject player;
AudioManager audioManager;


// Update is called once per frame
void Start() {
	player= GameObject.FindGameObjectWithTag("player");
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    void Update()
{

float distance = Vector2.Distance(transform.position, player.transform.position);

//Debug.Log(distance);

if (distance < 11) {
	timer += Time.deltaTime;

if(timer > Tempo)
{
timer = 0;
shoot();
}
}
}

void shoot() {
	Instantiate(bullet, bulletPos.position,Quaternion.identity);
        audioManager.PlaySFX(audioManager.attaccoGabbiano);

    }

}