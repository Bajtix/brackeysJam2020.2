using UnityEngine;


public class SwordController : MonoBehaviour
{


    public Transform sword;
    public Transform swordIdlePosition;
    public Transform swordDrawnPosition;
    public float swordSpeed = 2;
    public GameObject trail;

    public float timeSlowdown = 0.5f;
    public float energyDrown = 2f;
    public SwordSlicer swordSlicer;
    public GameObject viewModel;

    private Transform swordLook;
    private bool drawn = false;
    private bool playbackMode = false;

    private Vector3 lastPos = Vector3.zero;
    private Vector3 motion;

    private void Start()
    {
        swordLook = new GameObject("Sword Look").transform;
    }
    private void Update()
    {
        motion = viewModel.transform.position - lastPos;
        lastPos = viewModel.transform.position;


        drawn = Input.GetButton("Fire1");
        playbackMode = Input.GetButton("Fire2");

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
            return;
        if (drawn)
        {
            Vector3 mouseDirection = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            float tilt = Mathf.Clamp(mouseDirection.y, -1, 1);
            viewModel.transform.localRotation = Quaternion.Lerp(viewModel.transform.localRotation, Quaternion.Euler(tilt * 90, 90, 90), Time.deltaTime * 10);

            trail.SetActive(true);
            sword.position = swordDrawnPosition.position;
            swordLook.position = Vector3.Lerp(swordLook.position, transform.position + transform.forward * 10, Time.deltaTime * swordSpeed);
            sword.LookAt(swordLook);
            Time.timeScale = timeSlowdown;
        }
        else
        {
            trail.SetActive(false);
            sword.position = swordIdlePosition.position;
            sword.rotation = swordIdlePosition.rotation;
            Time.timeScale = 1;
        }

        swordSlicer.slicing = drawn;
    }
}
