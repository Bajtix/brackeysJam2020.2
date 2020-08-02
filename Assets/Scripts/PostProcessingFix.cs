using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class PostProcessingFix : MonoBehaviour
{
    public PostProcessResources postProcessResources;

    public Material ifx;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PostProcessLayer>().Init(postProcessResources);
    }

    /*[ExecuteInEditMode]
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Debug.Log("executing");
        //Graphics.Blit(source, destination, ifx);
    }*/
}
