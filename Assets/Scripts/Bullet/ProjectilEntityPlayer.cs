using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilEntityPlayer : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void Update()
    {
        rb.AddForce(Vector3.forward * 1);
    }
}
