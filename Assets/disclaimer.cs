using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disclaimer : MonoBehaviour
{

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        LeanTween.delayedCall(8, () => LeanTween.alphaCanvas(GetComponent<CanvasGroup>(),0,1));
    }

    
}
