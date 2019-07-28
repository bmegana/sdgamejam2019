using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingAxe : MonoBehaviour
{
    public Rigidbody rb;
    
    public float swingSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        swingSpeed += 1;

        if (transform.rotation.x >= 90f)
        {
            swingSpeed += -1;
        }
        else if (transform.rotation.x <= -90f)
        {
            swingSpeed += 1;
        }
        Quaternion rotation = Quaternion.Euler(swingSpeed, 0, 0);
        transform.rotation = rotation;
    }
}
