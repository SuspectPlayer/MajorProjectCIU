using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public GameObject MaleCamera;
    public GameObject FemaleCamera;

    private void Start()
    {
        if (FemaleCamera)
        {
            FemaleCamera.SetActive(false);
        }
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if(hit.transform.name == "MaleBox")
                {
                    FemaleCamera.SetActive(false);
                }
                if(hit.transform.name == "FemaleBox")
                {
                    FemaleCamera.SetActive(true);
                }
                Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
                
           }
        }

    }
}
