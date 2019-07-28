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
        if (Collision.gameObject.tag == "Fire")
        {
            Console.WriteLine("Press 'E' To Light");
            if (Input.GetAxis("Submit")>0)
            {
                RemoveItem.Inventory(Stick);
                AddItem.Inventory(Torch);
            }
        if (Collision.gameObject.tag == "tree")
            {
                Console.WriteLine("Press 'F' to Burn");
                if (Input.GetAxis("axis")> 0)
                {
                    Destroy(gameObject(Tree));
                    Create(gameObject(Fire));
                }
            }
        }
        if (Collision.gameObject.tag == "Water")
        {
            Console.WriteLine("Press 'E' to collect");
            if (Input.GetAxis("Submit")>0)
            {
                RemoveItem.Inventory(Bucket);
                AddItem.Inventory(Water);
            }
        }
        if (Collision.gameObject.tag == "Fire")
        {
            Console.WriteLine("Press 'Q' to Douse");
            if (Input.GetAxis("axis")>0)
            {
                RemoveItem.Inventory(Water);
                AddItem.Inventory(Bucket);
                Destroy(gameObject(Fire));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Recruit")
        {
            Instantiate(this, other.transform.position, other.transform.rotation);
            Destroy(other);
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
    
   
    /*void Rock()
    {
        if (OnCollisionEnter.gameObject.tag == "Rock")
        {
            Console.WriteLine("Press 'E' to Collect");
            if (input.GetAxis("Submit")>0)
            {
                AddItem.Inventory(Rock);
            }
        }
    }*/
   


}

