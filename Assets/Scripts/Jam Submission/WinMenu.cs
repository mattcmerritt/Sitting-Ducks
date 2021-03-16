using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public void BackToMenu ()
    {
        UnityEngine.Debug.Log("menu");
        // scene 0 is the menu, all the levels should be scenes 1-10
        SceneManager.LoadScene(0);
    }
}
