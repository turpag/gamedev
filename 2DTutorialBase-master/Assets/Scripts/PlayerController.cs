using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Movement_variables
    public float movespeed;
    float x_input;
    float y_input;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    #endregion

    #region Attack_variables
    public float Damage;
    public float attackspeed = 1;
    float attackTimer;
    public float hitboxtiming;
    public float endanimationtiming;
    bool isAttacking;
    Vector2 currDirection;
    #endregion

    #region Animation_components
    Animator anim;
    #endregion

    #region Health_variables
    public float maxHealth;
    float currHealth;
    public Slider HPSlider;
    #endregion


    #region Unity_functions
    private void Awake() { //when created
        PlayerRB = GetComponent<Rigidbody2D>();
        attackTimer = 0;
        anim = GetComponent<Animator>();
        currHealth = maxHealth;
        HPSlider.value = currHealth / maxHealth;
    }

    private void Update() { //every frame

        if (isAttacking) {
            return;
        }
        x_input = Input.GetAxisRaw("Horizontal");  //1 if moving right
        y_input = Input.GetAxisRaw("Vertical");

        Move();

        if (Input.GetKeyDown(KeyCode.J) && attackTimer <= 0) {
            Attack();
        } else {
            attackTimer -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            Debug.Log("holding L");
            Interact();
        }
    }
    #endregion

    #region Movement_functions

    private void Move() {
        anim.SetBool("Moving", true);
        if (x_input>0) {
            PlayerRB.velocity = Vector2.right * movespeed;
            currDirection = Vector2.right;
        } else if (x_input < 0) {
            PlayerRB.velocity = Vector2.left * movespeed;
            currDirection = Vector2.left;
        } else if (y_input > 0) {
            PlayerRB.velocity = Vector2.up * movespeed;
            currDirection = Vector2.up;
        } else if (y_input < 0) {
            PlayerRB.velocity = Vector2.down * movespeed;
            currDirection = Vector2.down;
        } else {
            anim.SetBool("Moving", false);
            PlayerRB.velocity = Vector2.zero;
        }
        anim.SetFloat("DirX", currDirection.x);
        anim.SetFloat("DirY", currDirection.y);
    }
    #endregion

    #region Attack_functions
    private void Attack() {
        Debug.Log("attacking now");
        Debug.Log(currDirection);
        attackTimer = attackspeed;
        StartCoroutine(AttackRoutine()); //animations and hit boxes
    }
    IEnumerator AttackRoutine() { //generator for animations
        isAttacking = true;
        PlayerRB.velocity = Vector2.zero;
        anim.SetTrigger("Attacktrig");

        //sound effects
        FindObjectOfType<AudioManager>().Play("PlayerAttack");

        yield return new WaitForSeconds(hitboxtiming);

        Debug.Log("Casting hitbox now");
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, Vector2.one, 0f, Vector2.zero);

        foreach(RaycastHit2D hit in hits) { 
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy")) {
                Debug.Log("tons of damage");
                hit.transform.GetComponent<Enemy>().TakeDamage(Damage);
            }
        }
        yield return new WaitForSeconds(hitboxtiming);
        isAttacking = false;
        yield return null;
    }
    #endregion

    #region Health_functions
    
    public void TakeDamage(float value) {
        //call sound effect
        FindObjectOfType<AudioManager>().Play("PlayerHurt");
        currHealth -= value;
        Debug.Log("Health is now " + currHealth.ToString());
        //change UI
        HPSlider.value = currHealth / maxHealth;
        

        //check if dead
        if (currHealth <= 0) {
            Die();
        }
    }

    public void Heal(float value) {
        currHealth += value;
        currHealth = Mathf.Min(currHealth, maxHealth);
        Debug.Log("Health is now " + currHealth.ToString());

        //change UI
        HPSlider.value = currHealth / maxHealth;

    }
    //destroys object and triggers end scene

    private void Die() {
        //call death sound effect
        FindObjectOfType<AudioManager>().Play("PlayerDeath");

        //destroy object
        Destroy(this.gameObject);
        GameObject gm = GameObject.FindWithTag("GameController");
        gm.GetComponent<GameManager>().LoseGame();
        //end game
    }

    #endregion

    #region Interact_functions
    private void Interact() {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position + currDirection, new Vector2(0.5f, 0.5f), 0f, Vector2.zero, 0f);
        foreach (RaycastHit2D hit in hits) {
            if (hit.transform.CompareTag("Chest")) {
                hit.transform.GetComponent<Chest>().Interact();
            }
        }
    }
    #endregion
}
