using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour, IDamagable<float>
{
    public GameManager gameManager;
    public GameObject player;
    //For use with taking damage. Each enemy prefab has their own modified health
    public float health = 50f;
    //For use of movement. ForwardForce is their movement speed,
    //rotationSpeed is how fast they change direction.
    public float forwardForce = 2f;
    public float FINALFORCE;
    public float rotationSpeed = 3f;
    public Color redLight;
    public Color redMaterial;

    public void OnTakeDamage(float DamageTaken)
    {
        TakeDamage(DamageTaken);
        if (floatingTextPrefab)
        {
            showFloatingText();
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
        if (health <= 0f)
        {
            if (!isBomber)
            {
                Die();
            }
            else
            {
                Explode();
            } 
        }
    }
    public bool destroyed = false;
    public int nextChance = 0;
    public GameObject powerFreeze;
    public GameObject powerFastFire;
    public GameObject powerLightRepair;
    //Kills the Enemy when conditions are met, and incriments Player's Score
    void Die()
    {
        destroyed = true;
        Destroy(gameObject);
        GameManager.Instance.scoreCount++;
        spawnDrop();

    }

    public GameObject radiusSphere;
    void Explode()
    {
        Die();
        GameObject rad = Instantiate(radiusSphere);
        rad.transform.position = gameObject.transform.position;
        
    }

    void spawnDrop()
    {
        if (nextChance <= 25)//25%
        {
            GameObject freeze = Instantiate(powerFreeze);
            freeze.transform.position = gameObject.transform.position;
        }
        else if (nextChance <= 40)//15%
        {
            GameObject fastFire = Instantiate(powerFastFire);
            fastFire.transform.position = gameObject.transform.position;
        }
        else if(nextChance <= 45)//%5
        {
            GameObject lightPower = Instantiate(powerLightRepair);
            lightPower.transform.position = gameObject.transform.position;
        }
    }

    IEnumerator checkForChance()
    {
        yield return new WaitForSeconds(2f);
        nextChance = gameManager.location.Next(0, 101); //uses the random generator used in randomized spawning, to find a chance to drop ice.

    }
    //UPDATE
    //Movement Is in Update, Moves towards player at FORWARDFORCE speed
    void Update()
    {
        //Only Seeks if current closestlight is inactive
        if (!(closestLight.gameObject.activeSelf))
        {
            if (Prot1go.activeSelf || Prot2go.activeSelf || Prot3go.activeSelf)
            {
                Seek();
                Debug.Log(gameObject.name + " is Re-Seeking");
            }

        }

        //A smoother version of LookAt, instead using Slerp to smoothly transition rotation.
        //Float rotationSpeed affects how fast they change direction.
        Quaternion targetRotation = Quaternion.LookRotation(closestLight.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //Move In forward direction, towards closestLight
        transform.position += transform.forward * forwardForce * Time.deltaTime;

        if (gameObject.transform.position.y <= -5)
        {
            Destroy(gameObject);
        }
    }

    //RUDIMENTARY AI--Enemy will seek the closest of the 3 lights, Prot1,Prot2, & Prot3

    //Start- Will assign all Protection Game Objects(Prot1go, etc...) and their transforms as Prots(Prot1, etc)  

    void Start()
    {
        if(gameObject.transform.childCount > 0)
        {
            if(gameObject.transform.GetChild(0).name == "BombEye")
                isBomber = true;
            
        }
        gameManager = GameManager.Instance;
        //Finds all Protection Objects, assigns them, and then looks for them.
        Prot1go = gameManager.Protect1go;
        Prot2go = gameManager.Protect2go;
        Prot3go = gameManager.Protect3go;
        Prot1 = Prot1go.transform;
        Prot2 = Prot2go.transform;
        Prot3 = Prot3go.transform;
        Seek();
        //Things That Must Be Initialized After Spawning...
        FINALFORCE = forwardForce;
        redLight = gameObject.GetComponent<Light>().color;
        redMaterial = gameObject.GetComponent<Renderer>().material.color;
        powerFreeze = GameManager.Instance.powerFreezeGo;
        powerFastFire = GameManager.Instance.powerFastFireGo;
        powerLightRepair = GameManager.Instance.powerLightRepair;
        StartCoroutine(checkForChance());
        player = gameManager.gun.gameObject;
        radiusSphere = gameManager.blastRad;
    }

    bool isBomber = false;

    //Transforms for position tracking
    public Transform closestLight;
    public Transform Prot1;
    public Transform Prot2;
    public Transform Prot3;
    //Objects of These Types
    public GameObject Prot1go;
    public GameObject Prot2go;
    public GameObject Prot3go;

    public void invokeSeek()
    {
        Seek();
    }

    void Seek()
    {
        if (Prot1go.activeSelf && Prot2go.activeSelf && Prot3go.activeSelf)
        {
            seekIfAll();
        }
        else //If ALL 3 aren't Present,
        {
            //If 1...
            if (Prot1go.activeSelf)
            {
                if (Prot2go.activeSelf)//If 1 & 2
                {
                    seekIfTwo(Prot1, Prot2);
                }
                else if (Prot3go.activeSelf)//If 1 & 3
                {
                    seekIfTwo(Prot1, Prot3);
                }
                else //Must just be 1 then
                {
                    closestLight = Prot1;
                }
            }
            else
            {
                if (Prot2go.activeSelf)//If not 1, then 2..
                {
                    if (Prot3go.activeSelf)//If 2 & 3
                    {
                        seekIfTwo(Prot2, Prot3);
                    }
                    else //Must just be 2 then
                    {
                        closestLight = Prot2;
                    }
                }
                else //Must just be 3 then
                {
                    closestLight = Prot3;
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
    void seekIfTwo(Transform first, Transform second)
    {
        if (Vector3.Distance(first.position, transform.position) >= Vector3.Distance(second.position, transform.position))
        {
            closestLight = second;
        }
        else
        {
            closestLight = first;
        }
    }

}

