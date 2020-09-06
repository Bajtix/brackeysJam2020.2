using System;
using System.Collections.Generic;
using UnityEngine;

public class RailBuilder : MonoBehaviour
{


    


    public List<RailPiece> railPieces;

    public int segmentCount;
    public GameObject prefab;


    public List<bool> editors;

    [ExecuteInEditMode]
    public void Refresh()
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


    
}
