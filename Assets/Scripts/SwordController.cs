using UnityEngine;


public class SwordController : MonoBehaviour
{


    public Transform sword;
    public Transform swordIdlePosition;
    public Transform swordDrawnPosition;
    public float swordSpeed = 2;
    public GameObject trail;
    private Transform swordLook;
    private bool drawn;

    public float timeSlowdown = 0.5f;

    public float energyDrown = 2f;

    private void Start()
    {
        swordLook = new GameObject("Sword Look").transform;
    }
    private void Update()
    {


        drawn = Input.GetButton("Fire1");

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

        if (drawn)
        {
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
    }
}
