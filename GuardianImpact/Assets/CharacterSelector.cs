using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterSelector : MonoBehaviourPun
{
    public GameObject MaleCamera;
    public GameObject FemaleCamera;
    public static CharacterSelector master;

    private void Awake()
    {
        if (master != null) Destroy(this);
        master = this;
    }
    private void Start()
    {
        if (FemaleCamera)
        {
            FemaleCamera.SetActive(false);
        }
    }
    public void SetMaleCharacter()
    {
        ExitGames.Client.Photon.Hashtable newHash = new ExitGames.Client.Photon.Hashtable();
        newHash.Add("i", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(newHash);
        FemaleCamera.SetActive(false);
    }
    public void SetFemaleCharacter()
    {
        ExitGames.Client.Photon.Hashtable newHash = new ExitGames.Client.Photon.Hashtable();
        newHash.Add("i", 1);
        PhotonNetwork.LocalPlayer.SetCustomProperties(newHash);
        FemaleCamera.SetActive(true);
    }
    private void Update()
    {
        if (!PhotonManager.master.IsConnectedToServer()) return;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if(hit.transform.name == "MaleBox")
                {
                    SetMaleCharacter();
                }
                if(hit.transform.name == "FemaleBox")
                {
                    SetFemaleCharacter();
                }
                //Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
                
           }
        }

    }
}
