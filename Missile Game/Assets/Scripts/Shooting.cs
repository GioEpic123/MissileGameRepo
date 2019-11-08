using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {

    public float damage = 10f;
    public float range = 100f;
    public float setFireRate;
    public float defaultFireRate = 5f;
    public float chargedFireRate = 10f;

    private float nextTimeToFire = 0f;

    public Camera eyes;
    public GameObject impactAffect;

    //for muzzle flash animation 
    public ParticleSystem DefaultMuzzle;
    public ParticleSystem ChargedMuzzle;
    [HideInInspector]
    public ParticleSystem setMuzzle;

    private void Awake()
    {
        setMuzzle = DefaultMuzzle;
        setFireRate = defaultFireRate;
    }

    void Update () {
        /*
		if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / setFireRate;
            Shoot();
        }*/

        //Developing Bullet physics
        
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / setFireRate;
            Debug.Log("Firing Bullet");
            fire();
        }
	}

    //Bullet Physics Code
    
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletLifetime = 1f;
    public float bulletSpeed = 10f;
    
    private IEnumerator destroyAfterLifetime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    void fire()
    {
        setMuzzle.Play();
        spawnBullet();
    }

    void spawnBullet()//All this To spawn a bullet and send it where you Aim
    {
        GameObject bullet = Instantiate(bulletPrefab);
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(),
            bulletSpawn.parent.GetComponent<Collider>());//Dont count it when you hit the gun
        GameObject[] mutes = new GameObject[] {GameManager.Instance.Protect1go, GameManager.Instance.Protect1go,
            GameManager.Instance.Protect1go};
        foreach(GameObject obj in mutes)
        {
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(),
            obj.transform.GetChild(1).GetComponent<Collider>());
        }
        bullet.transform.position = bulletSpawn.position;

        Vector3 rotation = bullet.transform.rotation.eulerAngles;
        bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(destroyAfterLifetime(bullet, bulletLifetime));
    }

    public void impact(GameObject hit)
    {
            MonoBehaviour[] list = hit.transform.GetComponents<MonoBehaviour>();
            foreach(MonoBehaviour mb in list)
            {
                if(mb is IPowerup)
                {
                    IPowerup powerup = (IPowerup)mb;
                    powerup.OnActivation();
                }
                if(mb is IDamagable<float>)
                {
                    IDamagable<float> objHit = (IDamagable<float>)mb;
                    objHit.OnTakeDamage(damage);
                }
            }
    }
    

    //Concerned With Raycast Shooting, Will remove on implimentation of bulletphysics
    /*
    void Shoot()
    {
        setMuzzle.Play();
        
        RaycastHit hit;
        if (Physics.Raycast(eyes.transform.position, eyes.transform.forward, out hit, range))
        {
            MonoBehaviour[] list = hit.transform.GetComponents<MonoBehaviour>();
            foreach(MonoBehaviour mb in list)
            {
                if(mb is IPowerup)
                {
                    IPowerup powerup = (IPowerup)mb;
                    powerup.OnActivation();
                }
                if(mb is IDamagable<float>)
                {
                    IDamagable<float> objHit = (IDamagable<float>)mb;
                    objHit.OnTakeDamage(damage);
                }
            }

            GameObject impactGo = Instantiate(impactAffect, hit.point, Quaternion.LookRotation(hit.normal));
            impactGo.SetActive(true);
            Destroy(impactGo, 1f);
        }
    }*/

    
}
