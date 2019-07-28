using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFollower : MonoBehaviour
{
	[SerializeField] private float speed = 30.0f;
	[SerializeField] private float maxOffset = 2.0f;
	[SerializeField] private float angularSpeed = 360.0f;
	[SerializeField] private float distanceTolerance = 0.1f;
	[SerializeField] private float elevationDespawnCutoff = 28.0f;

	[SerializeField] private float rightOffset = 0.0f;
	private Rigidbody rb;

	private static List<LineFollower> followerRoster = new List<LineFollower>();

    // Start is called before the first frame update
    void Start()
    {
		followerRoster.Add( this );
		rightOffset = Random.Range( -maxOffset, maxOffset );
		rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		LineLeader leader = LineLeader.GetActiveLeader();
        if ( leader != null && leader.gameObject != gameObject )
        {
			Vector3 targetPos = LineLeader.GetFollowerLocation( GetFollowerIndex(), rightOffset );
			targetPos.y = transform.position.y;
			float distance = Vector3.Distance( targetPos, transform.position );
			if ( distance < distanceTolerance )
			{
				Vector3 vel = Vector3.zero;
				vel.y = rb.velocity.y;
				rb.velocity = vel;
			}
			else
			{
				float currSpeed = Mathf.Min( distance / Time.deltaTime, speed );
				Vector3 direction = targetPos - transform.position;
				direction.y = 0;
				direction.Normalize();
				direction *= currSpeed;
				direction.y = rb.velocity.y;
				rb.velocity = direction;

				Quaternion targetRotation = Quaternion.LookRotation( direction, Vector3.up );
				Quaternion newRotation = Quaternion.RotateTowards( transform.rotation, targetRotation, angularSpeed * Time.deltaTime );

				transform.rotation = newRotation;
			}
        }

		if ( transform.position.y < elevationDespawnCutoff )
		{
			Destroy( gameObject );
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
