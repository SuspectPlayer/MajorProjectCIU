using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    public Animator loginPanel;
    private void Update()
    {
        if (PlayfabManager.master.IsLoggedIn())
        {
            Invoke("AnimateLogin", 3);
        }
        else
        {
            Invoke("AnimateLogout", 3);
        }
    }

    public void AnimateLogin()
    {
        if (!loginPanel.GetBool("LoggedIn"))
        {
            loginPanel.SetBool("LoggedIn", true);
        }

       
    }

    public void AnimateLogout()
    {
       
            if (loginPanel.GetBool("LoggedIn"))
            {
                loginPanel.SetBool("LoggedIn", false);
            }
        
    }
}
