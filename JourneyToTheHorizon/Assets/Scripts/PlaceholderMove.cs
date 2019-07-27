using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderMove : MonoBehaviour
{
    [SerializeField] private Vector3 speed = new Vector3( 7.5f, 0.0f, 7.5f );

    private Rigidbody moverRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        moverRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float xMove = Input.GetAxis( "Horizontal" );
        float zMove = Input.GetAxis( "Vertical" );

        Vector3 currSpeed = new Vector3( speed.x * xMove, 0.0f, speed.z * zMove );
        if ( moverRigidbody != null )
        {
            moverRigidbody.velocity = currSpeed;
        }
        
    }
}
