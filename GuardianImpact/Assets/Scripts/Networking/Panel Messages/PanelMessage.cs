using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelMessage : MonoBehaviour
{
    float timer = 0f;
    [SerializeField] float destroyTime = 4f;
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= destroyTime) Destroy(gameObject);
        timer += Time.deltaTime;
    }
    public void SetText(string text)
    {
        this.text.text = text;
    }
}
