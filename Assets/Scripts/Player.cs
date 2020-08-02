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


    [ColorUsage(true, true)]
    public Color fullColor;

    [ColorUsage(true, true)]
    public Color emptyColor;

    public Material EnergyVisualiser;

    public float playerEnergy = 5;
    //[System.NonSerialized]
    public float energy = 5;

    Vector3 movement;

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
        //EnergyVisualiser.SetFloat("Saturation", energy / playerEnergy);
        EnergyVisualiser.SetColor("_EmissionColor", Color.Lerp(emptyColor, fullColor, energy / playerEnergy));

        if (TimeController.instance.playbackMode) return;

        gravity = Vector3.zero;

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X"), 0);
        xRot -= Input.GetAxis("Mouse Y");
        if (xRot > 90)
            xRot = 90;

        if (xRot < -90)
            xRot = -90;
        cam.localRotation = Quaternion.Euler(xRot, 0, 0);

        float my = movement.y;
        movement = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * speed;
        movement.y = my;


        if (controller.isGrounded && Input.GetButton("Jump"))
            movement.y = jumpForce;

        
        movement.y -= gravityMultiplier * Time.deltaTime;
        controller.Move(Time.deltaTime * movement);
    }
    
}
