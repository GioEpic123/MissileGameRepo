using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {

    public float damage = 10f;
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

    private void Start()
    {
        aud = GetComponent<AudioSource>();
    }
    //For Audio
    AudioSource aud;


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
    public Transform[] bulletSpawns;
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
        aud.Play();

    }

    public int wOffset = 0;
    public int xOffset = 0;
    public int yOffset = 0;
    public int zOffset = 0;

    void spawnBullet()//All this To spawn a bullet and send it where you Aim
    {
        foreach(Transform spawn in bulletSpawns)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            if(!(spawn.parent.GetComponent<Collider>() == null))//Dont count it when you hit the gun, if the gun has a collider
            {
                Physics.IgnoreCollision(bullet.GetComponent<Collider>(),
                spawn.parent.GetComponent<Collider>());
            }
            GameObject[] mutes = new GameObject[] {GameManager.Instance.Protect1go, GameManager.Instance.Protect2go,
            GameManager.Instance.Protect3go};
            foreach (GameObject obj in mutes)
            {
                Physics.IgnoreCollision(bullet.GetComponent<Collider>(),
                obj.transform.GetChild(1).GetComponent<Collider>());
            }
            bullet.transform.position = spawn.position;

            Vector3 rotation = bullet.transform.rotation.eulerAngles;
            //bullet.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, rotation.y, rotation.z);
            //bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
            //Quaternion fixedRot = new Quaternion(transform.rotation.x + xOffset,transform.rotation.y,transform.rotation.z + zOffset,transform.rotation.w + wOffset);     //(transform.rotation.x, transform.rotation.y, transform.rotation.z)
            bullet.transform.rotation = transform.rotation;
            //bullet.transform.rotation = fixedRot;
            bullet.GetComponent<Rigidbody>().AddForce(spawn.forward * bulletSpeed, ForceMode.Impulse);
            StartCoroutine(destroyAfterLifetime(bullet, bulletLifetime));
        } 
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
