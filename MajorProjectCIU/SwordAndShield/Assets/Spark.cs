using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : MonoBehaviour
{
    public GameObject spark;
    public GameObject lastObj;
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.gameObject.name != "NewDummy")
        {
            if (lastObj != null)
            {
                Destroy(lastObj);
            }
            else
            {
                lastObj = (GameObject)Instantiate(spark, transform.position, Quaternion.identity);
                Destroy(lastObj, 1);
            }
        }

        Debug.Log("Hit" + col.gameObject.name);
    }

}
