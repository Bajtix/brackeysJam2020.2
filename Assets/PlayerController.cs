using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cam;

    public float speed = 5f;
    public float jumpForce = 0.1f;
    public float gravityMultiplier = 0.1f;
    private float xRot = 0;
    private Vector3 gravity;
    private CharacterController controller;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X"), 0);
        xRot -= Input.GetAxis("Mouse Y");
        if (xRot > 90)
            xRot = 90;

        if (xRot < -90)
            xRot = -90;
        cam.localRotation = Quaternion.Euler(xRot, 0, 0);

        if (!controller.isGrounded)
            gravity += Physics.gravity * Time.deltaTime * gravityMultiplier;
        else
        {
            if (Input.GetButton("Jump"))
            {
                if (controller.isGrounded)
                {
                    gravity += new Vector3(0, jumpForce, 0);
                }
            }
            else
                gravity = Vector3.zero;
        }
        controller.Move(gravity);
    }
    private void FixedUpdate()
    {      
        controller.Move(Time.deltaTime * (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * speed);
    }
}
