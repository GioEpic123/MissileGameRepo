using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Makes "Speed" A thing
    public float speed = 2f;
    //Makes "Sensitivity" A thing
    public float sensitivity = 2f;
    //Makes This a controller

    //references the camera as "eyes"
    public GameObject eyes, barrel;
    //...and the gun as Gun
    

    //Clamp Declaration
    public float upClamp = 30f;
    public float downClamp = 30f;

    //References Movement here
    private float rotx, roty, gunRoty, vertVelocity;

    private bool hasJumped;
    private bool isCrouched;

    float getRoty()
    {
        return roty;
    }

    void Start()
    {
       
    }
    void Update()
    {
        Movement();
    }


    void Movement()
    {
 
        //Rotation inputted, and clamped between two values
        rotx = Input.GetAxis("Mouse X") * sensitivity;
        roty -= Input.GetAxis("Mouse Y") * sensitivity;
        roty = Mathf.Clamp(roty, -upClamp, downClamp);

        //Rotation strictly for the Gun Object
        gunRoty = roty;
        gunRoty = Mathf.Clamp(gunRoty, 0, 90f);

        //rotates actual character about the X axis
        transform.Rotate(0, rotx, 0);
        //rotates the camera "eyes" about Z axis(up & down)
        eyes.transform.localRotation = Quaternion.Euler(roty, 90, 0);

        //rotates barrel up and down,
        barrel.transform.localRotation = Quaternion.Euler(0, 0, -roty);
        


    }



    
}
