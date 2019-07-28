using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Kraken : MonoBehaviour
{
    private int PLAYER_LAYER;
    private int TREE_LAYER;

    public float shootingHeight;
    public float shootingTime;
    private float initShootingTime;

    public float shootingRadius;
    private Collider collider;
    private bool playerWithinRange;
    private Vector3 playerPosition;

    public float projectileSpeed;
    public GameObject waterProjectile;

    public List<Tree> trees;
    public List<Vector3> burningTreePositions;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        TREE_LAYER = LayerMask.NameToLayer("Tree");
        PLAYER_LAYER = LayerMask.NameToLayer("Player");

        burningTreePositions = new List<Vector3>();
    }

    private void AddTreeOnFire(Vector3 pos)
    {
        if (!burningTreePositions.Contains(pos))
        {
            burningTreePositions.Add(pos);
        }
    }

    private void Start()
    {
        initShootingTime = shootingTime;

        Collider[] colliders = Physics.OverlapSphere(
            transform.position, shootingRadius, 1 << TREE_LAYER
        );

        foreach (Collider c in colliders)
        {
            Tree treeComponent = c.gameObject.GetComponent<Tree>();
            if (treeComponent != null)
            {
                trees.Add(treeComponent);
            }
        }
        foreach (Tree t in trees)
        {
            t.burnEvent.AddListener(AddTreeOnFire);
        }
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position, shootingRadius, 1 << PLAYER_LAYER
        );

        if (0 < colliders.Length)
        {
            playerWithinRange = true;
            playerPosition = colliders[0].transform.position;
        }
        else
        {
            playerWithinRange = false;
        }
    }

    private void Shoot(Vector3 pos)
    {
        Vector3 shootingPosition = new Vector3(
            transform.position.x,
            transform.position.y + shootingHeight,
            transform.position.z
        );
        GameObject waterOrb = Instantiate(
            waterProjectile, shootingPosition, new Quaternion()
        );
        
        Rigidbody waterOrbRb = waterOrb.GetComponent<Rigidbody>();
        Vector3 projectileDir = pos - transform.position;
        waterOrbRb.velocity = projectileDir * projectileSpeed;
    }

    private void Update()
    {
        if (0 < burningTreePositions.Count || playerWithinRange)
        {
            shootingTime -= Time.deltaTime;
            if (shootingTime < 0.0f)
            {
                shootingTime = initShootingTime;

                if (0 < burningTreePositions.Count)
                {
                    Shoot(burningTreePositions[0]);
                    burningTreePositions.RemoveAt(0);
                }
                else
                {
                    Shoot(playerPosition);
                }
            }
        }
    }
}
