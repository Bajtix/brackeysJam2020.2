using System;
using UnityEngine;

public class SwordController : MonoBehaviour
{


    public Transform sword;
    public Transform swordIdlePosition;
    public Transform swordDrawnPosition;
    public float swordSpeed = 2;
    public float swordPredictionSpeed = 2;
    public GameObject trail;
    public float prediction = 5;
    public float timeSlowdown = 0.5f;
    public float energyDrown = 2f;
    public SwordSlicer swordSlicer;
    public GameObject viewModel;

    private Transform swordLook;
    private bool drawn = false;
    private bool playbackMode = false;

    private Vector3 lastPos = Vector3.zero;
    private Vector3 motion;
    private Vector3 mouseSv = Vector3.zero;

    private Vector3 slicerMotion;
    private Vector3 slicerLastPos;

    public Material rewindMatIFX;

    private void Start()
    {
        swordLook = new GameObject("Sword Look").transform;
    }
    private void Update()
    {
        if (Player.instance.dead)
        {
            return;
        }

        motion = viewModel.transform.position - lastPos;
        lastPos = viewModel.transform.position;

        slicerMotion = sword.position - slicerLastPos;
        slicerLastPos = sword.position;

        motion = new Vector3(Math.Abs(motion.x), Math.Abs(motion.y), Math.Abs(motion.z));
        motion = motion.normalized;
        drawn = Input.GetButton("Fire1");
        playbackMode = Input.GetButton("Rewind");
        if (playbackMode)
            rewindMatIFX.SetFloat("_SINE", 0.02f);
        else
            rewindMatIFX.SetFloat("_SINE", 0f);

        RaycastHit h;
        if (Physics.Raycast(transform.position, transform.forward, out h))
        {
            if (h.collider.GetComponent<TELinker>() != null)
            {
                LevelInfo.instance.lockState = h.collider.GetComponent<TELinker>().link.canBeLocked;
            }
            else if (h.collider.GetComponent<TimeEntity>() != null)
            {
                LevelInfo.instance.lockState = h.collider.GetComponent<TimeEntity>().canBeLocked;
            }
            else
            {
                LevelInfo.instance.lockState = false;
            }
        }
        else
        {
            LevelInfo.instance.lockState = false;
        }//todo: merge with Fire2 below

        if (Input.GetButtonDown("Fire2"))
        {
            drawn = false;
            playbackMode = false;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.collider.GetComponent<TELinker>() != null)
                {
                    hit.collider.GetComponent<TELinker>().link.Lock(!hit.collider.GetComponent<TELinker>().link.locked);
                }
                else if (hit.collider.GetComponent<TimeEntity>() != null)
                {
                    hit.collider.GetComponent<TimeEntity>().Lock(!hit.collider.GetComponent<TimeEntity>().locked);
                }
            }
        }

        if (Input.GetButtonDown("Use"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 4))
            {
                if (hit.collider.GetComponent<Interactable>() != null)
                {
                    hit.collider.GetComponent<Interactable>().E();
                }
            }
        }

        if (drawn)
        {
            if (Player.instance.energy <= 0)
            {
                drawn = false;
            }
            else
            {
                Player.instance.energy -= Time.deltaTime * timeSlowdown * energyDrown;
            }
        }
        if (Player.instance.energy <= 0)
        {
            playbackMode = false;

        }
        TimeController.instance.playbackMode = playbackMode;


        if (TimeController.instance.playbackMode)
        {
            return;
        }
        //First we check if the sword is drawn
        if (drawn)
        {
            //if it is we get the mouse input axis using unity's input class
            Vector3 mouseDirection = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            //then we make it lerp slowly from value to value. We aslo create a prediction based on predictionAmount
            mouseSv = Vector3.Lerp(mouseSv, mouseDirection * prediction, Time.deltaTime * swordPredictionSpeed);
            //the sword tilt is calculated using the motion's Y componetnt, which is created by taking the sword's movement and normalizing it's absolute value.
            float tilt = Mathf.Clamp(motion.y * 2, -2, 2);
            viewModel.transform.localRotation = Quaternion.Lerp(viewModel.transform.localRotation, Quaternion.Euler(tilt * 45, 90, 90), Time.deltaTime * 10);

            //the sword trail (the fast movement effect is enabled)
            trail.SetActive(true);
            //and the sword is positioned in the center of player's vision
            sword.position = swordDrawnPosition.position;
            //then we calculate the sword look transform which exists only to make the rotation calculations easier by calculating a position and not a quaternion rotation
            swordLook.position = Vector3.Lerp(swordLook.position, transform.position + transform.forward * 10 + transform.TransformVector(mouseSv), Time.deltaTime * swordSpeed);
            //after we make the sword look at the swordLook object the only thing left is to slow down time
            sword.LookAt(swordLook);
            //done!
            Time.timeScale = timeSlowdown;
        }
        else
        {
            trail.SetActive(false);
            sword.position = swordIdlePosition.position;
            sword.rotation = swordIdlePosition.rotation;

        }

        if (Input.GetButtonUp("Fire1"))
        {
            Time.timeScale = 1;
        }

        swordSlicer.slicing = drawn;
    }
    public void Deflect()
    {
        swordLook.position -= slicerMotion * 100;

        Player.instance.CameraLook(swordLook.position);
    }
}
