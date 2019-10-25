using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

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
    }

    //Update - Constantly checks if game is over, notifies console if so & ends game.
    //Constantly checks if wave is over to spawn next wave but only if canCheck = true 
    //(Prevents Overspawning during the Spawn Process)
    bool canCheck = true;
    void Update()
    {
        if (!(Protect1go.activeSelf || Protect2go.activeSelf || Protect3go.activeSelf))
        {
            Debug.Log("Ending Game...");
            EndGame();
        }

        //Checks if Enemies are present, if not then it starts up a new wave
        if ((GameObject.FindGameObjectsWithTag("Enemy").Length == 0) && canCheck)
        {
            canCheck = false;
            levelComplete();


        }

        //Updates the Number of Enemies to display on screen.
        waveLength = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemiesRemainHUD.GetComponent<Text>().text = "Remaining: " + waveLength;

        //Updates the Wave Count
        waveCounterHUD.GetComponent<Text>().text = "" + (waveCount + 1);

        //Updates Player's Score
        scoreCountHUD.GetComponent<Text>().text = "Score: " + scoreCount;
    }


    //Below is Concerned with The Wave-based Mechanics

    //For the Player's HUD
    public GameObject enemiesRemainHUD;
    public GameObject waveCounterHUD;
    public GameObject scoreCountHUD;

    public int scoreCount;//How many enemies We've killed.
    int waveLength;// How many enemies are present 
    int waveCount = 0; // Which wave we're on
    
    public GameObject enemy; //Cube Enemy Prefab
    public GameObject roundEnemy; //Round Enemy Prefab

    public Vector3 myVector; //A vector to spawn enemies 

    //When wave ends, incriments waveCount & spawns the new wave
    void levelComplete(){
            waveCount++;
            spawner();
    }

    System.Random location; //A random Object for use with spawning
    public int deltaX; //Change in X Coords, used in sectioning off spawning into 4 sections
    public int deltaZ; //Change in Z Coords for same reason

    void spawner(){
        //Spawns in 4 Sections, one at a time. 
        //The for loop corrects the coordinates each ideration, so the enemies are only spawned in their section.
        for (int i = 0; i < 4; i++){
            if (i == 0){
                //change vals for UpRight Corner. 
                deltaX = 1;
                deltaZ = 1;
            } else if (i == 1){
                //Change vals for UpLft
                deltaX = -1;
                deltaZ = 1;
            } else if (i == 2){
                //Change vals for lowLf
                deltaX = -1;
                deltaZ = -1;
            } else{
                //Change vals for lowRt
                deltaX = 1;
                deltaZ = -1;
            }
                
            //Spawns 2 RoundEnemies and 1 Cube in each section for every wave.
            for(int j = 0; j < waveCount; j++){
                //Spawns 2 Round here
                for(int k = 0; k < 2; k++){
                    myVector = new Vector3((location.Next(27, 49) * deltaX ),10,(location.Next(27, 49)  * deltaZ));
                    GameObject round = Instantiate(roundEnemy);
                    round.transform.position = myVector;
                }
                //Spawns 1 Cube Here
                myVector = new Vector3((location.Next(27, 49)*deltaX), 10,(location.Next(27, 49)*deltaZ));
                GameObject cube = Instantiate(enemy);
                cube.transform.position = myVector;
            }
        }
        //After Spawning is done, this method Gives Update Method Permission to check if round is over.
        canCheck = true;
    }
    /* NOTES ON SPAWNING 
     *Keep in mind the field is a plane, player starts out aiming up the Z axis(plane is a x,z plane)
     *for each corner, predefined from the corner of the map (x,y) to a given range (x+range, y+range), 
      giving us a square spawning ground
     *spawn an enemy in a random spot within said spawning ground. use random number gen and have it 
      clamp to the range's avaliable locations.

*/

    //Protection Objects- Objects that need to be protected. Set in inspector.
    public GameObject Protect1go;
    public GameObject Protect2go;
    public GameObject Protect3go;


    //Ends the Game when conditions are met
    public GameObject deathExplosion;
    public GameObject player;

    bool gameHasEnded = false;
    public float restartDelay = 3f;

    public void EndGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            //Makes an explosion on Death, and simultaneously destroys the player
            Instantiate(deathExplosion, player.transform.position, player.transform.rotation);
            player.SetActive(false);
            //Restarys the game after a given delay.
            //Invoke("Restart", restartDelay);
        }
    }

    //Restarts Level - for use with buttons & death.
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    //Concerned With Pausing the Game
    
    public bool gamePaused = false;
    public GameObject PauseScreen;
    public GameObject HUD;
    //public Text pauseText;

    //Brings up pause menu & pauses game
    public void Paused()
    {

        if (!gamePaused)
        {
            gamePaused = true;
            Time.timeScale = 0;
            PauseScreen.SetActive(true);
            HUD.SetActive(false);
        }
        else if (gamePaused)
        {
            gamePaused = false;
            Time.timeScale = 1;
            PauseScreen.SetActive(false);
            HUD.SetActive(true);

        }
    }
}
