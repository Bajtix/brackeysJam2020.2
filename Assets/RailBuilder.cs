using System;
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


    public List<RailPiece> railPieces;

    public int segmentCount;
    public GameObject prefab;


    public List<bool> editors;

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
                    r.bones[i].localRotation = Quaternion.Lerp(r.bones[i - 1].localRotation, Quaternion.Euler(r.bend / 4), 0.5f);
                }
                else
                {
                    if (j > 0)
                    {
                        RailPiece k = railPieces[j - 1];
                        r.bones[i].rotation = Quaternion.Lerp(k.bones[k.bones.Count - 2].rotation, Quaternion.Euler(r.bend / 4), 0f);
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
        private List<bool> foldouts;
        private List<bool> edits;

        private void DestroyRail()
        {
            RailBuilder self = (RailBuilder)target;
            if (self.railPieces != null)
            {
                foreach (RailPiece r in self.railPieces)
                {
                    if (r.self != null)
                    {
                        DestroyImmediate(r.self.gameObject);
                    }
                }
                self.railPieces.Clear();
            }
        }

        private void CreateRail()
        {

            RailBuilder self = (RailBuilder)target;

            Quaternion srot = self.transform.rotation;
            self.transform.rotation = Quaternion.identity;
            DestroyRail();
            self.railPieces = new List<RailPiece>();
            for (int i = 0; i < self.segmentCount; i++)
            {
                GameObject piece = Instantiate(self.prefab, self.transform.position - Vector3.forward * i * 5, Quaternion.identity, self.transform);
                self.railPieces.Add(new RailPiece(piece.transform));
            }
            self.transform.rotation = srot;
            self.Refresh();
        }
        public override void OnInspectorGUI()
        {

            RailBuilder self = (RailBuilder)target;
            self.editors = edits;
            if (self.railPieces == null)
            {
                self.railPieces = new List<RailPiece>();
            }
            if(foldouts == null)
            {
                foldouts = new List<bool>();
                foreach (RailPiece r in self.railPieces)
                    foldouts.Add(false);
            }
            self.prefab = (GameObject)EditorGUILayout.ObjectField("Rail Prefab", self.prefab, typeof(GameObject), false);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("-"))
            {
                foldouts = new List<bool>();
                edits = new List<bool>();

                RailPiece[] backup = new RailPiece[self.railPieces.Count];
                self.railPieces.CopyTo(backup);

                self.segmentCount--;
                CreateRail();
                int index = 0;
                foreach (RailPiece r in self.railPieces)
                {
                    foldouts.Add(false);
                    edits.Add(false);
                    if (backup != null && index < backup.Length)
                    {
                        r.bend = backup[index].bend;
                    }

                    index++;
                }
            }
            GUILayout.Label(self.segmentCount.ToString());
            if (GUILayout.Button("+"))
            {
                foldouts = new List<bool>();
                edits = new List<bool>();

                RailPiece[] backup = new RailPiece[self.railPieces.Count];
                self.railPieces.CopyTo(backup);

                self.segmentCount++;
                CreateRail();
                int index = 0;
                foreach (RailPiece r in self.railPieces)
                {
                    foldouts.Add(false);
                    edits.Add(false);
                    if (backup != null && index < backup.Length)
                    {
                        r.bend = backup[index].bend;
                    }

                    index++;
                }
            }
            EditorGUILayout.EndHorizontal();
            //base.OnInspectorGUI();
            if (GUILayout.Button("Destroy rail"))
            {
                DestroyRail();
                self.segmentCount = 0;
            }
            ((RailBuilder)this.target).Refresh();
            if (self.railPieces != null)
            {
                int j = 0;
                foreach (RailPiece r in self.railPieces)
                {
                    foldouts[j] = EditorGUILayout.Foldout(foldouts[j], "Rail Piece " + j);
                    if (foldouts[j])
                    {
                        r.bend = EditorGUILayout.Vector3Field("Rail Bend", r.bend);
                        /*if (GUILayout.Button(edits[j] ? "Finish" : "Edit"))
                        {
                            if (!edits[j])
                            {
                                for (int i = 0; i < edits.Count; i++)
                                {
                                    edits[i] = false;
                                }

                                edits[j] = true;

                            }
                            else
                            {
                                edits[j] = false;
                            }

                        }*/
                        /*if(GUILayout.Button("Create Junction"))
                        {
                            GameObject n = new GameObject("Junction");
                            n.transform.parent = r.bones[r.bones.Count-1];
                            n.transform.localPosition = Vector3.zero;
                            n.transform.rotation = r.bones[r.bones.Count - 1].rotation * Quaternion.Euler(90,0,0);
                            n.AddComponent<RailBuilder>();
                            n.GetComponent<RailBuilder>().prefab = self.prefab;
                            Selection.activeGameObject = n;
                        }*/

                    }
                    j++;
                }
            }
            ((RailBuilder)this.target).Refresh();

        }

        private void OnSceneGUI()
        {
            RailBuilder self = (RailBuilder)target;
            if (self.railPieces == null)
            {
                return;
            }

            int j = 0;
            foreach (RailPiece r in self.railPieces)
            {
                if (r.bones[0] != null)
                {

                    GUIStyle style = new GUIStyle();

                    style.fontSize = 25;
                    Handles.Label(r.bones[0].position, j.ToString(),style);

                }
                j++;
            }
        }


    }
}
