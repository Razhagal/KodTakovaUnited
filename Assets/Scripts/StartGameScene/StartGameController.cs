using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameController : MonoBehaviour
{
    public void OnClickStartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
