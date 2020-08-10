using UnityEngine;

public class MinecartLevel : MonoBehaviour
{
    public float tug = 1;
    public float ctug;
    public Vector2 bound;
    public GameObject cart1;
    public GameObject cart2;

    public bool tugActive = false;
    public AudioSource soundSource;
    private void Update()
    {
        if (!tugActive) return;

        soundSource.loop = false;
        ctug += tug * Time.deltaTime;
        float sstime = soundSource.time;
        soundSource.Pause();
        if (tug > 0 && cart1.transform.position.x < bound.x)
        {
            cart1.transform.position += new Vector3(tug * Time.deltaTime, 0, 0);
            if (!soundSource.isPlaying)
                soundSource.Play();
            soundSource.loop = true;
        }
        if (tug > 0 && cart2.transform.position.x > bound.y)
        {
            cart2.transform.position -= new Vector3(tug * Time.deltaTime, 0, 0);
            soundSource.loop = true;
            if (!soundSource.isPlaying)
                soundSource.Play();
        }


        if (tug < 0 && cart1.transform.position.x > bound.y)
        {
            cart1.transform.position += new Vector3(tug * Time.deltaTime, 0, 0);
            if (!soundSource.isPlaying)
                soundSource.Play();
            soundSource.loop = true;
        }
        if (tug < 0 && cart2.transform.position.x < bound.x)
        { 
            cart2.transform.position -= new Vector3(tug * Time.deltaTime, 0, 0);
            if (!soundSource.isPlaying)
                soundSource.Play();
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
