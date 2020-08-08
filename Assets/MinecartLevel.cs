using UnityEngine;

public class MinecartLevel : MonoBehaviour
{
    public float tug = 1;
    public float ctug;
    public float bound;
    public GameObject cart1;
    public GameObject cart2;

    public bool tugActive = false;

    private void Update()
    {
        if (!tugActive) return;
        if (ctug < bound && tug > 0)
        {
            ctug += tug * Time.deltaTime;
            cart1.transform.position += new Vector3(tug * Time.deltaTime, 0, 0);
            cart2.transform.position -= new Vector3(tug * Time.deltaTime, 0, 0);
        }

        if (ctug > 0 && tug < 0)
        {
            ctug += tug * Time.deltaTime;
            cart1.transform.position += new Vector3(tug * Time.deltaTime, 0, 0);
            cart2.transform.position -= new Vector3(tug * Time.deltaTime, 0, 0);
        }      

    }

    public void TugActive(bool set)
    {
        tugActive = set;
    }

    public void SetTug(int t)
    {
        tug = t;
    }

}
