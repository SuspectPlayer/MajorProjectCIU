using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAcrossScenes : MonoBehaviour
{
    public static SaveAcrossScenes master;
    private void Awake()
    {
        if (master != null) Destroy(this.gameObject);
        master = this;
    }
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
