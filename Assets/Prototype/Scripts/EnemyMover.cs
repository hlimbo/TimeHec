using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMover : MonoBehaviour {

    public float moveSpeed;
    public float patrolTime;
    [SerializeField]
    private float patrolDist = 0;
    private int yDirection;
    private Rigidbody2D rb;
    private Vector2 velocity;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        yDirection = -1;
        velocity = new Vector2(0.0f, 0.0f);
        StartCoroutine(ChangeDirection());
	}
	
	// Update is called once per frame
	void Update ()
    {
        velocity.Set(0.0f, moveSpeed * yDirection);
        rb.velocity = velocity * Time.deltaTime;
	}

    IEnumerator ChangeDirection()
    {
        while(true)
        {
            patrolDist = patrolTime * moveSpeed * Time.deltaTime;
            yDirection = (yDirection == 1) ? -1 : 1;
            yield return new WaitForSeconds(patrolTime);
        }
    }
}
