using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour {
	private PlayerScript player;
	private Text text;

	// Use this for initialization
	void Start () {
		player = (PlayerScript)FindObjectOfType<PlayerScript> ();
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "Kills: " + player.killCount + "\nDeaths: " + player.deathCount + "\nInk Pot: " + player.inkPot;
	}
}
