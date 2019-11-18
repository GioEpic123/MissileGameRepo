using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPower : MonoBehaviour, IPowerup
{   
    public void OnActivation()
    {
        gameManager.ff(gameObject);
        Destroy(gameObject);
    }
    public void OnExploded()
    {
        Destroy(gameObject);
        Debug.Log("FastFire Expolded!");
    }

    public float FFwaitTime = 5f;
    public void fastFire(GameObject mGun)
    {
        Shooting shooting = mGun.GetComponent<Shooting>();

        Debug.Log("Speeding FireRate");
        shooting.DefaultMuzzle.gameObject.SetActive(false);
        shooting.ChargedMuzzle.gameObject.SetActive(true);
        shooting.setFireRate = shooting.chargedFireRate;
        shooting.setMuzzle = shooting.ChargedMuzzle;
    }
    public void slowFire(GameObject mGun)
    {
        Shooting shooting = mGun.GetComponent<Shooting>();

        shooting.DefaultMuzzle.gameObject.SetActive(true);
        shooting.ChargedMuzzle.gameObject.SetActive(false);
        shooting.setFireRate = mGun.GetComponent<Shooting>().defaultFireRate;
        shooting.setMuzzle = mGun.GetComponent<Shooting>().DefaultMuzzle;

    }

    public GameManager gameManager;
    void Start()
    {
        startHeight = transform.position.y;
        gameManager = GameManager.Instance;
        StartCoroutine(killAfterDelay(10f));
    }

    public IEnumerator killAfterDelay(float delay)
    {
        Debug.Log("Gun Power Dropped");
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
