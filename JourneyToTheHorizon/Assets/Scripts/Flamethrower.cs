using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public float rotateSpeed;
    private bool clockwise;



    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Player") &&
            Input.GetButtonDown("Submit"))
        {
            Debug.Log("Rotating flamethrower.");
        }
    }
}
