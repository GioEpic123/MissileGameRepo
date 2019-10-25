using UnityEngine;

public class mouseLOCK : MonoBehaviour
{
    public GameManager gameManager;
    bool CursorLockedVar;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
        CursorLockedVar = (true);
        
    }

    void Update()
    {
        if (Input.GetKeyDown("escape") && !CursorLockedVar)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = (false);
            CursorLockedVar = (true);
            gameManager.Paused();
            Debug.Log("Game Resumed");
            
        }
        else if (Input.GetKeyDown("escape") && CursorLockedVar)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
            CursorLockedVar = (false);
            gameManager.Paused();
            Debug.Log("Game Paused");
        }
    }
}