using UnityEngine;

public class BoulderTrigger : MonoBehaviour
{
    public Boulder boulder;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            boulder.Activate();
        }
    }
}
