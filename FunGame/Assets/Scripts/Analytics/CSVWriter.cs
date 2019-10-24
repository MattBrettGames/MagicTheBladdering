using UnityEngine;
using System.IO;

public static class CSVWriter
{
    /*
    private static string reportDirectoryName = "Reports";
    private static string reportFileName = "data.csv";
    private static string reportSeparator = ",";
    private static string[] reportHeaders = new string[5]
    {
        "Character",
        "Skin",
        "Map",
        "",
        ""
    };

    static void CreateReport()
    {
        VerifyDirectory();
        using (StreamWriter sw = File.CreateText(GetFilePath()))
        {
            string finalString = "";
            for(int i = 0; i < reportHeaders.Length; i++)
            {
                if(finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += reportSeparator;
            }
            sw.WriteLine(finalString);            
        }
    }

    public static void AppendToReport(string[] strings)
    {
        VerifyDirectory();
        VerifyFile();
        using(StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = "";
            for(int i = 0; i < strings.Length; i++)
            {
                if(finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += strings[i];
            }
        }
    }

    static void VerifyDirectory()
    {
        string dir = GetDirectoryPath();
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    static void VerifyFile()
    {
        string file = GetFilePath();
        if (!File.Exists(file))
        {
            CreateReport();
        }
    }

    static string GetDirectoryPath()
    {
        return Application.dataPath + "/" + reportDirectoryName;
    }

    static string GetFilePath()
    {
        return GetDirectoryPath() + "/" + reportFileName;
    }
    */

}