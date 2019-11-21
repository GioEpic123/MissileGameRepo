﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour, IDamagable<float>
{
    public GameManager gameManager;
    public GameObject player;
    //For use with taking damage. Each enemy prefab has their own modified health
    public float startHealth;
    public float health = 50;
    //For use of movement. ForwardForce is their movement speed,
    //rotationSpeed is how fast they change direction.
    //public float rotationSpeed = 3f; -- Delete later
    public Color redLight;
    //Transforms for position tracking
    public Transform closestLight;
    int nextChance = 0; //The enemy's current chance for a drop



    //Start--

    void Start()
    {

        //Things That Must Be Initialized After Spawning...
        startHealth = health;
        gameManager = GameManager.Instance;
        redLight = gameObject.transform.GetChild(0).GetComponent<Light>().color;
        anim = gameObject.GetComponent<Animator>();
        //redMaterial = gameObject.GetComponent<Renderer>().material.color;
        player = gameManager.gun.gameObject;

        //Calls some funtions After Spawning...
        StartCoroutine(checkForChance());


    }

    //UPDATE
    //Movement Is in Update, Moves towards player at FORWARDFORCE speed
    void Update()
    {

        /*----OLD MOVEMENT, 
        //A smoother version of LookAt, instead using Slerp to smoothly transition rotation.
        //Float rotationSpeed affects how fast they change direction.
        Quaternion targetRotation = Quaternion.LookRotation(closestLight.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //Move In forward direction, towards closestLight
        transform.position += transform.forward * forwardForce * Time.deltaTime;
        */
        if (gameObject.transform.position.y <= -5)
        {
            Destroy(gameObject);
        }
    }


    IEnumerator checkForChance()
    {
        yield return new WaitForSeconds(2f);
        nextChance = gameManager.location.Next(0, 101); //uses the random generator used in randomized spawning, to find a chance to drop ice.

    }

    public void invokeSeek()
    {
        if(closestLight)
            gameObject.GetComponent<Targeting>().invokeSeek();
    }

    public void OnTakeDamage(float DamageTaken)
    {
        if (!destroyed)
        {
            TakeDamage(DamageTaken);
            if (floatingTextPrefab)
            {
                showFloatingText();
            }
        }
        
    }

    public GameObject floatingTextPrefab;
    public GameObject go;
    void showFloatingText()
    {
        Vector3 fixedPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        //go = Instantiate(floatingTextPrefab, transform.position, Quaternion.LookRotation(player.transform.position), transform);
        go = Instantiate(floatingTextPrefab, fixedPos, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = "" + health;
 
    }


    //When Shot...
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
                Die();       
        }
    }
    public bool destroyed = false;

    //Kills the Enemy when conditions are met, and incriments Player's Score
    void Die()
    {
        destroyed = true;
        GetComponent<Targeting>().freeze();
        if (gameObject.GetComponent<Bomber>())
        {
            gameObject.GetComponent<Bomber>().Explode();
            Destroy(gameObject);
        }
        else
        {
            anim.SetBool("isDead", true);
            Destroy(gameObject, .5f);
        } 
        GameManager.Instance.scoreCount = startHealth + GameManager.Instance.scoreCount;
        spawnDrop();
        
        
    }
    Animator anim;

    void spawnDrop()
    {
        if (nextChance <= 35)//35%
        {
            GameObject freeze = Instantiate(gameManager.powerFreezeGo);
            freeze.transform.position = gameObject.transform.position;
        }
        else if (nextChance <= 60)//25%
        {
            GameObject fastFire = Instantiate(gameManager.powerFastFireGo);
            fastFire.transform.position = gameObject.transform.position;
        }
        else if(nextChance <= 65)//%5
        {
            GameObject lightPower = Instantiate(gameManager.powerLightRepair);
            lightPower.transform.position = gameObject.transform.position;
        }
    }


}

