using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{
    public Vector3 waterStream;
    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Rigidbody>() != null)
        {
            other.GetComponent<Rigidbody>().AddForce((waterStream - Physics.gravity * 50) * Time.deltaTime);
        }
    }
}
