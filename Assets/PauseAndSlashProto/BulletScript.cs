using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
	private Transform transform;
	private PlayerScript player;
	public Vector3 direction;

	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform> ();
		player = (PlayerScript)FindObjectOfType<PlayerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += direction * player.dilationMod;
	}
}
