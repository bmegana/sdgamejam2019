using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFollower : MonoBehaviour
{
	[SerializeField] private float speed = 30.0f;
	[SerializeField] private float maxOffset = 2.0f;
	[SerializeField] private float angularSpeed = 360.0f;

	[SerializeField] float rightOffset = 0.0f;

	private static List<LineFollower> followerRoster = new List<LineFollower>();

    // Start is called before the first frame update
    void Start()
    {
		followerRoster.Add( this );
		rightOffset = Random.Range( -maxOffset, maxOffset );
    }

    // Update is called once per frame
    void Update()
    {
		LineLeader leader = LineLeader.GetActiveLeader();
        if ( leader != null && leader.gameObject != gameObject )
        {
			Vector3 newPos = Vector3.MoveTowards( transform.position, LineLeader.GetFollowerLocation( GetFollowerIndex(), rightOffset ), speed * Time.deltaTime );
			Vector3 newDir = newPos - transform.position;
			if ( newDir.magnitude > 0 )
			{
				newDir.Normalize();
				Quaternion targetRotation = Quaternion.LookRotation( newDir, Vector3.up );
				Quaternion newRotation = Quaternion.RotateTowards( transform.rotation, targetRotation, angularSpeed * Time.deltaTime );

				transform.rotation = newRotation;
			}

			transform.position = newPos;

        }
    }

	private void OnDestroy()
	{
		followerRoster.Remove( this );
	}

	private int GetFollowerIndex()
	{
		LineLeader leader = LineLeader.GetActiveLeader();
		if ( leader != null && leader.gameObject == gameObject )
		{
			return followerRoster.Count;
		}

		int leaderIdx = followerRoster.Count;
		for ( int idx = 0; idx < followerRoster.Count; idx++ )
		{
			LineFollower follower = followerRoster[idx];
			if ( leader != null && follower.gameObject == leader.gameObject )
			{
				leaderIdx = idx;
			}

			if ( follower == this )
			{
				return idx <= leaderIdx ? idx : idx - 1;
			}
		}

		return followerRoster.Count;
	}
}
