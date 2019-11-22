using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Makes "Speed" A thing
    public float speed = 2f;
    //Makes "Sensitivity" A thing
    public float sensitivity = 2f;

    //references the camera as "eyes"
    public GameObject eyes, barrel;

    //Just To make Sure game isn't Paused
    public GameManager gameManager;

    //Clamp Declaration
    public float upClamp = 40f;
    public float downClamp = 100f;

    //References Movement here
    private float rotx, roty, gunRoty, vertVelocity;

    void Update()
    {
        Movement();
    }

    private void Start()
    {
        soundEnabled = GameManager.Instance.soundEnabled;
    }

    public void write()
    {
        GameManager.Instance.soundEnabled = soundEnabled;
    }

    public GameObject mainCam;
    public bool soundEnabled = true;
    //Maybe make a global sound handler...? If not, then the player is gonna have to deal with it :/
    void toggleSound()
    {
        if (soundEnabled)
        {
            mainCam.GetComponent<AudioListener>().enabled = false;
            soundEnabled = false;
        }
        else
        {
            mainCam.GetComponent<AudioListener>().enabled = false;
            soundEnabled = true;
        }
    }


    void Movement()
    {
        //Checks to make sure game isn't paused first
        if (!gameManager.gamePaused) 
        {
            
            //Rotation inputted, and clamped between two values
            rotx = Input.GetAxis("Mouse X") * sensitivity;
            roty -= Input.GetAxis("Mouse Y") * sensitivity;
            roty = Mathf.Clamp(roty, -downClamp, upClamp);

            if (!gameManager.gameHasEnded)
            {
                //rotates actual character about the X axis if game is active
                transform.Rotate(0, rotx, 0);
            }
            else
            {
                eyes.transform.Rotate(0, rotx, 0);
            }

            //rotates the camera "eyes" about Z axis(up & down)
            eyes.transform.localRotation = Quaternion.Euler(roty, 90, 0);

            //rotates barrel up and down,
            barrel.transform.localRotation = Quaternion.Euler(0, 0, -roty);
        }
    }



    
}
