using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameStarter : MonoBehaviour
{
    public void OnClick_Start()
    {
        SceneManager.LoadScene("Scene_Play");
    }
}
