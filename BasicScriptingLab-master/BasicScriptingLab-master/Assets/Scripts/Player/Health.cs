using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    /// Non-programmers only need to look between >>>> and <<<<
    /// >>>>

    /* Most scripts will have a collection of Variables at the top. Similar to their function
    in mathematical equations, a variable is a name that is associated with a value. In this example, 
    the variable startingHealth is set to a value of 1. Following the variable is a comment that 
    explains its purpose. Variables preceded by the keyword public will show up in the Inspector tab 
    when you attach this script to a GameObject in the Hierarchy tab. */

    public int startingHealth = 1; // This is how much health you have before you die

    // <<<< Non-programmers may stop reading here

    int currentHealth;

    // The Start function is always called once when the game starts or when the object is first created.
	void Start () {
        currentHealth = startingHealth;
	}

    // This function is public so that other scripts can call it,
    public void takeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {   // In a real game, you would probably display a game over screen.  Here instead we disable a handful of components to simulate the same effect.

            // This turns the player invisible.
            gameObject.GetComponent<SpriteRenderer>().enabled = false;

            // This prevents the player from moving.
            gameObject.GetComponent<PlayerMovement>().enabled = false;

            // This prevents enemies from dying to the player after the game ends.
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            // "gameObject" is a keyword similar to "this" in Java. It refers to the GameObject that this script is attached to.
            // Inside the <>'s of GetComponent<Type>(), write the class name of the component you want to get.

            // Alternatively for a "game over" you can use:
            // Destroy(gameObject);
            // The downside with this line is that many other scripts will have null pointers and will throw errors once the player is destroyed (which is technically fine).

            GameObject.Find("GameManager").GetComponent<MyGameManager>().GameOver();
        }
    }
}
