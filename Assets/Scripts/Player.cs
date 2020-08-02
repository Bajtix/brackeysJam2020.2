using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform cam;

    public float speed = 5f;
    public float jumpForce = 0.1f;
    public float gravityMultiplier = 0.1f;
    private float xRot = 0;
    private Vector3 gravity;
    private CharacterController controller;

    public float playerEnergy = 5;
    //[System.NonSerialized]
    public float energy = 5;



    #region Singleton
    public static Player instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }
    #endregion

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
            gravity += Physics.gravity * gravityMultiplier * Time.deltaTime;
        else
        {
            if (Input.GetButton("Jump"))
            {
                    gravity += new Vector3(0, jumpForce, 0);
            }
            else
                gravity = Vector3.zero;
        }
        controller.Move(gravity * Time.deltaTime);
        controller.Move(Time.deltaTime * (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * speed);
    }
    
}
