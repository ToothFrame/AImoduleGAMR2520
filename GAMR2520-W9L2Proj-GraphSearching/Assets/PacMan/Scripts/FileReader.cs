using UnityEngine;
using UnityEditor;
using System.IO;

public static class FileReader
{
    [MenuItem("Tools/Read file")]
    public static string ReadString(string level)
    {
        string path = "Assets/PacMan/Resources/" + level;
        string textHolder;
        string text = "";
        StreamReader reader = new StreamReader(path);
        textHolder = reader.ReadToEnd();
        reader.Close();

        foreach(char c in textHolder)
        {
            if(
                c.ToString() == "\\"||
                c.ToString() == "/" ||
                c.ToString() == "-" ||
                c.ToString() == "|" ||
                c.ToString() == "L" || 
                c.ToString() == "I" ||
                c.ToString() == "=" ||
                c.ToString() == "0" ||
                c.ToString() == "1" ||
                c.ToString() == "P"
                )
            {
                text += c;
            }
        }

        return text;
    }

}