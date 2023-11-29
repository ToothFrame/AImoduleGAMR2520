using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSScript : MonoBehaviour
{
    UIControllerBFSScript uiController; //IGNORE

    const int nNodes = 8; // Number of nodes (A, B, C, D, E, F, G, H)

    bool[,] adjMat = new bool[nNodes, nNodes];// Adjacency matrix for out nNodes

    // Start is called before the first frame update
    void Start()
    {
        //Set up UI Canvas controller
        uiController = GameObject.Find("ConsoleCanvas").GetComponent<UIControllerBFSScript>(); //IGNORE
        //Initialise Adjacency Matrix
        InitAdjMat();
        //Set up AdjMat
        SetAdjMat();
        //Run Breadth First Search, with root node D (3)
        BFS(3);
    }

    void InitAdjMat()
    {
        // Initialise the adjacency matrix with false
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                adjMat[i, j] = false;
            }
        }
    }

    void SetAdjMat()
    {
        // set which nodes can get to the other nodes
        // the first component is the node we are at, and the second is the node we can reach
        // A B C D E F G H
        // 0 1 2 3 4 5 6 7
        adjMat[0, 1] = true;

        adjMat[1, 0] = true;
        adjMat[1, 2] = true;
        adjMat[1, 3] = true;

        adjMat[2, 1] = true;

        adjMat[3, 1] = true;
        adjMat[3, 4] = true;
        adjMat[3, 6] = true;

        adjMat[4, 3] = true;
        adjMat[4, 5] = true;

        adjMat[5, 4] = true;
        adjMat[5, 6] = true;

        adjMat[6, 3] = true;
        adjMat[6, 5] = true;
        adjMat[6, 7] = true;

        adjMat[7, 6] = true;
    }

    public void BFS(int rootNode)
    {
        /* /////////////////////////////////////////////
         * 
         * 
         * 
         * This is an incomplete function
         * 
         * 
         * 
         * ////////////////////////////////////////////*/


        //Create an array list of type bool (bool[] visitedNodes), of size nNodes (8) to store if the node has been visited.
        bool[] visitedNodes = new bool[nNodes];

        //Create a Queue<int> to store the nodes to be checked
        Queue<int> nodeQueue = new Queue<int>();

        //Create an int to store the current node that we are looking at in the while loop.
        int currentNode = rootNode;



        //Initialise all visitedNodes elements to false.
        for (int i = 0; i < nNodes; i++)
        {
            visitedNodes[i]=false;
        }

        //Add the currentNode (which is the rootNode at this point) as the first node in the Queue
        nodeQueue.Enqueue(currentNode);
        //Set this node in the visitedNodes array to true
        visitedNodes[currentNode] = true;

        //While the nodeQueue still has elements.
        while (nodeQueue.Count > 0)
        {
            currentNode = nodeQueue.Peek();
            //Get the first element of the list nodeQueue, nodeQueue.Peek();

            //Remove this from the list, nodeQueue.Dequeue();
            nodeQueue.Dequeue();


            //print node i as ASCII character
            uiController.PrintLine(Convert.ToChar(currentNode + 65).ToString());

            //Iterate through the nodes
            for (int i = 0; i < nNodes; i++)
            {
                //if currentNode can visit node i...
                if (adjMat[currentNode, i])
                {
                    //and if this node i has not been visited, then...
                    if (!visitedNodes[i])
                    {
                        //Add node i to the list
                        nodeQueue.Enqueue(i);

                        //set node i to visited.
                        visitedNodes[i] = true;

                    }
                }
            }

        }
        //Done
        uiController.PrintLine("BFS Done! Root Node: " + Convert.ToChar(rootNode + 65).ToString());
        uiController.PrintLine("----------------------- \n");
    }

}
