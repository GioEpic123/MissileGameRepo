using UnityEngine;

public class target : MonoBehaviour {

    public float health = 50f;
    public float forwardForce = 10;
    public Transform player;
    public GameManager gameManager;



    void Update()
    {
        transform.LookAt(player);
        transform.position -= transform.forward * forwardForce * Time.deltaTime;
    }

    public void TakeDamage (float amount)
    {
        transform.LookAt(player);
        health -= amount;
        if (health <=  0f)
        {
            Die();
        }

    }

    void Die()
    {
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hello pls work");
        if (other.gameObject.tag == "Player")
        {
            //movement.enabled = false;
            gameManager.EndGame();
            Debug.Log("HitEnemy");

        }
    }

}
