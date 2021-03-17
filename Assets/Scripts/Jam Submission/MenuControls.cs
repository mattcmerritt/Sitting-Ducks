using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    public void QuitGame ()
    {
        UnityEngine.Debug.Log("QUIT");
        Application.Quit();
    }

    public void LoadLevel (int level)
    {
        //UnityEngine.Debug.Log(level);
        // scene 0 is the menu, all the levels should be scenes 1-10
        SceneManager.LoadScene(level);
    }
}
