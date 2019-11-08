using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPower : MonoBehaviour, IPowerup
{
    public void OnActivation()
    {
        repairLight();
        destroyed = true;
        Destroy(gameObject);
    }

    public GameManager gameManager;
    bool destroyed = false;

    public void repairLight()
    {
        if (!(gameManager.Protect1go.activeSelf && gameManager.Protect2go.activeSelf && gameManager.Protect3go.activeSelf))
        {
            lightToFix = findMissing(gameManager.Protect1go, gameManager.Protect2go, gameManager.Protect3go);
            lightToFix.SetActive(true);
            Debug.Log("Repairing " + lightToFix.name);
        }
        else
        {
            Debug.Log("Cant Repair - All Lights are active!");
        }
    }
    GameObject lightToFix;
    GameObject findMissing(GameObject a, GameObject b, GameObject c)
    {
        if (a.activeSelf)
        {
            if (b.activeSelf)
            {
                return c;
            }
            else
            {
                return b;
            }
        }
        else
        {
            return a;
        }
    }

    public IEnumerator killAfterDelay(float delay)
    {
        Debug.Log("Light Power Dropped");
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    void Start()
    {
        startHeight = transform.position.y;
        gameManager = GameManager.Instance;
        StartCoroutine(killAfterDelay(10f));
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
