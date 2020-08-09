using System.Net;
using UnityEditor;
using UnityEngine;


public class ObjectPlacerWindow : EditorWindow
{

    private bool placing = false;
    private ScatterObjectPack pack = null;
    private int next;
    GameObject previewed;
    private Editor gameObjectEditor;
    private bool randomRot;
    private Vector3 additionalRot;
    [MenuItem("Tools/Object Scatter")]
    private static void Init()
    {
        ObjectPlacerWindow window = (ObjectPlacerWindow)EditorWindow.GetWindow(typeof(ObjectPlacerWindow));
        window.Show();
        window.next = 0;
    }


    private void OnGUI()
    {
        e();
        pack = (ScatterObjectPack)EditorGUILayout.ObjectField("Pack",pack,typeof(ScatterObjectPack),false);

        
        if (pack != null && pack.objects.Length != 0)
        {

            additionalRot = EditorGUILayout.Vector3Field("Rotation", additionalRot);
            randomRot = GUILayout.Toggle(randomRot, "Random Y rotation");
            if (GUILayout.Button(placing ? "Exit Place Mode" : "Place Mode"))
            {
                placing = !placing;
                
            }
        }
        else
            GUILayout.Label("Select an assetpack");

        if (Event.current.type == EventType.MouseMove)
            Repaint();
    }

    void e()
    {
        if (pack != null && pack.objects.Length != 0)
        {
            GameObject gameObject = pack.objects[next];
            Preview(gameObject);
        }
    }


    void Preview(GameObject gameObject)
    {
        if (gameObject != null)
        {
            if (previewed != gameObject)
            {
                if (gameObjectEditor != null)
                    DestroyImmediate(gameObjectEditor);
                gameObjectEditor = Editor.CreateEditor(gameObject);

            }
            previewed = gameObject;
            gameObjectEditor.OnPreviewGUI(GUILayoutUtility.GetRect(100, 100), EditorStyles.whiteLabel);
        }
    }
    // Window has been selected
    private void OnFocus()
    {
        
        // Remove delegate listener if it has previously
        // been assigned.
        SceneView.duringSceneGui -= this.OnSceneGUI;
        // Add (or re-add) the delegate.
        SceneView.duringSceneGui += this.OnSceneGUI;
    }

    private void OnDestroy()
    {
        placing = false;
        // When the window is destroyed, remove the delegate
        // so that it will no longer do any drawing.
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.control && placing)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log(hit.collider.gameObject.name);
                next = Random.Range(0, pack.objects.Length);
                GameObject g = randomRot ? Instantiate(pack.objects[next], hit.point, Quaternion.Euler(0,Random.Range(0,360),0) *Quaternion.Euler(additionalRot)) : Instantiate(pack.objects[next], hit.point, Quaternion.LookRotation(hit.normal) * Quaternion.Euler(additionalRot));
                if(pack.randomiseMaterials)
                {
                    Material m = pack.randomMaterials[Random.Range(0, pack.randomMaterials.Length - 1)];
                    g.GetComponent<MeshRenderer>().material = m;
                }
                g.tag = "Small Detail";
                Undo.RegisterCreatedObjectUndo(g, "Create GameObject");
                Repaint();
                //HandleUtility.AddDefaultControl(0);
            }
        }
    }



}
