using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    //Start, Update
    //Update strobes the eye from red to black, and disables/enables a light within it. No light on actual GO, just the eye.
    public GameObject explodeRadius;
    public void Explode()
    {
        GameObject rad = Instantiate(explodeRadius);
        rad.transform.position = gameObject.transform.position;
    }


}
