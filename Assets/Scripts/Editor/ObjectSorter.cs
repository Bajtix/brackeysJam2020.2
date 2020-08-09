using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectSorter : EditorWindow
{

    Transform logicsStuff;
    //Transform shatterableStuff;
    Transform decorativeStuff;
    //Transform otherStuff;
    //Transform utilityStuff;
    Transform lightStuff;
    
    [MenuItem("Tools/Sorter")]
    private static void Init()
    {
        ObjectSorter window = (ObjectSorter)EditorWindow.GetWindow(typeof(ObjectSorter));
        window.Show();
    }

    private void OnGUI()
    {
        logicsStuff = (Transform)EditorGUILayout.ObjectField("Logic",logicsStuff, typeof(Transform),false);
        //shatterableStuff = (Transform)EditorGUILayout.ObjectField("Logic",shatterableStuff, typeof(Transform),false);
        decorativeStuff = (Transform)EditorGUILayout.ObjectField("Logic",decorativeStuff, typeof(Transform),false);
        //otherStuff = (Transform)EditorGUILayout.ObjectField("Logic",otherStuff, typeof(Transform),false);
        //utilityStuff = (Transform)EditorGUILayout.ObjectField("Logic",utilityStuff, typeof(Transform),false);
        lightStuff = (Transform)EditorGUILayout.ObjectField("Logic",lightStuff, typeof(Transform),false);

        if (GUILayout.Button("Create Categories"))
        {
            if (logicsStuff == null)
                logicsStuff = FindOrCreateObject("Level Logic");
           /* if (shatterableStuff == null)
                shatterableStuff = FindOrCreateObject("Shatterable");*/
            if (decorativeStuff == null)
                logicsStuff = FindOrCreateObject("Decorative");
            /*if (otherStuff == null)
                otherStuff = FindOrCreateObject("Other");*/
            /*if (utilityStuff == null)
                utilityStuff = FindOrCreateObject("Utility");*/
            if (lightStuff == null)
                lightStuff = FindOrCreateObject("Lighting");
        }
        if (GUILayout.Button("Sort Objects"))
        {
            GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
            foreach(GameObject g in interactables)
            {
                if (g.transform.parent == null)
                    g.transform.SetParent(logicsStuff);
            }

            GameObject[] decorations = GameObject.FindGameObjectsWithTag("Small Detail");
            foreach (GameObject g in decorations)
            {
                if (g.transform.parent == null)
                    g.transform.SetParent(decorativeStuff);
            }

            GameObject[] lighting = GameObject.FindGameObjectsWithTag("LightSource");
            foreach (GameObject g in lighting)
            {
                if (g.transform.parent == null)
                    g.transform.SetParent(decorativeStuff);
            }
            ReflectionProbe[] probes = GameObject.FindObjectsOfType<ReflectionProbe>();
            foreach (ReflectionProbe g in probes)
            {
                if (g.transform.parent == null)
                    g.transform.SetParent(decorativeStuff);
            }
        }
    }

    private Transform FindOrCreateObject(string objname)
    {
        GameObject g = GameObject.Find(objname);
        if (g == null)
            g = new GameObject(objname);

        return g.transform;
    }
}
