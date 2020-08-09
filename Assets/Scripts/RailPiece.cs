using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
