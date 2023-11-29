using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerBFSScript : MonoBehaviour
{
    public BFSScript BFSScrpt;
    string[] consoleString = new string[20];
    Text consoleText;
    int currentLine = 0;
    // Start is called before the first frame update
    void Start()
    {
        consoleText = GameObject.Find("ConsoleText").GetComponent<Text>();
    }

    public void Print(string stg)
    {
        if(currentLine < consoleString.Length)
        {
            consoleString[currentLine] += stg;
        }
        else
        {
            consoleString[consoleString.Length - 1] += stg;
        }
        UpdateConsoleText();
    }

    public void PrintLine(string stg)
    {
        if (currentLine < consoleString.Length)
        {
            consoleString[currentLine] += stg;
        }
        else
        {
            consoleString[consoleString.Length - 1] += stg;
        }
        UpdateConsoleText();
        NewLine();
    }


    public void NewLine()
    {
        currentLine++;

        if (currentLine >= consoleString.Length)
        { 
            for (int i = 1; i < consoleString.Length; i++)
            {
                consoleString[i - 1] = consoleString[i];

            }
            consoleString[consoleString.Length - 1] = "";
        }

        UpdateConsoleText();

    }

    void UpdateConsoleText()
    {
        StopCoroutine("UpdateConsoleWait");
        StartCoroutine("UpdateConsoleWait");
    }

    IEnumerator UpdateConsoleWait()
    {
        consoleText.text = "";
        for (int i = 0; i < consoleString.Length; i++)
        {
            consoleText.text += consoleString[i] + "\n";
            yield return new WaitForEndOfFrame();
        }
    }

    public void Clear()
    {
        for (int i = 0; i < consoleString.Length; i++)
        {
            consoleString[i] = "";
        }
        currentLine = 0;
        UpdateConsoleText();
    }

    public void ErrorInput()
    {
        NewLine();
        Print("Enter Valid Number in the Input Field");
        NewLine();
    }

    public void BFSearch(InputField inpF)
    {
        try
        {
            int input = Convert.ToInt16(inpF.text);
            if (input >= 0 && input <= 7)
            {
                BFSScrpt.BFS(input);
            }
            else
            {
                ErrorInput();
            }
        }
        catch
        {
            ErrorInput();
        }
    }
}
