using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class JoinGameTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public float timer = 1;
    StartGameManager startGameManager;
    private void Start()
    {
        startGameManager = GetComponent<StartGameManager>();
    }
    void Update()
    {
        timerText.text = string.Format("{00}", (int)timer);
        
        if (timer < 99 && startGameManager.timerOn) 
        {
            timer += Time.deltaTime;
        }

        if(timer >= 99)
        {
            timer = 0;
        }


        if (!startGameManager.timerOn)
        {
            timer = 0;
        }
    }

}
