using UnityEngine;

public class MinecartLevel : MonoBehaviour
{
    public float tug = 1;
    public float ctug;
    public float bound;
    public GameObject cart1;
    public GameObject cart2;

    public bool tugActive = false;
    public AudioSource soundSource;
    private void Update()
    {
        if (!tugActive) return;
        soundSource.loop = false;

        if (ctug < bound && tug > 0)
        {
            ctug += tug * Time.deltaTime;
            cart1.transform.position += new Vector3(tug * Time.deltaTime, 0, 0);
            cart2.transform.position -= new Vector3(tug * Time.deltaTime, 0, 0);
            soundSource.loop = true;
        }

        if (ctug > 0 && tug < 0)
        {
            ctug += tug * Time.deltaTime;
            cart1.transform.position += new Vector3(tug * Time.deltaTime, 0, 0);
            cart2.transform.position -= new Vector3(tug * Time.deltaTime, 0, 0);
            soundSource.loop = true;
        }

        if (!soundSource.loop)
            soundSource.Stop();

    }

    public void TugActive(bool set)
    {
        tugActive = set;

        if (set) soundSource.Play();
        else soundSource.Stop();
    }

    public void SetTug(int t)
    {
        tug = t;
    }

}
