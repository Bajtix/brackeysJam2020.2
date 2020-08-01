using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAnimator : MonoBehaviour
{
    public LegAnimator otherLeg;

    public Transform legStart;
    public Transform legTargetTransform;
    public float step = 2;
    public float movementMultiplier;
    public bool grounded = true;

    public float groundedMult = 0.25f;
    public float errorMult = 1.2f;

    public float delayTime = 0;

    private Vector3 lastPosition;
    private Vector3 lastPPos;
    private Vector3 movement;

    void Start()
    {
        lastPPos = transform.position;
        RaycastHit hit;
        Physics.Raycast(legStart.position, Vector3.down,out hit);
        lastPosition = hit.point;
        legTargetTransform.position = hit.point;
    }

    void FixedUpdate()
    {
        //LeanTween.delayedCall(delayTime, () => CalculateMovement());
        CalculateMovement();
    }

    public void CalculateMovement()
    {
        movement = lastPPos - transform.position;
        lastPPos = transform.position;
        legTargetTransform.position = Vector3.Lerp(legTargetTransform.position, lastPosition, Time.deltaTime * 10);
        RaycastHit hit;
        if (Physics.Raycast(legStart.position, Vector3.down, out hit))
        {
            if ((Vector3.Distance(hit.point, lastPosition) > step && otherLeg.grounded) || movement.magnitude < 0.01f)
            {
                lastPosition = hit.point - movement * movementMultiplier;
            }

            if (Vector3.Distance(legTargetTransform.position, hit.point) > step * errorMult)
            {
                lastPosition = hit.point - movement * movementMultiplier;
            }
        }



        if (Vector3.Distance(legTargetTransform.position, lastPosition) < step * groundedMult)
            grounded = true;
        else
        {
            grounded = false;
        }
    }
    
}
