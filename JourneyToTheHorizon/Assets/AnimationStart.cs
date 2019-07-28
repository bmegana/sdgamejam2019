using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStart : MonoBehaviour
{
    private Animation Anim;

    // Start is called before the first frame update
    void Start()
    {
        Anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        Anim.Play("CamAnimation");
    }
}
