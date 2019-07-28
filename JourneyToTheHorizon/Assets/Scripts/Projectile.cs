using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float secondsProjectileLife = 2.0f;

    private IEnumerator ShortenProjectileLife()
    {
        while (0 < secondsProjectileLife)
        {
            secondsProjectileLife -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        StartCoroutine(ShortenProjectileLife());
    }

    private void OnCollisionEnter(Collision col)
    {
        GameObject entity = col.gameObject;
        if (entity.CompareTag("Player") || entity.CompareTag("Tree"))
        {
            if (entity.CompareTag("Player"))
            {
                Destroy(entity);
            }
            Destroy(gameObject);
        }
    }
}
