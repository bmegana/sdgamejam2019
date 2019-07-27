using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFollower : MonoBehaviour
{
    [SerializeField] private int followerIdx;
    [SerializeField] LineLeader leader;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( leader != null )
        {
            transform.position = leader.GetFollowerLocation( followerIdx );
        }
    }
}
