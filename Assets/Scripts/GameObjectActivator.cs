using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectActivator : MonoBehaviour
{
    public GameObject manipulate;

    public void Set(bool d)
    {
        manipulate.SetActive(d);
    }
}
