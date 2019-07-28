using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryArea : MonoBehaviour
{
	private void OnTriggerEnter( Collider other )
	{
		LineLeader leader = LineLeader.GetActiveLeader();
		if ( leader != null && leader.gameObject == other.gameObject )
		{
			SceneManager.LoadSceneAsync( "Ending Scene" );
		}
	}
}
