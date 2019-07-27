using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] terrainPrefabs;
    [SerializeField] private GameObject playerCharacter;
    [SerializeField] private Vector3 centerAdjust = new Vector3( 50.0f, 0.0f, 50.0f );
    [SerializeField] private Vector3 spawnExtents = new Vector3( 40.0f, 0.0f, 40.0f );
    [SerializeField] private Vector3 spawnLimits = new Vector3( 50.0f, 0.0f, 50.0f );
    [SerializeField] private Vector3 spawnOffset = new Vector3( 100.0f, 0.0f, 100.0f );
    [SerializeField] private float despawnDist = 200.0f;
    [SerializeField] private bool enableSpawn = true;
    [SerializeField] private Vector3 debugPlayerDist = new Vector3();

    // Going clockwise (looking towards -Y) starting at +X
    private TerrainSpawner[] terrainNeighbors = { null, null, null, null, null, null, null, null };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( playerCharacter != null )
        {
            Vector3 vPlayerDistance = playerCharacter.transform.position - transform.position - centerAdjust;
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
                    if ( terrainNeighbors[7] == null )
                    {
                        SpawnNeighbor( 7, new Vector3( posX, sameY, posZ ) );
                    }
                    if ( terrainNeighbors[0] == null )
                    {
                        SpawnNeighbor( 0, new Vector3( posX, sameY, sameZ ) );
                    }
                    if ( terrainNeighbors[1] == null )
                    {
                        SpawnNeighbor( 1, new Vector3( posX, sameY, negZ ) );
                    }
                }
                else if ( vPlayerDistance.x < -spawnExtents.x )
                {
                    if ( terrainNeighbors[3] == null )
                    {
                        SpawnNeighbor( 3, new Vector3( negX, sameY, negZ ) );
                    }
                    if ( terrainNeighbors[4] == null )
                    {
                        SpawnNeighbor( 4, new Vector3( negX, sameY, sameZ ) );
                    }
                    if ( terrainNeighbors[5] == null )
                    {
                        SpawnNeighbor( 5, new Vector3( negX, sameY, posZ ) );
                    }
                }

                if ( vPlayerDistance.z > spawnExtents.z )
                {
                    if ( terrainNeighbors[5] == null )
                    {
                        SpawnNeighbor( 5, new Vector3( negX, sameY, posZ ) );
                    }
                    if ( terrainNeighbors[6] == null )
                    {
                        SpawnNeighbor( 6, new Vector3( sameX, sameY, posZ ) );
                    }
                    if ( terrainNeighbors[7] == null )
                    {
                        SpawnNeighbor( 7, new Vector3( posX, sameY, posZ ) );
                    }
                }
                else if ( vPlayerDistance.z < -spawnExtents.z )
                {
                    if ( terrainNeighbors[1] == null )
                    {
                        SpawnNeighbor( 1, new Vector3( posX, sameY, negZ ) );
                    }
                    if ( terrainNeighbors[2] == null )
                    {
                        SpawnNeighbor( 2, new Vector3( sameX, sameY, negZ ) );
                    }
                    if ( terrainNeighbors[3] == null )
                    {
                        SpawnNeighbor( 3, new Vector3( negX, sameY, negZ ) );
                    }
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
                terrainNeighbors[idx].terrainNeighbors[( idx + 4 ) % 8] = null;
                terrainNeighbors[idx] = null;
            }
        }
    }

    private void SpawnNeighbor( int idx, Vector3 pos )
    {
        GameObject neighbor = Instantiate( terrainPrefabs[0], pos, Quaternion.identity );
        terrainNeighbors[idx] = neighbor.GetComponent<TerrainSpawner>();
        if ( terrainNeighbors[idx] == null )
        {
            terrainNeighbors[idx] = neighbor.AddComponent<TerrainSpawner>();
        }
        terrainNeighbors[idx].terrainNeighbors[( idx + 4 ) % 8] = this;
        terrainNeighbors[idx].playerCharacter = playerCharacter;
    }
}
