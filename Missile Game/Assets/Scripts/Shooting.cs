using UnityEngine;

public class Shooting : MonoBehaviour {

    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    private float nextTimeToFire = 0f;

    public Camera eyes;
    public GameObject impactAffect;

    //for muzzle flash animation 
    public ParticleSystem Muzzle;

	void Update () {
		if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
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

            GameObject impactGo = Instantiate(impactAffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 1f);

           
        }
    }

    
}
