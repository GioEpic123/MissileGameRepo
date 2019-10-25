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
    


    public void Restart()
    {
        Debug.Log("Restarting");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    
    //For Main Menu --- --- --- ---

    //Starts The First Level
    public void StartLevel()
    {
        Debug.Log("Starting First Level");
        SceneManager.LoadScene("Level0");

    }

    //Quits Game
    public void Quit()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }


}
