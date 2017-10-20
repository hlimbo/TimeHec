using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
	private Transform transform;
	private SpriteRenderer sr;
	private PlayerScript player;
	private const float moveRate = 0.125f;
	private const float decayRate = 0.75f;
	private const float fireChancePerFrame = 0.015625f;
	private const float bulletSpeedMod = 0.09375f;

	public bool isDying;
	public GameObject bulletPrefab;

	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform> ();
		sr = GetComponent<SpriteRenderer> ();
		player = (PlayerScript)FindObjectOfType<PlayerScript> ();
		isDying = false;
	}
	
	// Update is called once per frame
	void Update () {
		// enemies drift towards bottom of screen
		transform.position += new Vector3 (0.0f, -moveRate * player.dilationMod, 0.0f);
		if (isDying) {
			if (sr.color.a >= 0.00390625f) {
				sr.color *= decayRate; //DECAY RATE
			} else {
				++player.killCount;
				Destroy (this);
			}
		} else if (Random.value <= fireChancePerFrame) {
		// occasionally shoot a musket ball towards player's location
			GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
			BulletScript bs = bullet.GetComponent<BulletScript> ();
			bs.direction = bulletSpeedMod * Vector3.Normalize(player.transform.position-transform.position);
		}
	}
}
