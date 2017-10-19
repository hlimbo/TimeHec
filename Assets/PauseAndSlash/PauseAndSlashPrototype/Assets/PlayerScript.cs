using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	private Transform transform;
	private LineRenderer line;

	private int moveIndex;
	private bool initiatedPath;

	private const float minMoveDistPerFrame = 0.015625f;
	private const float maxMoveDistPerFrame = 0.4375f;
	private const float maxInkPotDistance = 25.0f;
	private const float inkPotRegenPerFrame = 0.125f;
	//private const float maxHourglass = 5.0f;
	//private const float hourglassRegenPerKill = 0.125f;//0.015625f;
	private const float dilationRate = 0.125f;
	 
	public float inkPot;
	//public float hourglass;
	public int killCount;
	public int deathCount;

	public float dilationMod;

	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform> ();
		line = GetComponent<LineRenderer> ();
		moveIndex = -1;
		initiatedPath = false;
		inkPot = maxInkPotDistance;
		//hourglass = maxHourglass;
		killCount = 0;
		deathCount = 0;
		dilationMod = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (moveIndex > -1) {
			if (Vector3.Distance (transform.position, line.GetPosition (moveIndex)) <= maxMoveDistPerFrame) {
				transform.position = line.GetPosition (moveIndex++);
			} else {
				transform.position += maxMoveDistPerFrame * Vector3.Normalize (line.GetPosition (moveIndex) - transform.position);
			}
			if (moveIndex >= line.positionCount) {
				line.positionCount = 0;
				moveIndex = -1;
				dilationMod = 1.0f;
			}
		} else if (!initiatedPath) {
			inkPot += inkPotRegenPerFrame;
			if (inkPot > maxInkPotDistance)
				inkPot = maxInkPotDistance;
		}
	}

	void OnMouseDown () {
		if (line.positionCount == 0) {
			//initiatedPath = false;
			++line.positionCount;
			Vector3 v = Input.mousePosition;
			v.z = 10.0f;
			line.SetPosition (line.positionCount - 1, Camera.main.ScreenToWorldPoint (v));
			dilationMod = dilationRate;
		}
	}

	void OnMouseExit () {
		if (line.positionCount == 1)
			initiatedPath = true;
	}

	void OnMouseDrag () {
		//optimize later. this is just a prototype.
		if (initiatedPath) {
			Vector3 v = Input.mousePosition;
			v.z = 10.0f;
			v = Camera.main.ScreenToWorldPoint (v);

			if (inkPot < Vector3.Distance (v, line.GetPosition (line.positionCount - 1))) {
				++line.positionCount;
				line.SetPosition (line.positionCount - 1, line.GetPosition(line.positionCount-2) + ( inkPot * Vector3.Normalize(v - line.GetPosition(line.positionCount-2))));
				inkPot = 0;
				OnMouseUp ();
			} else {
				
				inkPot -= Vector3.Distance (v, line.GetPosition (line.positionCount - 1));
				++line.positionCount;
				line.SetPosition (line.positionCount - 1, v);
			}
		}
	}

	void OnMouseUp () {
		if (line.positionCount > 0) {
			moveIndex = 0;
			initiatedPath = false;
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (moveIndex > -1) {
			//send kill signal to other guy's enemyscript or just remove if it's a bullet
			EnemyScript enemy = other.gameObject.GetComponent<EnemyScript> ();
			if (enemy) {
				enemy.isDying = true;
			} else {
				Destroy (other.gameObject);
			}
		} else {
			//die
			++deathCount;
			foreach (GameObject o in GameObject.FindGameObjectsWithTag("Enemy")) {
				Destroy (o);
			}
			moveIndex = -1;
			initiatedPath = false;
			inkPot = maxInkPotDistance;
			//hourglass = maxHourglass;
		}
	}
}
