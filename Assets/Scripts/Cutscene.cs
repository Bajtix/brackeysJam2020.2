using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public GameObject ender;
    private void Start()
    {
        LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0, 2);
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            ender.SetActive(true);
        }
    }
}
