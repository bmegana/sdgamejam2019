using System.Collections;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public float rumbleTime;
    public float rumbleDelta;
    private float initRumbleTime;

    private Rigidbody rb;
    private bool activated = false;

    private IEnumerator Rumble()
    {
        Vector3 initPosition = transform.position;
        while (0.0f < rumbleTime)
        {
            rumbleTime -= Time.deltaTime;
            transform.position = new Vector3(
                initPosition.x + Random.Range(-rumbleDelta, rumbleDelta),
                initPosition.y + Random.Range(-rumbleDelta, rumbleDelta),
                initPosition.z + Random.Range(-rumbleDelta, rumbleDelta)
            );
            yield return new WaitForSeconds(0.01f);
            transform.position = initPosition;
        }
        activated = true;
        rb.useGravity = true;
    }

    public void Activate()
    {
        StartCoroutine(Rumble());
    }

    private void Awake()
    {
        initRumbleTime = rumbleTime;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        if (!activated)
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
    }
}
