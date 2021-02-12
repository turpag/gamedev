using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayer : MonoBehaviour {
	
	[SerializeField]
	private float rotateSpeed;
	[SerializeField]
	private Animator anim;

	// Update is called once per frame
	void Update () {
		Vector2 direction = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		float dXY = (Mathf.Abs (direction.x) + Mathf.Abs (direction.y));
		float dX = direction.x / dXY;
		float dY = direction.y / dXY;
		anim.SetFloat ("dirX", dX);
		anim.SetFloat ("dirY", dY);
	}
}
