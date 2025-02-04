using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingAlveare1 : MonoBehaviour {

	[SerializeField] private int Tempo;
public GameObject bullet; 
public Transform bulletPos;
private float timer;
private GameObject player;
	private Animator anim;
	// Update is called once per frame
	void Start() {
	player= GameObject.FindGameObjectWithTag("player");
}

	private void Awake()
	{
		anim = GetComponent<Animator>();
		
	}

	void Update()
{

float distance = Vector2.Distance(transform.position, player.transform.position);

//Debug.Log(distance);

if (distance < 8) {
	timer += Time.deltaTime;

if(timer > Tempo)
{
timer = 0;

shoot();
}
}
}

void shoot() {
		anim.SetTrigger("Shoot");
		Instantiate(bullet, bulletPos.position,Quaternion.identity);
}

}