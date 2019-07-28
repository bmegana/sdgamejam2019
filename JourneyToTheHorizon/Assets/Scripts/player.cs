using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class player : MonoBehaviour {
    public GameObject food;
    public SphereCollider FoodCollider;
    int hunger = 30;
    void Start()
    {

       
    }
    void OnCollisionEnter(Collision Collision)
    {
        if (Collision.gameObject.tag == "Food")
        {
  
            Console.WriteLine("Press 'E' To Use");
            if (Input.GetAxis("Submit")> 0)
            {
                Destroy(Collision.gameObject);
            }
            hunger = 30;
        }
        
    }

    void Hunger()
    {
        
        Debug.Log (hunger);
        hunger--;
        
        if (hunger == 0)
            {
                //void Destroy(Object Follower)
            }

    }
    void Torch()
    {
        
    }


   


   
    
}

