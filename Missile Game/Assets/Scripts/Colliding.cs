using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliding : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject assignedProtect;

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            assignedProtect.SetActive(false);

            Debug.Log(assignedProtect.name + " Was destroyed.");


        }
    }

}