using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashTimer : MonoBehaviour
{
    public float sceneChangeTimer = 5f;
    public int sceneIndexNumber;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDown());
    }

    // Update is called once per frame
    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(sceneChangeTimer);
        SceneManager.LoadScene(sceneIndexNumber);
    }
}
