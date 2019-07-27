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

    public Vector3 rushPoint;
    private Vector3 initPosition;
    private Vector3 rushDirection;

    private void Awake()
    {
        initRushTime = timeUntilRush;

        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        initPosition = transform.position;
    }

    private void Start()
    {
        rushDirection = rushPoint - initPosition;
        rushDirection.Normalize();
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
            else if (collider.bounds.Contains(rushPoint))
            {
                rb.velocity = -rushDirection * rushSpeed;
            }
        }
        else if (rushing && !outOfPoint)
        {
            if (!collider.bounds.Contains(initPosition) &&
                !collider.bounds.Contains(rushPoint))
            {
                outOfPoint = true;
            }
        }
        else if (rushing && outOfPoint)
        {
            if (collider.bounds.Contains(initPosition) ||
                collider.bounds.Contains(rushPoint))
            {
                rb.velocity = Vector3.zero;
                timeUntilRush = initRushTime;
                outOfPoint = false;
                rushing = false;
            }
        }
    }
}
