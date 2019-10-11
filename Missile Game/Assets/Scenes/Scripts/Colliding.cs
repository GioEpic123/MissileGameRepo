using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliding : MonoBehaviour
{
    public GameManager gameManager;
    //public PlayerMovement movement;
    private void OnTriggerStay(Collider col) {
        if (col.gameObject.tag == "Enemy")
        {
            //movement.enabled = false;
            gameManager.EndGame();
            Debug.Log("HitEnemy");

        }
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("Collision Detected");

        if (collisionInfo.gameObject.tag == "Enemy")
        {
            //movement.enabled = false;
            gameManager.EndGame();
            Debug.Log("HitEnemy");

        }
        else
        {
            Debug.Log("Not Enemy");
        }


    }

}