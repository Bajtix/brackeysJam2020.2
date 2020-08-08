using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewerLevel : MonoBehaviour
{
    // Start is called before the first frame update
    public int barrels = 0;
    public GameObject portal;
    private void Update()
    {
        if(barrels > 2)
        {
            portal.SetActive(true);
        }
    }

    public void Barrel()
    {
        barrels++;
    }
}
