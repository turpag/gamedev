using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float jumpTimer = 0;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        jumpTimer += Time.deltaTime;
        if (jumpTimer > 1 && Input.GetKeyDown(KeyCode.Space)) {
            jumpTimer = 0;
            animator.SetTrigger("Jump");
            rb.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.A)) {
            animator.SetBool("isMoving", true);
            transform.position = (Vector2)transform.position + new Vector2(-5, 0) * Time.deltaTime;
            sr.flipX = true;
        } else if (Input.GetKey(KeyCode.D)) {
            animator.SetBool("isMoving", true);
            transform.position = (Vector2)transform.position + new Vector2(5, 0) * Time.deltaTime;
            sr.flipX = false;
        } else {
            animator.SetBool("isMoving", false);
        }
    }
}
