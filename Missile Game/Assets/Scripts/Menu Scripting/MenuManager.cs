using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //For Pause Menu --- --- --- ---
    public GameObject astheticFog;
    public GameObject astheticDust;
    bool fogOn;
    bool dustOn;

    void Start()
    {
        player = GameManager.Instance.player;
        if (astheticDust.activeSelf)
            dustOn = true;
        else
            dustOn = false;

        if (astheticFog.activeSelf)
            fogOn = true;
        else
            fogOn = false;
    }

    public void toggleFog()
    {
        if (fogOn)
        {
            astheticFog.SetActive(false);
            fogOn = false;
        }
        else
        {
            astheticFog.SetActive(true);
            fogOn = true;
        }
    }

    public void toggleDust()
    {
        if (dustOn)
        {
            astheticDust.SetActive(false);
            dustOn = false;
        }
        else
        {
            astheticDust.SetActive(true);
            dustOn = true;
        }
    }


    public void ToMain()
    {
        Debug.Log("Quiting to Main...");
        SceneManager.LoadScene("MainMenu");
    }

    public GameObject player;

    public void Restart()
    {
        player.GetComponentInParent<PlayerMovement>().write();
        Debug.Log("Restarting");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;

    }
    

    //For Main Menu --- --- --- ---

    //Starts The First Level
    public void StartLevel()
    {
        Debug.Log("Starting First Level");
        SceneManager.LoadScene("Level 0");

    }

    public void thatOneLevelMadeForTestingIThinkICalledItDevOrSomething()
    {
        Debug.Log("Through the wormhole...");
        SceneManager.LoadScene("TestLevl");
    }

    public void loadInfoLevel()
    {
        Debug.Log("Going to Info Scene");
        SceneManager.LoadScene("Info");
    }

    //Quits Game
    public void Quit()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }


}
