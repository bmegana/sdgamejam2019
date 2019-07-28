using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed;
	public float angularSpeed;

    private Rigidbody rb;

	public Transform cameraTransform;
	private Quaternion originalCameraRotation;

	private bool isFireHeld = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
		originalCameraRotation = cameraTransform.rotation;
    }

	private void Start()
	{
		isFireHeld = Input.GetAxis( "Fire1" ) > 0;
	}

	private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

		if ( horizontal != 0 || vertical != 0 )
		{
			Vector3 dir = new Vector3( horizontal, 0.0f, vertical );
			dir.Normalize();

			Quaternion targetRotation = Quaternion.LookRotation( dir, Vector3.up );

			Quaternion newRotation = Quaternion.RotateTowards( transform.rotation, targetRotation, angularSpeed * Time.deltaTime );
			transform.rotation = newRotation;
			cameraTransform.rotation = originalCameraRotation;

			dir *= maxSpeed;
			dir.y = rb.velocity.y;

			rb.velocity = dir;
		}
		else
		{
			Vector2 newVelocity = Vector3.zero;
			newVelocity.y = rb.velocity.y;
			rb.velocity = newVelocity;
		}

		if ( Input.GetAxis( "Fire1" ) > 0 )
		{
			if ( !isFireHeld )
			{
				Destroy( gameObject );
			}
			isFireHeld = true;
		}
		else
		{
			isFireHeld = false;
		}
    }

	public void ResetCamera()
	{
		cameraTransform.rotation = originalCameraRotation;
	}
}
