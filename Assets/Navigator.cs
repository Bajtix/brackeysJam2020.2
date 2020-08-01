using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Update()
    {
        GetComponent<NavMeshAgent>().destination = target.position;
    }


}
