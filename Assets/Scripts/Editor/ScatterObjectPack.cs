using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Asset Pack",menuName = "Game/AssetPack")]
public class ScatterObjectPack : ScriptableObject
{
    public GameObject[] objects;
    public bool randomiseMaterials;
    public Material[] randomMaterials;
}
