using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed;
	public float angularSpeed;

    private Rigidbody rb;

	public Transform cameraTransform;
	private Quaternion originalCameraRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
		originalCameraRotation = cameraTransform.rotation;
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
			rb.velocity = Vector3.zero;
		}
    }
}
