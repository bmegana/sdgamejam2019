//#define LIMIT_TERRAIN_SPAWNING
//#define USING_NAVMESH

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TerrainSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] terrainPrefabs;
    [SerializeField] private GameObject destinationPrefab;
    [SerializeField] private Vector3 centerAdjust = new Vector3( 50.0f, 0.0f, 50.0f );
    [SerializeField] private Vector3 spawnExtents = new Vector3( 40.0f, 0.0f, 40.0f );
    [SerializeField] private Vector3 spawnLimits = new Vector3( 50.0f, 0.0f, 50.0f );
    [SerializeField] private Vector3 spawnOffset = new Vector3( 100.0f, 0.0f, 100.0f );
    [SerializeField] private float despawnDist = 200.0f;
    [SerializeField] private bool enableSpawn = true;
    [SerializeField] private Vector3 debugPlayerDist = new Vector3();
    [SerializeField] private float borderWidth = 0.5f;
	[SerializeField] private int spawnCountdown = 50;
	[SerializeField] private float earlySpawnChance = 0.025f;

    // Going clockwise (looking towards -Y) starting at +X
    [SerializeField] private TerrainSpawner[] terrainNeighbors = { null, null, null, null, null, null, null, null };
#if USING_NAVMESH
	[SerializeField] private NavMeshLink[] neighborLink = { null, null, null, null };
