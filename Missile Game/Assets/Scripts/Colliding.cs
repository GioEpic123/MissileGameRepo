using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliding : MonoBehaviour, IDamagable<float>
{
    public GameManager gameManager;
    public GameObject assignedProtect;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            destroyProtect();
        }
    }

    public void OnTakeDamage(float DamageTaken)
    {
        Debug.Log(assignedProtect.name + " hit by bomb.");
        if(DamageTaken >= 60)
        {
            destroyProtect();
        }
    }

    void destroyProtect()
    {
        assignedProtect.SetActive(false);

        Debug.Log(assignedProtect.name + " Was destroyed.");
        gameManager.GetComponent<GameManager>().updateActiveSignals();
    }

}