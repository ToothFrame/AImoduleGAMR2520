using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAdjMatScript : MonoBehaviour
{
    UIControllerScript uiController;



    bool[,] adjmat = new bool[8,8];// Adjacency matrix for eight nodes

    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.Find("ConsoleCanvas").GetComponent<UIControllerScript>();

        InitAdjMat();
        PrintAdjMat();
    }


    void InitAdjMat()
    {
        // Initialise the adjacency matrix
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                adjmat[i,j] = false;
            }
        }


        //Set Up AdjMat

        ///////////////////////////////////////


    }


    void PrintAdjMat()
    {
        // Print the adjacency matrix

        uiController.Print("         0   1   2   3   4   5   6   7");
        uiController.NewLine();
        uiController.NewLine();


        int traversalTotal = 0;


        for (int i = 0; i < 8; i++)
        {
            uiController.Print(i + "       ");
            for (int j = 0; j < 8; j++)
            {
                if(adjmat[i, j] == true)
                {
                    uiController.Print("1   ");
                    traversalTotal++;
                }
                else
                {
                    uiController.Print("0   ");
                }
            }
            uiController.NewLine();
        }

        uiController.PrintLine("Total Traversable Nodes: " + traversalTotal);
    }
}
