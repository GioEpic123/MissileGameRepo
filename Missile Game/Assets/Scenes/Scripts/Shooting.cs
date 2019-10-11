using UnityEngine;

public class Shooting : MonoBehaviour {

    public float damage = 10f;
    public float range = 100f;

    public Camera eyes;

    //for muzzle flash animation 
    public ParticleSystem Muzzle;
    //USE FOR RELOAD LATER
    //if (Input.GetKey("r")){}
    //public float reloadDelay = 1f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
	}

    void Shoot()
    {
        Muzzle.Play();

        RaycastHit hit;
        if (Physics.Raycast(eyes.transform.position, eyes.transform.forward, out hit, range))
        {
            Debug.Log("Shot " + hit.transform.name);

            target target = hit.transform.GetComponent<target>();
            if (target != null)
            {
                target.TakeDamage(damage); 
            }
        }
    }

}
