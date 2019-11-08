using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsController : MonoBehaviour
{

    public string character1;
    public string skin1;
    public string character2;
    public string skin2;
    public string map;
    public string genSuccess;
    public float curTime;

    public void CreateCSV()
    {
        CSVWriter.AppendToReport(new string[7] { character1, skin1, character2, skin2, map,  genSuccess, curTime.ToString()});
    }
}