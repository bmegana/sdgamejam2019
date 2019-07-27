using UnityEngine;

public class SkyRockSpawn : MonoBehaviour
{
    public float spawnRate;
    private float time = 0.0f;

    public GameObject skyRock;
    public float height;
    public float xBound;
    public float zBound;

    private void Update()
    {
        time += Time.deltaTime;
        if (spawnRate < time)
        {
            time = 0.0f;

            float xPos = Random.Range(-xBound, xBound);
            float zPos = Random.Range(-zBound, zBound);
            Vector3 spawnPoint = new Vector3(
                xPos, height, zPos
            );
            Instantiate(skyRock, spawnPoint, new Quaternion());
        }
    }
}
