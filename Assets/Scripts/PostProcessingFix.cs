using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


[ExecuteInEditMode]
public class PostProcessingFix : MonoBehaviour
{
    public PostProcessResources postProcessResources;

    public Material ifx;
    public Material ifx2;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PostProcessLayer>().Init(postProcessResources);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, ifx);
        if(ifx2 != null)
            Graphics.Blit(source, destination, ifx2);
    }
}
