using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePower : MonoBehaviour, IPowerup
{
    //Instance Methods
    public void OnActivation()
    {
        gameManager.ice(gameObject);
        Destroy(gameObject);
    }
    public void OnExploded()
    {
        Destroy(gameObject);
        Debug.Log("Ice Expolded!");
    }

    public GameManager gameManager;

    public float iceWaitTime = 2f;

    public void freeze()
    {
        Debug.Log("Freezing");
        foreach (GameObject eachEnemy in gameManager.enemies)
        {
            eachEnemy.gameObject.GetComponent<Targeting>().freeze();
            eachEnemy.transform.GetChild(0).GetComponent<Light>().color = Color.blue;

            /*--Trying to get blue rendering working
            if(eachEnemy.GetComponent<Renderer>())
                eachEnemy.GetComponent<Renderer>().material.color = gameManager.enemyFrozen.color;
            else
                eachEnemy.transform.GetChild(0).GetComponent<Renderer>().material.color = gameManager.enemyFrozen.color;
            /*
            if (eachEnemy.GetComponent<Renderer>() != null)
                eachEnemy.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
            else
                eachEnemy.gameObject.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);*/


        }
    }


    public void unFreeze(GameObject eachEnemy)
    {
        if (!eachEnemy.GetComponent<target>().destroyed)
        {
            eachEnemy.GetComponent<Targeting>().unFreeze();
            eachEnemy.transform.GetChild(0).GetComponent<Light>().color = eachEnemy.GetComponent<target>().redLight;
        }
            

        /*--Trying to get the blue renderer working :/
        if (eachEnemy.GetComponent<Renderer>())
            eachEnemy.GetComponent<Renderer>().material.color = gameManager.enemyRed.color;
        else
            eachEnemy.transform.GetChild(0).GetComponent<Renderer>().material.color = gameManager.enemyRed.color;

        /*eachEnemy.transform.GetChild(0).GetComponent<Renderer>().material.color = gameManager.enemyRed.color;
        /*
        if (eachEnemy.GetComponent<Renderer>() != null)
            eachEnemy.GetComponent<Renderer>().material.SetColor("_Color", eachEnemy.GetComponent<target>().redMaterial);
        else
            eachEnemy.gameObject.transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", eachEnemy.GetComponent<target>().redMaterial);*/
    }

    void Start()
    {
        startHeight = transform.position.y;
        gameManager = GameManager.Instance;
        StartCoroutine(killAfterDelay(10f));
    }

    public IEnumerator killAfterDelay(float delay)
    {
        Debug.Log("Ice Power Dropped");
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    //Makes PowerUp Rotate
    float spinSpeed = 120f;
    float startHeight;
    void Update()
    {
        //Rotates, and bounces it up and down
        transform.Rotate(Vector3.left, spinSpeed * Time.deltaTime);

        //Bobs the Powerup Up and Down
        transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(startHeight - 0.15f, startHeight + 0.15f, Mathf.PingPong(Time.time * 0.8f, 1f)), transform.position.z);
    }
}
