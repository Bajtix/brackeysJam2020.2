using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class PostProcessingFix : MonoBehaviour
{
    public PostProcessResources postProcessResources;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PostProcessLayer>().Init(postProcessResources);
    }

}
