using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnconstraintedElevator : MonoBehaviour
{
    public float upperLimit;
    public float lowerLimit;
    public float speed = 1f;
    private bool goingUp = true;
    private bool running = false;
    private void Update()
    {
        if(running)
        {
            transform.position += new Vector3(0, (goingUp ? speed : -speed) * Time.deltaTime, 0);
        }
        if (transform.position.y > upperLimit)
            transform.position -= new Vector3(0,speed * Time.deltaTime, 0);
        if (transform.position.y < lowerLimit)
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }

    public void Enabled(bool t)
    {
        running = t;
    }
    public void State(bool t)
    {
        goingUp = t;
    }
}
