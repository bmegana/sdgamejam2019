using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFollower : MonoBehaviour
{
    [SerializeField] private int followerIdx;
    [SerializeField] LineLeader leader;

	[SerializeField] float rightOffset = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
		rightOffset = Random.Range( -2.0f, 2.0f );
    }

    // Update is called once per frame
    void Update()
    {
        if ( leader != null )
        {
            transform.position = leader.GetFollowerLocation( followerIdx, rightOffset );
        }
    }
}
