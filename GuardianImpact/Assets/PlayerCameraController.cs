using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
   
    int rotationPower;
    int aimValue;
    Vector3 _look;
    Vector3 _move;
    Vector3 angles;
    Vector3 nextPosition;
    Quaternion nextRotation;

    public GameObject followTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);

        nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime);

        if(_move.x == 0 && _move.y == 0)
        {
            nextPosition = transform.position;

            if(aimValue == 1)
            {
                transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
                followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            }
        }
    }
}
