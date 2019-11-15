using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{

    public GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        StartCoroutine(killAfterDelay(0.25f));
        Instantiate(gameManager.deathExplosion, transform.position, transform.rotation);
    }

    private void OnTriggerStay(Collider col)
    {

        MonoBehaviour[] list = col.transform.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour mb in list)
        {
            if (mb is IPowerup)
            {
                //Kill Powerup
            }
            if (mb is IDamagable<float>) //Will take lights out & insta-kill any enemy
            {
                IDamagable<float> objHit = (IDamagable<float>)mb;
                objHit.OnTakeDamage(1000);
            }
        }
    }

    public IEnumerator killAfterDelay(float delay)
    {
        Debug.Log("Exploded!");
        yield return new WaitForSeconds(delay);
        Debug.Log("Radius Dissapearing...");
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
