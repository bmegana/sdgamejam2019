using UnityEngine;
using UnityEngine.Events;

public class Tree : MonoBehaviour
{
    public class BurnEvent : UnityEvent<Vector3>
    {
    }

    public BurnEvent burnEvent = new BurnEvent();

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Tree on fire.");
            //burnEvent.Invoke(transform.position);
        }
    }
}
