using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Targeting : MonoBehaviour
{
    //Targeting Handles the "Seek" mechanic, which decides which light to head towards.
    //Targeting also Handles Player Movement.
    public NavMeshAgent agent;

    Transform closestLight;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the NavMesh for Movement here
        agent = GetComponent<NavMeshAgent>();

        gameManager = GameManager.Instance;
        Prot1go = gameManager.Protect1go;
        Prot2go = gameManager.Protect2go;
        Prot3go = gameManager.Protect3go;
        Prot1 = Prot1go.transform;
        Prot2 = Prot2go.transform;
        Prot3 = Prot3go.transform;
        Seek();
    }


    void Update()
    {
        //Movement of enemy hereeee
        if (!(closestLight.gameObject.activeSelf))//Only Seeks if current closestlight is inactive
        {
            if (gameManager.Protect1go.activeSelf || gameManager.Protect2go || gameManager.Protect3go)
            {
                invokeSeek();
                Debug.Log(gameObject.name + " is Re-Seeking");
            }

        }

        agent.SetDestination(closestLight.position);

        /*
        try
        {
            agent.SetDestination(closestLight.position);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        catch
        {
            Debug.Log("Free-Falling");
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }*/
        
    }

    public void freeze()
    {
        agent.isStopped = true;
    }
    public void unFreeze()
    {
        agent.isStopped = false;
    }

    //Transforms for position tracking
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

    public void setCloseLight(Transform light)
    {
        closestLight = light;
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
                    setCloseLight(Prot1);
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
                        setCloseLight(Prot2);
                    }
                }
                else //Must just be 3 then
                {
                    setCloseLight(Prot3);
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
                setCloseLight(Prot3);
            }
            else //If Prot 2 is closer than Prot 3 & 1...
            {
                setCloseLight(Prot2);
            }
        }
        else //If Prot 1 is Closer to enemy than Prot 2
        {
            //If Prot 3 is closer to Enemy than Prot 1 & 2...
            if (Vector3.Distance(Prot1.position, transform.position) >= Vector3.Distance(Prot3.position, transform.position))
            {
                setCloseLight(Prot3);
            }
            else //If Prot 1 is closer to enemy than prot 2 & 3...
            {
                setCloseLight(Prot1);
            }
        }
    }

    void seekIfTwo(Transform first, Transform second)
    {
        if (Vector3.Distance(first.position, transform.position) >= Vector3.Distance(second.position, transform.position))
        {
            setCloseLight(second);
        }
        else
        {
            setCloseLight(first);
        }
    }

}


