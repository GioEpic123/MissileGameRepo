using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject[] enemies; //Array that Contains all Active objs with "Enemy" Tag
    public Material enemyRed;
    public Material enemyFrozen;

    //Makes an instance of the Manager that stays active within the scene
    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    //This just checks to make sure that theres only one instance of the gamemanager object active.
    private void Awake()
    {
        if (_instance == null && _instance != this)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    //Start - On game start, it says "Game In Progress".
    //Also Declares the Random Object "location" as a new Random (For use with random spawning) & Score=0.
    void Start()
    {
        Debug.Log("Game In Progress");
        Time.timeScale = 1;
        scoreCount = 0;
        location = new System.Random();
        redLight = enemy.transform.GetChild(0).GetComponent<Light>().color;
        greenLight = signal1.GetComponent<Light>().color;
        setGlows();

        
    }

    //Testing
    public bool soundEnabled = true;
    


    //Update - Constantly checks if game is over, notifies console if so & ends game.
    //Constantly checks if wave is over to spawn next wave but only if canCheck = true 
    //(Prevents Overspawning during the Spawn Process)
    bool canCheck = true;
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        waveLength = enemies.Length;

        if ((waveLength == 0) && canCheck)
        {
            canCheck = false;
            waveCount++;
            spawner();
        }
        enemiesRemainHUD.GetComponent<Text>().text = "Remaining: " + waveLength;
        //Updates the Wave Count
        waveCounterHUD.GetComponent<Text>().text = "" + (waveCount + 1);
        //Updates Player's Score
        scoreCountHUD.GetComponent<Text>().text = "Score: " + scoreCount;
    }
    //Signals On Body
    public GameObject signal1;
    bool oneIsActive = true;
    public GameObject signal2;
    bool twoIsActive = true;
    public GameObject signal3;
    bool threeIsActive = true;
    public Material RedGlow;
    public Material GreenGlow;
    Color redLight;
    Color greenLight;
   
    void setGlows()
    {
        signal1.GetComponent<Renderer>().material.SetColor("_Color", GreenGlow.color);
        signal2.GetComponent<Renderer>().material.SetColor("_Color", GreenGlow.color);
        signal3.GetComponent<Renderer>().material.SetColor("_Color", GreenGlow.color);
    }


    public void updateActiveSignals()
    {
        if (!(Protect1go.activeSelf || Protect2go.activeSelf || Protect3go.activeSelf))
        {
            Debug.Log("Ending Game...");
            EndGame();
        }
        else
        {
            //Updates Prot1
            if (!(Protect1go.activeSelf))
            {
                if (oneIsActive)
                {
                    oneIsActive = false;
                    deactivate(signal1);
                }
            }
            else
            {
                if (!oneIsActive)
                {
                    oneIsActive = true;
                    activate(signal1);
                }
            }
            //Updates Prot2
            if (!(Protect2go.activeSelf))
            {
                if (twoIsActive)
                {
                    twoIsActive = false;
                    deactivate(signal2);
                }
            }
            else
            {
                if (!twoIsActive)
                {
                    twoIsActive = true;
                    activate(signal2);
                }
            }
            //Updates Prot3
            if (!(Protect3go.activeSelf))
            {
                if (threeIsActive)
                {
                    threeIsActive = false;
                    deactivate(signal3);
                }
            }
            else
            {
                if (!threeIsActive)
                {
                    threeIsActive = true;
                    activate(signal3);
                }
            }
        }
    }

    void activate(GameObject signal)
    {
        signal.GetComponent<Renderer>().material.SetColor("_Color", GreenGlow.color);
        signal.GetComponent<Light>().color = greenLight;
    }

    void deactivate(GameObject signal)
    {
        signal.GetComponent<Renderer>().material.SetColor("_Color", RedGlow.color);
        signal.GetComponent<Light>().color = redLight;
    }
    //The Radius Used by explosions
    public GameObject blastRad;

    //Power-Ups-------------
    
    float Duration = 0;
    public GameObject powerLightRepair;


    //Ice
    public GameObject powerFreezeGo;//A Gameobject that holds freeze powerup prefab so enemies can drop it
    bool iceInitiated = false;
    bool iceCooled = false;
    public GameObject iceDecal;
    public void ice(GameObject powerUp)
    {
        if (!iceCooled)
        {
            if (!iceInitiated)
            {
                Duration = powerUp.GetComponent<IcePower>().iceWaitTime;
                iceInitiated = true;
                powerUp.GetComponent<IcePower>().freeze();
                iceDecal.SetActive(true);//Activates decal on player
                StartCoroutine(iceCooldown(powerUp));//START THE DURATION
            }
            else
            {
                Debug.Log("Already Froze!");
            }
        }
        else
        {
            Debug.Log("Unfreezing");
            foreach (GameObject eachEnemy in enemies)
            {
                powerFreezeGo.GetComponent<IcePower>().unFreeze(eachEnemy);
            }
            iceDecal.SetActive(false);
            iceCooled = false;
        }
        
    }
    
    //FastFire
    public GameObject powerFastFireGo;
    public GameObject gun;
    public GameObject ffDecal;
    bool ffInitiated = false;
    bool ffCooled = false;
    public void ff(GameObject powerUp)
    {
        if (!ffCooled)
        {
            
            if (!ffInitiated)
            {
                Duration = powerUp.GetComponent<GunPower>().FFwaitTime;
                ffInitiated = true;
                powerUp.GetComponent<GunPower>().fastFire(gun);
                ffDecal.SetActive(true);
                StartCoroutine(ffCooldown(powerUp));
            }
            else
            {
                Debug.Log("Already Fast!");
            }

        }
        else
        {
            Debug.Log("Slowing FireRate");
            powerFastFireGo.GetComponent<GunPower>().slowFire(gun);
            ffDecal.SetActive(false);
            ffCooled = false;
        }
    }

    //The CoreRoutine That controls the Duration of ice powerup
    public IEnumerator iceCooldown(GameObject powerUp)
    {
        Debug.Log("Ice is running...");
        yield return new WaitForSeconds(Duration);
        Debug.Log("Ice Ended");
        if (iceInitiated)
        {
            iceCooled = true;
            iceInitiated = false;
            ice(powerUp);
        }
    }
    //Same but for ff
    public IEnumerator ffCooldown(GameObject powerUp)
    {
        Debug.Log("FF is Running...");
        yield return new WaitForSeconds(Duration);
        Debug.Log("FF Ended");
        if (ffInitiated)
        {
            Debug.Log("Test");
            ffCooled = true;
            ffInitiated = false;
            ff(powerUp);
        }
    }


    //Wave-Based Mechanics-------------
    //For the Player's HUD
    public GameObject enemiesRemainHUD;
    public GameObject waveCounterHUD;
    public GameObject scoreCountHUD;
    //Spawning----
    public float scoreCount;//How many enemies We've killed.
    int waveLength = 3;// How many enemies are present 
    public int waveCount = 0; // Which wave we're on
    public GameObject enemy; //Cube Enemy Prefab
    public GameObject largeEnemy; //Round Enemy Prefab
    public GameObject bomber; //Bomber enemy prefab
    Vector3 myVector; //A vector to spawn enemies 
    public System.Random location; //A random Object for use with spawning
    [HideInInspector]
    public int deltaX; //Change in X Coords, used in sectioning off spawning into 4 sections
    [HideInInspector]
    public int deltaZ; //Change in Z Coords for same reason

    /*
    void spawner()
    {
        //Spawns in 4 Sections, one at a time. 
        //The for loop corrects the coordinates each ideration, so the enemies are only spawned in their section.
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                //change vals for UpRight Corner. 
                deltaX = 1;
                deltaZ = 1;
            }
            else if (i == 1)
            {
                //Change vals for UpLft
                deltaX = -1;
                deltaZ = 1;
            }
            else if (i == 2)
            {
                //Change vals for lowLf
                deltaX = -1;
                deltaZ = -1;
            }
            else
            {
                //Change vals for lowRt
                deltaX = 1;
                deltaZ = -1;
            }

            //Spawns 2 enemies & 1 large enemy for Every Quadrant, per wave (8 enemies & 4 large * wave number)
            for (int j = 0; j < waveCount; j++)
            {
                //Spawns 2 Round here
                for (int k = 0; k < 2; k++)
                {
                    myVector = new Vector3((location.Next(27, 49) * deltaX), 10, (location.Next(27, 49) * deltaZ));
                    GameObject round = Instantiate(enemy);
                    round.transform.position = myVector;
                }
                //Spawns 1 Cube Here
                myVector = new Vector3((location.Next(27, 49) * deltaX), 10, (location.Next(27, 49) * deltaZ));
                GameObject cube = Instantiate(largeEnemy);
                cube.transform.position = myVector;
                //Spawns 1 Bomber Here
                myVector = new Vector3((location.Next(27, 49) * deltaX), 10, (location.Next(27, 49) * deltaZ));
                GameObject bomb = Instantiate(bomber);
                bomb.transform.position = myVector;
            }
        }
        //After Spawning is done, this method Gives Update Method Permission to check if round is over.
        canCheck = true;
    }*/

    void spawner()
    {
        
        //Spawns in 4 Sections, one at a time. 
        //The for loop corrects the coordinates each ideration, so the enemies are only spawned in their section.
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                //change vals for UpRight Corner. 
                deltaX = 1;
                deltaZ = 1;
            }
            else if (i == 1)
            {
                //Change vals for UpLft
                deltaX = -1;
                deltaZ = 1;
            }
            else if (i == 2)
            {
                //Change vals for lowLf
                deltaX = -1;
                deltaZ = -1;
            }
            else
            {
                //Change vals for lowRt
                deltaX = 1;
                deltaZ = -1;
            }

            if (waveCount >= 5)  //After round 5,for each wave n each quad gets (n-1)reg + (n/2)lg + (n/2)bomb
            {
                for (int k = 0; k < (waveCount - 1); k++)
                {
                    myVector = new Vector3((location.Next(27, 49) * deltaX), 10, (location.Next(27, 49) * deltaZ));
                    GameObject cube = Instantiate(enemy);
                    cube.transform.position = myVector;
                }
                for (int l = 0; l < (waveCount / 2); l++)
                {
                    //Spawns 1 Cube Here
                    myVector = new Vector3((location.Next(27, 49) * deltaX), 10, (location.Next(27, 49) * deltaZ));
                    GameObject large = Instantiate(largeEnemy);
                    large.transform.position = myVector;
                    //Spawns 1 Bomber Here
                    myVector = new Vector3((location.Next(27, 49) * deltaX), 10, (location.Next(27, 49) * deltaZ));
                    GameObject bomb = Instantiate(bomber);
                    bomb.transform.position = myVector;
                }

            }
            else
            {
                preSetSpawn();
            }
        }
        //After Spawning is done, this method Gives Update Method Permission to check if round is over.
        canCheck = true;
    }

    int numR;
    int numL;
    int numB;

    void preSetSpawn()
    {
        if (waveCount == 1)//on wave 2 - 2 Reg,1 Lg, 1 Bomb
        {
            numR = 2;
            numL = 1;
            numB = 1;
        }
        else if (waveCount == 2)//on wave 3 - 3 Reg,1 Lg, 1 Bomb
        {
            numR = 3;
            numL = 1;
            numB = 1;
        }
        else if (waveCount == 3)//4r 2l 1b
        {
            numR = 4;
            numL = 2;
            numB = 1;
        }
        else
        {
            numR = 4;
            numL = 2;
            numB = 2;

        }
        for (int k = 0; k < numR; k++)
        {
            myVector = new Vector3((location.Next(27, 49) * deltaX), 10, (location.Next(27, 49) * deltaZ));
            GameObject cube = Instantiate(enemy);
            cube.transform.position = myVector;
        }
        for (int l = 0; l < numL; l++)
        {
            myVector = new Vector3((location.Next(27, 49) * deltaX), 10, (location.Next(27, 49) * deltaZ));
            GameObject large = Instantiate(largeEnemy);
            large.transform.position = myVector;
        }
        for (int m = 0; m < numB; m++)
        {
            myVector = new Vector3((location.Next(27, 49) * deltaX), 10, (location.Next(27, 49) * deltaZ));
            GameObject bomb = Instantiate(bomber);
            bomb.transform.position = myVector;
        }
    }

    //Protection Objects- Objects that need to be protected. Set in inspector.
    public GameObject Protect1go;
    public GameObject Protect2go;
    public GameObject Protect3go;
    //Ends the Game when conditions are met
    public GameObject deathExplosion;
    public GameObject player;
    //For Game Over Screen
    public GameObject EndGameHUD;
    public GameObject finalWaveCount;
    public GameObject finalScoreCount;
    public GameObject crossHair;
    //gameHasEnded tells us if the game is over
    public bool gameHasEnded = false;
    public void EndGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            //Makes an explosion on Death, and simultaneously destroys the player
            Instantiate(deathExplosion, player.transform.position, player.transform.rotation);
            player.SetActive(false);
            //Sets All HUD off, pops up death screen with the score & wave count,and unlocks cursor for menu use
            EndGameHUD.SetActive(true);
            HUD.SetActive(false);
            crossHair.SetActive(false);
            finalScoreCount.GetComponent<Text>().text = "" + scoreCount;
            finalWaveCount.GetComponent<Text>().text = "" + (waveCount + 1);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
            player.GetComponentInParent<PlayerMovement>().write();
        }
    }

   
    //Pausing the Game------------------
    public bool gamePaused = false;
    public GameObject PauseScreen;
    public GameObject HUD;
    //Brings up pause menu & pauses game
    public void Paused()
    {

        if (!gamePaused)
        {
            gamePaused = true;
            Time.timeScale = 0;
            PauseScreen.SetActive(true);
            HUD.SetActive(false);
            crossHair.SetActive(false);
        }
        else if (gamePaused)
        {
            gamePaused = false;
            Time.timeScale = 1;
            PauseScreen.SetActive(false);
            HUD.SetActive(true);
            crossHair.SetActive(true);

        }
    }
}
