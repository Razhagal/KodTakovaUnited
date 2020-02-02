using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouLostScene : MonoBehaviour
{
    public void OnRestart()
    {
        SceneManager.LoadScene("Main");
    }
}
