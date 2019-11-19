    
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class ToolSet 
{
    [MenuItem("ToolSet/Analytics/Export to CSV %F1")]

    static void DEV_AppendToReport()
    {
        CSVWriter.AppendToReport(new string[5] { "Valderheim","Undies","Songbird","Radioactive","TestArena"});
    }
    
    [MenuItem("ToolSet/Workflow/Open Main Menu %F2")]
    static void ReturnToMenu()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
    }

    [MenuItem("ToolSet/Workflow/Open PvP Characters %F3")]
    static void PVPMenu()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MenuScenes/2CharacterselectorPvP.unity");

    }
    
}

