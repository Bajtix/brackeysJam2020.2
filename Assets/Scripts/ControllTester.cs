using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllTester : MonoBehaviour
{
    public float speed = 1;
    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed * Time.deltaTime;
    }
}