#endif

	private static int spawnCount = 0;

	// Start is called before the first frame update
	void Start()
    {
		spawnCount++;
    }

    // Update is called once per frame
    void Update()
    {
		LineLeader leader = LineLeader.GetActiveLeader();
        if ( leader != null )
        {
            Vector3 vPlayerDistance = leader.transform.position - transform.position - centerAdjust;
            debugPlayerDist = vPlayerDistance;
            if ( vPlayerDistance.magnitude > despawnDist )
            {
                Destroy( gameObject );
            }
            else if ( terrainPrefabs.Length > 0 && enableSpawn && Mathf.Abs( vPlayerDistance.x ) < spawnLimits.x && Mathf.Abs( vPlayerDistance.z ) < spawnLimits.z )
            {
                float sameX, sameY, sameZ, posX, posZ, negX, negZ;
                sameX = transform.position.x;
                sameY = transform.position.y;
                sameZ = transform.position.z;
                posX = transform.position.x + spawnOffset.x;
                posZ = transform.position.z + spawnOffset.z;
                negX = transform.position.x - spawnOffset.x;
                negZ = transform.position.z - spawnOffset.z;

                if ( vPlayerDistance.x > spawnExtents.x )
                {
#if !LIMIT_TERRAIN_SPAWNING
					if ( terrainNeighbors[7] == null )
                    {
                        SpawnNeighbor( 7, new Vector3( posX, sameY, posZ ) );
                    }
#endif
                    if ( terrainNeighbors[0] == null )
                    {
                        SpawnNeighbor( 0, new Vector3( posX, sameY, sameZ ) );
					}
#if !LIMIT_TERRAIN_SPAWNING
                    if ( terrainNeighbors[1] == null )
                    {
                        SpawnNeighbor( 1, new Vector3( posX, sameY, negZ ) );
                    }
#endif
                }
                else if ( vPlayerDistance.x < -spawnExtents.x )
                {

#if !LIMIT_TERRAIN_SPAWNING
                    if ( terrainNeighbors[3] == null )
                    {
                        SpawnNeighbor( 3, new Vector3( negX, sameY, negZ ) );
                    }
#endif
                    if ( terrainNeighbors[4] == null )
                    {
                        SpawnNeighbor( 4, new Vector3( negX, sameY, sameZ ) );
                    }

#if !LIMIT_TERRAIN_SPAWNING
                    if ( terrainNeighbors[5] == null )
                    {
                        SpawnNeighbor( 5, new Vector3( negX, sameY, posZ ) );
                    }
#endif
                }

                if ( vPlayerDistance.z > spawnExtents.z )
                {

#if !LIMIT_TERRAIN_SPAWNING
                    if ( terrainNeighbors[5] == null )
                    {
                        SpawnNeighbor( 5, new Vector3( negX, sameY, posZ ) );
                    }
#endif
                    if ( terrainNeighbors[6] == null )
                    {
                        SpawnNeighbor( 6, new Vector3( sameX, sameY, posZ ) );
                    }
#if !LIMIT_TERRAIN_SPAWNING
                    if ( terrainNeighbors[7] == null )
                    {
                        SpawnNeighbor( 7, new Vector3( posX, sameY, posZ ) );
                    }
#endif
                }
                else if ( vPlayerDistance.z < -spawnExtents.z )
                {
#if !LIMIT_TERRAIN_SPAWNING
                    if ( terrainNeighbors[1] == null )
                    {
                        SpawnNeighbor( 1, new Vector3( posX, sameY, negZ ) );
                    }
#endif
                    if ( terrainNeighbors[2] == null )
                    {
                        SpawnNeighbor( 2, new Vector3( sameX, sameY, negZ ) );
                    }
#if !LIMIT_TERRAIN_SPAWNING
                    if ( terrainNeighbors[3] == null )
                    {
                        SpawnNeighbor( 3, new Vector3( negX, sameY, negZ ) );
                    }
#endif
                }
            }
        }
    }

    private void OnDestroy()
    {
        for ( int idx = 0; idx < 8; idx++ )
        {
            if ( terrainNeighbors[idx] != null )
            {
                int idxInNeighbor = ( idx + 4 ) % 8;
                terrainNeighbors[idx].terrainNeighbors[idxInNeighbor] = null;
#if USING_NAVMESH
				if ( idx % 2 == 0 && terrainNeighbors[idx].neighborLink[idxInNeighbor / 2] != null )
				{
					Destroy( terrainNeighbors[idx].neighborLink[idxInNeighbor / 2] );
					terrainNeighbors[idx].neighborLink[idxInNeighbor / 2] = null;
				}
#endif

				terrainNeighbors[idx] = null;
#if USING_NAVMESH
				if ( idx % 2 == 0 && neighborLink[idx / 2] != null )
				{
					Destroy( neighborLink[idx / 2] );
					neighborLink[idx / 2] = null;
				}
#endif
			}
        }
    }

    private void SpawnNeighbor( int idx, Vector3 pos )
    {
		if ( destinationPrefab == null && terrainPrefabs.Length == 0 )
		{
			return;
		}

		bool spawnDestination = destinationPrefab && ( spawnCount >= spawnCountdown || Random.Range( 0.0f, 1.0f ) < earlySpawnChance );

		GameObject prefab = spawnDestination ? destinationPrefab : terrainPrefabs[Random.Range( 0, terrainPrefabs.Length )];
		if ( spawnCountdown > 0 )
		{
			spawnCount = spawnCount % spawnCountdown;
		}

		GameObject neighbor = Instantiate( prefab, pos, Quaternion.identity );
		TerrainSpawner neighborSpawner = neighbor.GetComponent<TerrainSpawner>();
        if ( neighborSpawner == null )
        {
			neighborSpawner = neighbor.AddComponent<TerrainSpawner>();
        }
		neighborSpawner.terrainNeighbors = new TerrainSpawner[8];
        for ( int neighborIdx = 0; neighborIdx < 8; neighborIdx++ )
        {
			neighborSpawner.terrainNeighbors[neighborIdx] = null;
        }
		CreateLink( idx, neighborSpawner );
		neighborSpawner.PopulateNeighbors();
    }

    private void PopulateNeighbors()
    {
        List<int> neighborsToCheck = new List<int>( 8 );
        for ( int idx = 0; idx < 8; idx++ )
        {
            if ( terrainNeighbors[idx] != null )
            {
                neighborsToCheck.Add( idx );
            }
        }

        for ( int idx = 0; idx < neighborsToCheck.Count; idx++ )
        {
            int neighborIdx = neighborsToCheck[idx];
            int myIdxInNeighbor = ( neighborIdx + 4 ) % 8;
            bool diagonalAdjacent = ( neighborIdx % 2 == 1 );
            if ( terrainNeighbors[neighborIdx].terrainNeighbors[( myIdxInNeighbor + 6 ) % 8] != null && !diagonalAdjacent )
            {
                int nextIdx = ( neighborIdx + 1 ) % 8;
                int nextIdxInNeighbor = ( myIdxInNeighbor + 6 ) % 8;
                if ( terrainNeighbors[nextIdx] != null && terrainNeighbors[nextIdx] != terrainNeighbors[neighborIdx].terrainNeighbors[nextIdxInNeighbor] )
                {
                    Debug.LogError( "Neighbor Mismatch!" );
                }
                CreateLink( nextIdx, terrainNeighbors[neighborIdx].terrainNeighbors[nextIdxInNeighbor] );

                if ( !neighborsToCheck.Contains( nextIdx ) )
                {
                    neighborsToCheck.Add( nextIdx );
                }
            }
            if ( terrainNeighbors[neighborIdx].terrainNeighbors[( myIdxInNeighbor + 7 ) % 8] != null )
            {
                int nextIdx = diagonalAdjacent ? ( neighborIdx + 1 ) % 8 : ( neighborIdx + 2 ) % 8;
                int nextIdxInNeighbor = ( myIdxInNeighbor + 7 ) % 8;
                if ( terrainNeighbors[nextIdx] != null && terrainNeighbors[nextIdx] != terrainNeighbors[neighborIdx].terrainNeighbors[nextIdxInNeighbor] )
                {
                    Debug.LogError( "Neighbor Mismatch!" );
                }
                CreateLink( nextIdx, terrainNeighbors[neighborIdx].terrainNeighbors[nextIdxInNeighbor] );

                if ( !neighborsToCheck.Contains( nextIdx ) )
                {
                    neighborsToCheck.Add( nextIdx );
                }
            }
            if ( terrainNeighbors[neighborIdx].terrainNeighbors[( myIdxInNeighbor + 1 ) % 8] != null )
            {
                int nextIdx = diagonalAdjacent ? ( neighborIdx + 7 ) % 8 : ( neighborIdx + 6 ) % 8;
                int nextIdxInNeighbor = ( myIdxInNeighbor + 1 ) % 8;
                if ( terrainNeighbors[nextIdx] != null && terrainNeighbors[nextIdx] != terrainNeighbors[neighborIdx].terrainNeighbors[nextIdxInNeighbor] )
                {
                    Debug.LogError( "Neighbor Mismatch!" );
                }
                CreateLink( nextIdx, terrainNeighbors[neighborIdx].terrainNeighbors[nextIdxInNeighbor] );

                if ( !neighborsToCheck.Contains( nextIdx ) )
                {
                    neighborsToCheck.Add( nextIdx );
                }
            }
            if ( terrainNeighbors[neighborIdx].terrainNeighbors[( myIdxInNeighbor + 2 ) % 8] != null && !diagonalAdjacent )
            {
                int nextIdx = ( neighborIdx + 7 ) % 8;
                int nextIdxInNeighbor = ( myIdxInNeighbor + 2 ) % 8;
                if ( terrainNeighbors[nextIdx] != null && terrainNeighbors[nextIdx] != terrainNeighbors[neighborIdx].terrainNeighbors[nextIdxInNeighbor] )
                {
                    Debug.LogError( "Neighbor Mismatch!" );
                }
                CreateLink( nextIdx, terrainNeighbors[neighborIdx].terrainNeighbors[nextIdxInNeighbor] );

                if ( !neighborsToCheck.Contains( nextIdx ) )
                {
                    neighborsToCheck.Add( nextIdx );
                }
            }
        }
    }

    private void CreateLink( int idx, TerrainSpawner neighbor )
    {
        int idxInNeighbor = ( idx + 4 ) % 8;
        terrainNeighbors[idx] = neighbor;
        terrainNeighbors[idx].terrainNeighbors[idxInNeighbor] = this;

#if USING_NAVMESH
		if ( idx % 2 == 0 )
        {
            int linkIdx = idx / 2;
            Vector3 start = centerAdjust;
            Vector3 end = centerAdjust;
			Vector3 neighborStart = centerAdjust;
            Vector3 neighborEnd = centerAdjust;
            float width = 0;
            switch ( linkIdx )
            {
                case 0:
                {
                    start.x += spawnLimits.x - borderWidth;
                    end.x += spawnLimits.x + borderWidth;
					neighborStart.x -= spawnLimits.x - borderWidth;
					neighborEnd.x -= spawnLimits.x + borderWidth;
					width = spawnOffset.z - borderWidth * 2;
                    break;
                }
                case 1:
                {
                    start.z -= spawnLimits.z - borderWidth;
                    end.z -= spawnLimits.z + borderWidth;
					neighborStart.z += spawnLimits.z - borderWidth;
					neighborEnd.z += spawnLimits.z + borderWidth;
					width = spawnOffset.x - borderWidth * 2;
                    break;
                }
                case 2:
                {
                    start.x -= spawnLimits.x - borderWidth;
                    end.x -= spawnLimits.x + borderWidth;
					neighborStart.x += spawnLimits.x - borderWidth;
					neighborEnd.x += spawnLimits.x + borderWidth;
					width = spawnOffset.z - borderWidth * 2;
                    break;
                }
                case 3:
                {
                    start.z += spawnLimits.z - borderWidth;
                    end.z += spawnLimits.z + borderWidth;
					neighborStart.z -= spawnLimits.z - borderWidth;
					neighborEnd.z -= spawnLimits.z + borderWidth;
					width = spawnOffset.x - borderWidth * 2;
                    break;
                }
                default:
                {
                    return;
                }
            }

			if ( neighborLink[linkIdx] != null )
			{
				Destroy( neighborLink[linkIdx] );
				neighborLink[linkIdx] = null;
			}
            neighborLink[linkIdx] = gameObject.AddComponent<NavMeshLink>();
            neighborLink[linkIdx].bidirectional = false;
            neighborLink[linkIdx].startPoint = start;
            neighborLink[linkIdx].endPoint = end;
            neighborLink[linkIdx].width = width;
			
			if ( terrainNeighbors[idx].neighborLink[idxInNeighbor / 2] != null )
			{
				Destroy( terrainNeighbors[idx].neighborLink[idxInNeighbor / 2] );
				terrainNeighbors[idx].neighborLink[idxInNeighbor / 2] = null;
			}
			terrainNeighbors[idx].neighborLink[idxInNeighbor / 2] = terrainNeighbors[idx].gameObject.AddComponent<NavMeshLink>();
            terrainNeighbors[idx].neighborLink[idxInNeighbor / 2].bidirectional = false;
            terrainNeighbors[idx].neighborLink[idxInNeighbor / 2].startPoint = neighborStart;
            terrainNeighbors[idx].neighborLink[idxInNeighbor / 2].endPoint = neighborEnd;
            terrainNeighbors[idx].neighborLink[idxInNeighbor / 2].width = width;
        }
#endif
	}
}

