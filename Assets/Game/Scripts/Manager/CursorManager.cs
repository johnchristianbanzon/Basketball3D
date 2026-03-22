using System;
using UnityEngine;

public class CursorManager : MonoBehaviour, ICursorManager
{
    public void DisableCursor()
    {
        Cursor.visible = false;
        // Optionally, lock the cursor to the center of the game window
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EnableCursor()
    {
        Cursor.visible = true;
        // Optionally, lock the cursor to the center of the game window
        Cursor.lockState = CursorLockMode.None;
    }


    // You can add an Update method to toggle visibility (e.g., for a pause menu)

}