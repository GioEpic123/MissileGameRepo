using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
    //For use with taking damage. Each enemy prefab has their own modified health
    public float health = 50f;
    //For use of movement. ForwardForce is their movement speed,
    //rotationSpeed is how fast they change direction.
    public float forwardForce = 2f;
    public float rotationSpeed = 3f;

    //private GameManager gameManager;

  
    //Movement Is in Update, Moves towards player at FORWARDFORCE speed
    void Update()
    {
        //Only Seeks if current closestlight is inactive
        if (!(closestLight.gameObject.activeSelf))
        {
            Seek();
        }
        
        
        
        //A smoother version of LookAt, instead using Slerp to smoothly transition rotation.
        //Float rotationSpeed affects how fast they change direction.
        Quaternion targetRotation = Quaternion.LookRotation(closestLight.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //Move In forward direction, towards closestLight
        transform.position += transform.forward * forwardForce * Time.deltaTime;
    }


    //When Shot...
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }

    }

    //Kills the Enemy when conditions are met, and incriments Player's Score
    void Die()
    {
        Destroy(gameObject);
        GameManager.Instance.scoreCount++;
    }


    //RUDIMENTARY AI--Enemy will seek the closest of the 3 lights, Prot1,Prot2, & Prot3

    //Start- Will assign all Protection Game Objects(Prot1go, etc...) and their transforms as Prots(Prot1, etc)  

    void Start()
    {
        //gameManager = GameManager.Instance;
        //Rather than trying to grab the values from a prefab that doesnt have them, this code gathers the
        //values from an instance that exists in the scene. Instance is made in Gamemanager
        Prot1go = GameManager.Instance.Protect1go;
        Prot2go = GameManager.Instance.Protect2go;
        Prot3go = GameManager.Instance.Protect3go;

        Prot1 = Prot1go.transform;
        Prot2 = Prot2go.transform;
        Prot3 = Prot3go.transform;

        Seek();
    }
    
    //Transforms for position tracking
    public Transform closestLight;
    public Transform Prot1;
    public Transform Prot2;
    public Transform Prot3;
    //Objects of These Types
    public GameObject Prot1go;
    public GameObject Prot2go;
    public GameObject Prot3go;

    bool SaidMissing = false;
    void SayMissing()
    {
        if (!SaidMissing)
        {
            Debug.Log("Somethings Missing..");
            SaidMissing = true;
        }
    }

    void Seek()
    {
        if (Prot1go.activeSelf && Prot2go.activeSelf && Prot3go.activeSelf)
        {
            seekIfAll();
            
        }
        else //If ALL 3 aren't Present,
        {
            
            SayMissing();
            //If 1 & 2
            if (Prot1go.activeSelf)
            {
                if (Prot2go.activeSelf)
                {
                    seekIfOneTwo();
                }
                else if (Prot3go.activeSelf)
                {
                    seekIfOneThree();
                }
                else
                {
                    closestLight = Prot1;
                }
            }
            else
            {
                if (Prot2go.activeSelf)
                {
                    if (Prot3go.activeSelf)
                    {
                        seekIfTwoThree();
                    }
                    else
                    {
                        closestLight = Prot2;
                    }
                }
                else
                {
                    if (Prot3go.activeSelf)
                    {
                        closestLight = Prot3;
                    }
                }
            }

        }
    }
    void seekIfAll()
    {
        //If Prot 2 is closer to Enemy than Prot 1...
        if (Vector3.Distance(Prot1.position, transform.position) >= Vector3.Distance(Prot2.position, transform.position))
        {

            //and Prot 3 is closer than Prot 2 & 1...
            if (Vector3.Distance(Prot2.position, transform.position) >= Vector3.Distance(Prot3.position, transform.position))
            {
                closestLight = Prot3;
            }
            else //If Prot 2 is closer than Prot 3 & 1...
            {
                closestLight = Prot2;
            }
        }
        else //If Prot 1 is Closer to enemy than Prot 2
        {
            //If Prot 3 is closer to Enemy than Prot 1 & 2...
            if (Vector3.Distance(Prot1.position, transform.position) >= Vector3.Distance(Prot3.position, transform.position))
            {
                closestLight = Prot3;
            }
            else //If Prot 1 is closer to enemy than prot 2 & 3...
            {
                closestLight = Prot1;
            }
        }
    }

    public float distBetween2;
    public float distBetween1;

    void seekIfOneTwo()
    {
        if (Vector3.Distance(Prot1.position, transform.position) >= Vector3.Distance(Prot2.position, transform.position))
        {
            closestLight = Prot1;
           
        }
        else if (distBetween1 < distBetween2)
        {
            
            closestLight = Prot2;
        }
    }

    void seekIfTwoThree()
    {
        if (Vector3.Distance(Prot2.position, transform.position) >= Vector3.Distance(Prot3.position, transform.position))
        {
            closestLight = Prot3;
        }
        else
        {
            closestLight = Prot2;
        }
    }

    void seekIfOneThree()
    {
        

        if (Vector3.Distance(Prot1.position, transform.position) >= Vector3.Distance(Prot3.position, transform.position))
        {
            closestLight = Prot3;
        }
        else
        {
            closestLight = Prot1;
        }
        
    }
}

