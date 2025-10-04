using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Collections.Generic;

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
            Debug.Log(sfxName);
            //CreateFolder(sfxPath, sfxName);
        }

        if (GUILayout.Button("Refresh Sound List"))
        {
            RefreshEnum();
        }
    }
    private void CreateFolder(string soundType, string folderName)
    {
        AssetDatabase.CreateFolder(pluginPath + soundType, folderName);
        string guid = AssetDatabase.CreateFolder(pluginPath + soundType + folderName, $"New {soundType}");
        string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
        Debug.Log(newFolderPath);
    }

    private void RefreshEnum()
    {
        if (!File.Exists(pluginPath + "/SoundList.cs"))
        {
            using (FileStream fs = File.Create(pluginPath + "/SoundList.cs"))
            {
                AddText(fs, "public enum Sound" + Environment.NewLine +
                            "{" + Environment.NewLine +
                            "   " + sfxPath + Environment.NewLine +
                            "}");
            }
        }

        RemoveLastLine(pluginPath + "/SoundList.cs");

        using (FileStream fs = File.OpenWrite(pluginPath + "/SoundList.cs"))
        {
            AddText(fs, "," +
                       $"\r\n   {musicPath}" +
                        "\r\n}");
        }
        AssetDatabase.Refresh();

    }

    private void AddText(FileStream fs, string value, int? lineLength = null)
    {
        byte[] info = new UTF8Encoding(true).GetBytes(value);
        fs.Write(info, (lineLength.HasValue ? lineLength: 0), info.Length);
    }

    private void RemoveLastLine(string enumPath)
    {
        List<string> lines = File.ReadLines(enumPath).ToList();
        lines.RemoveAt(lines.Count - 1);
        File.WriteAllLines(enumPath, lines);
    }
}
