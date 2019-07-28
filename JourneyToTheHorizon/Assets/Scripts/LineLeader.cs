using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LineLeader : MonoBehaviour
{
    public AudioSource playerDeath;
    [SerializeField] private float targetPathLength = 20.0f;
    [SerializeField] private float followerPadding = 0.5f;
    [SerializeField] private float waypointDist = 0.25f;
    [SerializeField] private GameObject debugWaypointPrefab;
    [SerializeField] private bool overrideLeader;

    private List<Vector3> leaderPath = new List<Vector3>();
    private List<GameObject> debugWaypointMarkers = new List<GameObject>();
    private float totalLength;
    private float lastSegmentLength;

	private static List<LineLeader> leaderRoster = new List<LineLeader>();

    // Start is called before the first frame update
    void Start()
    {
        playerDeath = GetComponent<AudioSource>();
        ResetPath();

		if ( overrideLeader )
		{
			leaderRoster.Insert( 0, this );
			overrideLeader = false;
		}
		else
		{
			leaderRoster.Add( this );
		}

		if ( leaderRoster[0] != this )
		{
			PlayerMovement moveComp = GetComponent<PlayerMovement>();
			if ( moveComp != null )
			{
				moveComp.enabled = false;
				moveComp.cameraTransform.gameObject.SetActive( false );
			}
		}
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

	private void OnDestroy()
	{

		if ( leaderRoster[0] == this )
		{
			if ( leaderRoster.Count > 1 )
			{
				// Transfer control to next in line
				LineLeader successor = leaderRoster[1];
				successor.playerDeath.Play();
				PlayerMovement moveComp = successor.gameObject.GetComponent<PlayerMovement>();
				if ( moveComp != null )
				{
					moveComp.enabled = true;
					moveComp.cameraTransform.gameObject.SetActive( true );
					moveComp.ResetCamera();
				}
			}
			else
			{
				// GAME OVER
#if UNITY_EDITOR
				EditorApplication.isPlaying = false;
#else
				SceneManager.LoadSceneAsync( "MainMenu" );
#endif
			}
		}
		else
		{
			leaderRoster[0].playerDeath.Play();
		}

		leaderRoster.Remove( this );
	}

	private void ResetPath()
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

	public static LineLeader GetActiveLeader()
	{
		if ( leaderRoster.Count > 0 )
		{
			return leaderRoster[0];
		}
		else
		{
			return null;
		}
	}

	public static Vector3 GetFollowerLocation( int followerIdx, float rightOffset )
	{
		LineLeader leader = GetActiveLeader();
		if ( leader == null )
		{
			return Vector3.zero;
		}

		if ( leader.leaderPath.Count == 0 )
		{
			return leader.transform.position + leader.transform.right * rightOffset;
		}

		float targetDist = ( followerIdx + 1 ) * leader.followerPadding;
		float totalDistAtCurrWaypoint = Vector3.Distance( leader.transform.position, leader.leaderPath[0] );
		if ( totalDistAtCurrWaypoint >= targetDist )
		{
			Vector3 result = Vector3.Lerp( leader.transform.position, leader.leaderPath[0], targetDist / totalDistAtCurrWaypoint );
			Vector3 diff = leader.transform.position - leader.leaderPath[0];
			result += Vector3.Cross( Vector3.up, diff.normalized ) * rightOffset;
			return result;
		}

		for ( int currWayPoint = 0; currWayPoint < leader.leaderPath.Count - 1; currWayPoint++ )
		{
			float currSegmentLength = Vector3.Distance( leader.leaderPath[currWayPoint], leader.leaderPath[currWayPoint + 1] );
			if ( totalDistAtCurrWaypoint + currSegmentLength > targetDist )
			{
				Vector3 result = Vector3.Lerp( leader.leaderPath[currWayPoint], leader.leaderPath[currWayPoint + 1], ( targetDist - totalDistAtCurrWaypoint ) / currSegmentLength );
				Vector3 diff = leader.leaderPath[currWayPoint] - leader.leaderPath[currWayPoint + 1];
				result += Vector3.Cross( Vector3.up, diff.normalized ) * rightOffset;
				return result;
			}
			totalDistAtCurrWaypoint += currSegmentLength;
		}

		Vector3 finalResult;
		finalResult = leader.leaderPath[leader.leaderPath.Count - 1];
		if ( leader.leaderPath.Count > 1 )
		{
			Vector3 diff = leader.leaderPath[leader.leaderPath.Count - 2] - leader.leaderPath[leader.leaderPath.Count - 1];
			finalResult += Vector3.Cross( Vector3.up, diff.normalized ) * rightOffset;
		}
		else
		{
			Vector3 diff = leader.transform.position - leader.leaderPath[0];
			finalResult += Vector3.Cross( Vector3.up, diff.normalized ) * rightOffset;
		}
		return leader.leaderPath[leader.leaderPath.Count - 1];
	}
}
