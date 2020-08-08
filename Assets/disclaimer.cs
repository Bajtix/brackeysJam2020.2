using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disclaimer : MonoBehaviour
{

    void Start()
    {
        LeanTween.delayedCall(10, () => LeanTween.alphaCanvas(GetComponent<CanvasGroup>(),0,1));
    }

    
}
