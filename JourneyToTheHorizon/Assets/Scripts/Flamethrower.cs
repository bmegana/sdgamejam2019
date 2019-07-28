using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public float rotateSpeed;
    private bool clockwise;
    public int flameTimeOff;
    public int flameTimeOn;
    private int initialFlameTime;
    ParticleSystem system;


    private void Start()
    {
        initialFlameTime = flameTimeOn;
        system = gameObject.GetComponent<ParticleSystem>();
    }
    /*private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Player") &&
            Input.GetButtonDown("Submit"))
        {
            Debug.Log("Rotating flamethrower.");
        }
    }*/

    private void Update()
    {
       
       
        if (flameTimeOn > 0)
        {
            flameTimeOn --;
            if (flameTimeOn <= 0)
            {
                system.Stop();
                flameTimeOff = initialFlameTime;
            }

        } 
         if (flameTimeOff > 0)
        {
            flameTimeOff--;
            if (flameTimeOff <=0)
            {
                system.Play();
                flameTimeOn = initialFlameTime;
            }
        }
    }
}
