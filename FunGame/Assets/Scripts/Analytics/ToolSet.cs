using UnityEngine;
using UnityEditor;

public static class ToolSet 
{
    
    //[MenuItem("ToolSet/Analytics/Export to CSV %F1")]

    static void DEV_AppendToReport()
    {
        CSVWriter.AppendToReport(new string[5] { "Valderheim","Undies","Songbird","Radioactive","TestArena"});
    }
    
}
