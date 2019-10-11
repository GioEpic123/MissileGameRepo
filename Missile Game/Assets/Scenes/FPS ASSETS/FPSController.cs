using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    //Makes "Speed" A thing
    public float speed = 2f;
    //Makes "Sensitivity" A thing
    public float sensitivity = 2f;
    //Makes This a controller
    CharacterController player;
    //references the camera as "eyes"
    public GameObject eyes;

    //References Movement here
    private float moveLR, moveFB, rotx, roty, vertVelocity;

    private bool hasJumped;
    private bool isCrouched;

    void Start()
    {
        player = GetComponent<CharacterController>();

    }
    void Update()
    {
        Movement();
        if (Input.GetButtonDown("Jump"))
        {
            hasJumped = true;

        }

        if (Input.GetButtonDown("Crouch"))
        {
            if (isCrouched == false)
            {
                player.height = player.height / 2;
                isCrouched = true;
            }
            else
            {
                player.height = player.height * 2;
                isCrouched = false;
            }
        }
        ApplyGravity();
    }
   

    void Movement()
    {
        moveFB = Input.GetAxis("Vertical") * speed;
        moveLR = Input.GetAxis("Horizontal") *-1 * speed;

        rotx = Input.GetAxis("Mouse X") * sensitivity;
        roty -= Input.GetAxis("Mouse Y") * sensitivity;
        roty = Mathf.Clamp(roty, -60f, 60f);

        Vector3 movement = new Vector3(moveFB, vertVelocity, moveLR);
        transform.Rotate(0, rotx, 0);
        eyes.transform.localRotation = Quaternion.Euler(roty, 90, 0);

        movement = transform.rotation * movement;
        player.Move (movement * Time.deltaTime);


    }
    //Up there was first movement, down here is changes to code:

    public float jumpForce = 4f;

    //Jumps, then lets system know it's jumped
   

    //Makes Gravity Work
    private void ApplyGravity()
    {
        if (player.isGrounded == true)
        {
            if (hasJumped == false)
            {
                vertVelocity = Physics.gravity.y;
            }
            else
            {
                vertVelocity = jumpForce;
            }
        }
        else
        {
            vertVelocity += Physics.gravity.y * Time.deltaTime;
            vertVelocity = Mathf.Clamp(vertVelocity, -50f, jumpForce);
            hasJumped = false;
        }
    }

}
