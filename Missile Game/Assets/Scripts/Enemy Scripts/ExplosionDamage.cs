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
        Instantiate(gameManager.deathExplosion, transform.position, transform.rotation);//Makes Expolsion effect
    }

    private void OnTriggerStay(Collider col)
    {

        MonoBehaviour[] list = col.transform.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour mb in list)
        {
            if (mb is IPowerup)
            {
                IPowerup powerHit = (IPowerup)mb;
                powerHit.OnExploded();
            }
            if (mb is IDamagable<float>) //Will take lights out & insta-kill any enemy
            {
                IDamagable<float> objHit = (IDamagable<float>)mb;
                objHit.OnTakeDamage(60);
            }
        }
    }

    public IEnumerator killAfterDelay(float delay)
    {
        Debug.Log("Exploded!");
        yield return new WaitForSeconds(delay);
        Debug.Log("Explosion Dissapearing...");
        Destroy(gameObject);
    }
}
