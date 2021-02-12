using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField]
	float speed = 5f;
	[SerializeField]
	Rigidbody2D rb;
	[SerializeField]
	Animator anim;





	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame

	void FixedUpdate () {
		if (Input.GetMouseButton (0)) {
			anim.SetBool ("walking", true);
			var targetPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			targetPos.z = transform.position.z;
			transform.position = Vector3.MoveTowards (transform.position, targetPos, speed * Time.deltaTime);
		} else {
			anim.SetBool ("walking", false);
		}
	}
}
