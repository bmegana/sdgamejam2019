using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class TRex : MonoBehaviour
{
    public float rushSpeed;
    public float timeUntilRush;

    private float initRushTime;
    private bool rushing = false;
    private bool outOfPoint = false;

    private Collider collider;
    private Rigidbody rb;

    public Transform rushPoint;
    private Vector3 initPosition;
    private Quaternion initRotation;
    private Vector3 rushDirection;

    private void Awake()
    {
        initRushTime = timeUntilRush;

        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        initPosition = transform.position;
        initRotation = transform.rotation;
    }

    private void Start()
    {
        rushDirection = rushPoint.position - initPosition;
        rushDirection.Normalize();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(col.gameObject);
        }
    }

    private void Update()
    {
        if (0.0f < timeUntilRush)
        {
            timeUntilRush -= Time.deltaTime;
        }
        else if (!rushing)
        {
            rushing = true;
            if (collider.bounds.Contains(initPosition))
            {
                rb.velocity = rushDirection * rushSpeed;
            }
            else if (collider.bounds.Contains(rushPoint.position))
            {
                rb.velocity = -rushDirection * rushSpeed;
            }
        }
        else if (rushing && !outOfPoint)
        {
            if (!collider.bounds.Contains(initPosition) &&
                !collider.bounds.Contains(rushPoint.position))
            {
                outOfPoint = true;
            }
        }
        else if (rushing && outOfPoint)
        {
            if (collider.bounds.Contains(initPosition) ||
                collider.bounds.Contains(rushPoint.position))
            {
                rb.velocity = Vector3.zero;
                timeUntilRush = initRushTime;
                outOfPoint = false;
                rushing = false;

                if (collider.bounds.Contains(initPosition))
                {
                    transform.position = initPosition;
                    transform.rotation = initRotation;
                }
                else
                {
                    transform.position = rushPoint.position;
                    transform.rotation = new Quaternion(
                        transform.rotation.x,
                        transform.rotation.y + 180,
                        transform.rotation.z,
                        0.0f
                    );
                }
            }
        }
    }
}
