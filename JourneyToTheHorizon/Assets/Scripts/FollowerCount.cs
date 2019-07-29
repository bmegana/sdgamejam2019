using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowerCount : MonoBehaviour
{
	private Text followerCount;

    // Start is called before the first frame update
    void Start()
    {
		followerCount = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
		followerCount.text = "Followers:\n" + ( LineLeader.GetLeaderRosterSize() - 1 );
    }
}
