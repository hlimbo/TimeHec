using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShots : MonoBehaviour {
    public GameObject Shots;
    public Transform BulletSpawn;
    public float fireRate;
    private float nextFire;

	// Update is called once per frame
	void Update ()
    {
		if(Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(Shots, BulletSpawn.position, BulletSpawn.rotation);
        }
	}
}
