using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherWater : MonoBehaviour
{
	bool bHaveWater = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if ( Input.GetAxis( "Submit" ) > 0 )
		{
			RaycastHit hit;
			if ( Physics.SphereCast( transform.position, 0.5f, transform.forward, out hit, maxDistance:1.0f ) )
			{
				if ( hit.collider.gameObject.tag == "Water" )
				{
					Debug.Log( "Found Water!" );
				}
			}
		}
    }
}
