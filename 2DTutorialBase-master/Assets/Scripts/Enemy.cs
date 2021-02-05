using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    #region Movement_variables
    public float movespeed;
    #endregion

    #region Physics_components
    Rigidbody2D EnemyRB;
    #endregion

    #region Targeting_variables
    public Transform player;
    #endregion

    #region Attack_variables
    public float explosionDamage;
    public float explosionRadius;
    public GameObject explosionObj;
    #endregion

    #region Health_variables
    public float maxHealth;
    float currHealth;
    #endregion

    #region Unity_functions
    private void Awake() {
        EnemyRB = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;

    }
    private void Update() {
        //check if we know where player is
        if (player == null) {
            return;
        } else {
            Move();
        }
    }
    #endregion

    #region Movement_functions

    private void Move() {
        //player position - enemy position = vector from p to e
        Vector2 direction = player.position - transform.position;

        EnemyRB.velocity = direction.normalized * movespeed;
    }
    #endregion

    #region Attack_functions
    private void Explode() {
        FindObjectOfType<AudioManager>().Play("Explosion");
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero);
        foreach (RaycastHit2D hit in hits) {
            if (hit.transform.CompareTag("Player")) {
                //Cause damage
                Debug.Log("Hit player with explosion");
                //Spawn explosion prefab
                Instantiate(explosionObj, transform.position, transform.rotation);
                hit.transform.GetComponent<PlayerController>().TakeDamage(explosionDamage);
            }
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.transform.CompareTag("Player")) {
            Explode();
        }
    }
    #endregion

    #region Health_functions
    public void TakeDamage(float value) {
        FindObjectOfType<AudioManager>().Play("BatHurt");
        currHealth -= value;
        //Debug.Log("Health is now " + currHealth.ToString());
        if (currHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        Destroy(this.gameObject);
    }
    #endregion
}
