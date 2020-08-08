using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RailBuilder : MonoBehaviour
{


    public class RailPiece
    {
        public Transform self;
        public Vector3 bend;
        public List<Transform> bones;

        public RailPiece(Transform self)
        {
            this.self = self;
            this.bend = Vector3.zero;
            GetBones();
        }

        [ExecuteInEditMode]
        public void GetBones()
        {
            bones = new List<Transform>();
            bones.AddRange(self.GetChild(0).GetComponentsInChildren<Transform>());
        }
    }


    private List<RailPiece> railPieces;

    public int segmentCount;
    public GameObject prefab;




    [ExecuteInEditMode]
    private void Refresh()
    {
        if (railPieces == null)
        {
            return;
        }

        int j = 0;
        foreach (RailPiece r in railPieces)
        {
            for (int i = 0; i < r.bones.Count; i++)
            {
                if (i > 1)
                {
                    r.bones[i].localRotation = Quaternion.Lerp(r.bones[i - 1].localRotation, Quaternion.Euler(r.bend), 0.5f);
                }
                else
                {
                    if (j > 0)
                    {
                        RailPiece k = railPieces[j -1];
                        r.bones[i].rotation = Quaternion.Lerp(k.bones[k.bones.Count - 2].rotation, Quaternion.Euler(r.bend), 0f);
                        r.bones[i].position = k.bones[k.bones.Count - 2].position;
                    }
                }

            }
            j++;
        }
    }
    [CustomEditor(typeof(RailBuilder))]
    public class RailEditor : Editor
    {
        
        void CreateRail()
        {
            RailBuilder self = (RailBuilder)target;
            if (self.railPieces != null)
            {
                foreach (RailPiece r in self.railPieces)
                {
                    DestroyImmediate(r.self.gameObject);
                }

            }
            self.railPieces = new List<RailPiece>();
            for (int i = 0; i < self.segmentCount; i++)
            {
                GameObject piece = Instantiate(self.prefab, self.transform.position - Vector3.forward * i * 5, Quaternion.identity, self.transform);
                self.railPieces.Add(new RailPiece(piece.transform));
            }

            self.Refresh();
        }
        public override void OnInspectorGUI()
        {
            RailBuilder self = (RailBuilder)target;
            self.prefab = (GameObject)EditorGUILayout.ObjectField("Rail Prefab", self.prefab, typeof(GameObject), false);
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("-"))
            {
                List<RailPiece> backup = self.railPieces;
                self.segmentCount--;
                CreateRail();
                int index = 0;
                foreach(RailPiece r in self.railPieces)
                {
                    r.bend = backup[index].bend;
                    index++;
                }
            }
            GUILayout.Label(self.segmentCount.ToString());
            if (GUILayout.Button("+"))
            {
                List<RailPiece> backup = self.railPieces;
                self.segmentCount++;
                CreateRail();
                int index = 0;
                foreach (RailPiece r in self.railPieces)
                {
                    if(index < backup.Count)
                    r.bend = backup[index].bend;
                        
                    index++;
                }
            }
            EditorGUILayout.EndHorizontal();
            //base.OnInspectorGUI();
            if (GUILayout.Button("Create rail"))
            {
                CreateRail();
            }
            if (self.railPieces != null)
            {
                foreach (RailPiece r in self.railPieces)
                {
                    GUILayout.Label("Rail Piece");
                    r.bend = EditorGUILayout.Vector3Field("Bend", r.bend);
                }
            }
            ((RailBuilder)this.target).Refresh();
            
        }
    }
}
