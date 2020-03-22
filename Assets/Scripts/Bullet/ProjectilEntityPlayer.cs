using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilEntityPlayer : MonoBehaviour
{
    public GameObject target;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void Update()
    {

        rb.AddForce(target.transform.position * 5); 
         
    }
    
}
