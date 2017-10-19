using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	private Transform transform;
	private Transform player;
	private GameObject[] enemies;

	private const int enemySpawnCap = 7;
	private const float spawnChancePerFrame = 0.0625f; //should be between 0.0f and 1.0f

	public GameObject enemyPrefab;

	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	// Update is called once per frame
	void Update () {
		transform.position = player.position + new Vector3(0.0f,2.5f,-10.0f);

		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		if (enemies.Length < enemySpawnCap && Random.value <= spawnChancePerFrame) {
			Instantiate (enemyPrefab, new Vector3(Random.value * (14.0f) - 7.0f,
				1.0f + GetComponent<Camera> ().orthographicSize,10.0f) + transform.position, Quaternion.identity);
			//don't hardcode this shit in the final build LUL
		}

		foreach (GameObject enemy in enemies) {
			if ((transform.position.y-enemy.transform.position.y) >= (1.0f + GetComponent<Camera> ().orthographicSize)) {
				Destroy (enemy);
			}
		}
	}
}
