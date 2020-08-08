using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disclaimer : MonoBehaviour
{

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        
    }

    public void Dismiss()
    {
        LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0, 1).setOnComplete(()=>gameObject.SetActive(false));
    }
}
