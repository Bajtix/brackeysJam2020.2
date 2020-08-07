using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GlobalGameSettings
{
    public static bool epilepsyMode;

    public static int lastLevel = 1;

    public static void SaveProgress()
    {
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/save");
        sw.Write(lastLevel);
        sw.Close();
    }

    public static void LoadProgress()
    {
        lastLevel = 1;
        if (!File.Exists(Application.persistentDataPath + "/save")) return;
        StreamReader sr = new StreamReader(Application.persistentDataPath + "/save");
        lastLevel = int.Parse(sr.ReadToEnd());
        sr.Close();
    }
}
