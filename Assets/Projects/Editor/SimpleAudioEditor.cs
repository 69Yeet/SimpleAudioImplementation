using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

public class SimpleAudioEditor : EditorWindow
{
    private string sfxName;
    private string pluginPath = "Assets/Audio/";
    private string sfxPath = "SFX";
    private string musicPath = "Music";
    private string vaPath = "VA";

    [MenuItem("Window/Audio/Simple Audio Emplementation")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SimpleAudioEditor));
    }
    private void OnGUI()
    {
        sfxName = EditorGUILayout.TextField("SFX Group Name", sfxName);
        if (GUILayout.Button("Add SFX Group"))
        {
            //Debug.Log(sfxName);
            CreateFolder(sfxPath, sfxName);
        }

        if (GUILayout.Button("Refresh Sound List"))
        {
            RefreshEnum();
        }
    }
    private void CreateFolder(string soundType, string folderName)
    {
        if(folderName.Contains(" "))
        {
            Debug.LogWarning("Cannot create a sound with space, please remove the space or change the name.");
            return;
        }
        if (!Directory.Exists(pluginPath + soundType + "/" + folderName))
        {
            AssetDatabase.CreateFolder(pluginPath + soundType, folderName);
        }
        else
        {
            Debug.LogWarning("Please write a nonexistant sound, or increment it without spaces.");
        }
        //string guid = AssetDatabase.CreateFolder(pluginPath + soundType + folderName, $"New {soundType}");
        //string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
        //Debug.Log(newFolderPath);
    }

    private void RefreshEnum()
    {
        string enumDir = String.Join("," + Environment.NewLine + "    ", GetAllFolders());

        Debug.Log(enumDir);

        if (File.Exists(pluginPath + "/SoundList.cs"))
        {
            File.Delete(pluginPath + "/SoundList.cs");
            using (FileStream fs = File.Create(pluginPath + "/SoundList.cs"))
            {
                AddText(fs, "public enum Sound" + Environment.NewLine +
                            "{" + Environment.NewLine +
                            "    " + enumDir + Environment.NewLine +
                            "}");
            }
        }

        //GetAllFolders();

        /*string pastFileString = String.Join(Environment.NewLine, RemoveLastLine(pluginPath + "/SoundList.cs"));

        using (FileStream fs = File.OpenWrite(pluginPath + "/SoundList.cs"))
        {
            AddText(fs,
                        pastFileString +
                        "," +
                       $"\r\n   {musicPath}" +
                        "\r\n}");
        }*/
        AssetDatabase.Refresh();

    }

    private void AddText(FileStream fs, string value)
    {
        byte[] info = new UTF8Encoding(true).GetBytes(value);
        fs.Write(info, 0, info.Length);
    }

    private List<string> RemoveLastLine(string enumPath)
    {
        List<string> lines = File.ReadLines(enumPath).ToList();
        lines.RemoveAt(lines.Count - 1);
        return lines;
        /*File.WriteAllLines(enumPath, lines);
        return lines.Count - 1;*/
    }

    private string[] GetAllFolders()
    {
        List<string> dir = new List<string>(Directory.EnumerateDirectories(pluginPath + sfxPath));
        string[] allFolders = new string[dir.Count];

        for(int i = 0; i < dir.Count; i++)
        {
            allFolders[i] = dir[i].Remove(0, (pluginPath + sfxPath).Length + 1);
        }

        return allFolders;

        /*foreach(string folder in allFolders)
        {
            Debug.Log(folder);
        }*/
    }
}
