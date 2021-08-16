using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerNameReader : MonoBehaviour
{
    public TMP_InputField inputText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string text = inputText.GetComponent<TMP_InputField>().text;
        if (text != null)
        {
            gameObject.GetComponent<TMP_Text>().text = ""+text;
        }
        else
        {
            gameObject.GetComponent<TMP_Text>().text = "Player";
        }
    }
}
