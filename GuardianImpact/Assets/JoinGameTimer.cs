using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class JoinGameTimer : MonoBehaviour
{
    public static JoinGameTimer master;
    public TMP_Text timerText;
    public float timer = 1;
    public bool timerOn = false;
    private void Awake()
    {
        if (master != null) Destroy(this);
        master = this;
    }
    void Update()
    {
        timerText.text = string.Format("{00}", (int)timer);
        
        if (timer <= 99 && timerOn) 
        {
            timer += Time.deltaTime;
        }

        if(timer > 99)
        {
            timer = 0;
        }


        if (!timerOn)
        {
            timer = 0;
        }
    }

}
