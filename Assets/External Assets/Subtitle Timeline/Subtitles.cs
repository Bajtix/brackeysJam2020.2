using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Subtitles : MonoBehaviour
{
    public void UpdateSub(string s)
    {
        if(GetComponent<TextMeshProUGUI>() != null)
            GetComponent<TextMeshProUGUI>().text = s;

        if (GetComponent<Text>() != null)
            GetComponent<Text>().text = s;
    }
}
