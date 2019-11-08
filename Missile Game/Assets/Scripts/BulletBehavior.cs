using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    //THIS CLASS IS for the bullet behavior of actual bullet physics.
    //Once colliders and such are fixed, It will dictate what the bullet does after being spawned in

    private void Update()
    {
        if(gameObject.transform.position.y <= -5)
        {
            Destroy(gameObject);
        }
    }

    public GameObject impactAffect;
    public GameObject Gun;

    /*
    void OnCollisionEnter(Collider other)
    {
        Gun = GameManager.Instance.gun;
        Debug.Log("Hit " + other.gameObject.name);
        impact(other);
        Destroy(gameObject);
        //Impact Affect Below
        GameObject impactGo = Instantiate(impactAffect, gameObject.transform.position, /*Quaternion.LookRotation(hit.normal) gameObject.transform.rotation);
        impactGo.transform.LookAt(Gun.transform);
        impactGo.SetActive(true);
        Destroy(impactGo, 1f);
    }*/

    /*Use Abstraction for Powerups -- lookup inheritance
     *IDamanagble..? --Interface 
     * into Powerup Abstract Class
     * 
     * 
     */

    private void OnTriggerEnter(Collider other)
    {   
        Gun = GameManager.Instance.gun;
        Debug.Log("Hit " + other.gameObject.name);
        impact(other);
        Destroy(gameObject);
        //Impact Affect Below
        GameObject impactGo = Instantiate(impactAffect, gameObject.transform.position, gameObject.transform.rotation);
        impactGo.transform.LookAt(Gun.transform);
        impactGo.SetActive(true);
        Destroy(impactGo, 1f);
        
    }
    
    
    void impact(Collider other)
    {
        Gun.GetComponent<Shooting>().impact(other.gameObject);
    }

}
