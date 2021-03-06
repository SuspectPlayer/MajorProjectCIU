using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public int sceneNumber;
    public KeyCode key;
    public AudioClip clickSound;

    void Update()
    {
        if (Input.GetKey(key))
        {
            AudioManager.master.ClickButtonSound();
            LoadScene();
        }
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
