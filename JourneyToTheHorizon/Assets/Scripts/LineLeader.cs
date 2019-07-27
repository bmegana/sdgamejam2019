using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineLeader : MonoBehaviour
{
    [SerializeField] private float targetPathLength = 20.0f;
    [SerializeField] private float followerPadding = 0.5f;
    [SerializeField] private float waypointDist = 0.25f;
    [SerializeField] private GameObject debugWaypointPrefab; 

    private List<Vector3> leaderPath = new List<Vector3>();
    private List<GameObject> debugWaypointMarkers = new List<GameObject>();
    private float totalLength;
    private float lastSegmentLength;

    // Start is called before the first frame update
    void Start()
    {
        ResetPath();
    }

    // Update is called once per frame
    void Update()
    {
        if ( leaderPath.Count > 0 && Vector3.Distance( transform.position, leaderPath[0] ) >= waypointDist )
        {
            PushCurrentLocation();
        }
    }

    private void PushCurrentLocation()
    {
        if ( debugWaypointPrefab != null )
        {
            debugWaypointMarkers.Insert( 0, Instantiate( debugWaypointPrefab, transform.position, Quaternion.identity ) );
        }

        float firstSegmentLength = leaderPath.Count > 0 ? Vector3.Distance( transform.position, leaderPath[0] ) : 0;
        leaderPath.Insert( 0, transform.position );
        totalLength += firstSegmentLength;
        if ( totalLength - lastSegmentLength >= targetPathLength && leaderPath.Count >= 3 )
        {
            leaderPath.RemoveAt( leaderPath.Count - 1 );
            lastSegmentLength = Vector3.Distance( leaderPath[leaderPath.Count - 1], leaderPath[leaderPath.Count - 2] );

            if ( debugWaypointMarkers.Count > 0 )
            {
                Destroy( debugWaypointMarkers[debugWaypointMarkers.Count - 1] );
                debugWaypointMarkers.RemoveAt( debugWaypointMarkers.Count - 1 );
            }
        }
        else if ( leaderPath.Count == 2 )
        {
            lastSegmentLength = Vector3.Distance( leaderPath[leaderPath.Count - 1], leaderPath[leaderPath.Count - 2] );
        }
    }

    public void ResetPath()
    {
        leaderPath.Clear();
        totalLength = 0;
        lastSegmentLength = 0;

        if ( debugWaypointMarkers.Count > 0 )
        {
            foreach ( GameObject marker in debugWaypointMarkers )
            {
                Destroy( marker );
            }
            debugWaypointMarkers.Clear();
        }

        PushCurrentLocation();
    }

    public Vector3 GetFollowerLocation( int followerIdx, float rightOffset )
    {
        if ( leaderPath.Count == 0 )
        {
            return transform.position + transform.right * rightOffset;
        }

        float targetDist = ( followerIdx + 1 ) * followerPadding;
        float totalDistAtCurrWaypoint = Vector3.Distance( transform.position, leaderPath[0] );
        if ( totalDistAtCurrWaypoint >= targetDist )
        {
			Vector3 result = Vector3.Lerp( transform.position, leaderPath[0], targetDist / totalDistAtCurrWaypoint );
			Vector3 diff = transform.position - leaderPath[0];
			result += Vector3.Cross( Vector3.up, diff.normalized ) * rightOffset;
			return result;
        }

        for ( int currWayPoint = 0; currWayPoint < leaderPath.Count - 1; currWayPoint++ )
        {
            float currSegmentLength = Vector3.Distance( leaderPath[currWayPoint], leaderPath[currWayPoint + 1] );
            if ( totalDistAtCurrWaypoint + currSegmentLength > targetDist )
            {
				Vector3 result = Vector3.Lerp( leaderPath[currWayPoint], leaderPath[currWayPoint + 1], ( targetDist - totalDistAtCurrWaypoint ) / currSegmentLength );
				Vector3 diff = leaderPath[currWayPoint] - leaderPath[currWayPoint + 1];
				result += Vector3.Cross( Vector3.up, diff.normalized ) * rightOffset;
				return result;
			}
            totalDistAtCurrWaypoint += currSegmentLength;
        }

		Vector3 finalResult;
		finalResult = leaderPath[leaderPath.Count - 1];
		if ( leaderPath.Count > 1 )
		{
			Vector3 diff = leaderPath[leaderPath.Count - 2] - leaderPath[leaderPath.Count - 1];
			finalResult += Vector3.Cross( Vector3.up, diff.normalized ) * rightOffset;
		}
		else
		{
			Vector3 diff = transform.position - leaderPath[0];
			finalResult += Vector3.Cross( Vector3.up, diff.normalized ) * rightOffset;
		}
        return leaderPath[leaderPath.Count - 1];
    }
}
