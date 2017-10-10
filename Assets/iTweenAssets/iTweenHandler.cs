using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iTweenHandler : MonoBehaviour {
	//Vector3[] path = new Vector3[] { new Vector3 (0, 0, 0), new Vector3 (2, 2, 0), new Vector3(4, 0, 0), new Vector3(4, 3, 0)};
	List<Vector3> pathList = new List<Vector3>();
	bool placePointsNow = false;


	// Use this for initialization
	void Start () {
		pathList.Add (gameObject.transform.position); //the first point in the path is the position the player is at
		//pathList.Add (new Vector3 (0, 0, 0));
		//pathList.Add (new Vector3 (2, 2, 0));
		//pathList.Add (new Vector3(4, 0, 0));
		//pathList.Add (new Vector3 (4, 3, 0));
		//pathList.Add (new Vector3 (-1, -1, 0));
	}



	void StageMovement() {
		Debug.Log ("placing point");
		Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		currentMousePosition.z = 0;
		GameObject pathMarker = Instantiate (GameObject.Find ("black_square"));
		pathMarker.tag = "cloned_black_squares";
		pathMarker.transform.position = currentMousePosition;
		pathList.Add (currentMousePosition);

	}
		
	void LaunchMovement() {
		iTween.MoveTo (gameObject, iTween.Hash ("path", pathList.ToArray(), "speed", 20, "easetype", "easeinoutcubic", "lookahead", .7)); //see the iTween Unity docs to understand this
	}

	void Clear() {
		GameObject[] clones = GameObject.FindGameObjectsWithTag("cloned_black_squares");
		for (int i = 0; i < clones.Length; ++i) {
			Destroy (clones [i]);
		}
		pathList.Clear ();
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			placePointsNow = true;
			GameObject.Find ("black_square").SetActive(true);
		}
		if (placePointsNow) {
			InvokeRepeating ("StageMovement", 0f, 0.2f); //start placing markers
			placePointsNow = false;
		}
		if (Input.GetMouseButtonUp (0)) {
			LaunchMovement ();
			CancelInvoke ("StageMovement"); //stop placing markers
		}

		if (Input.GetKeyDown("space")) {
			Clear ();
		}
	}
}
