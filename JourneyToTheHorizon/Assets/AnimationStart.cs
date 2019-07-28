using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationStart : MonoBehaviour
{
    private Animation Anim;

    // Start is called before the first frame update
    void Start()
    {
        Anim = gameObject.GetComponent<Animation>();
        Anim.Play("CamAnimation2");
        
    }
    
    // Update is called once per frame
}
