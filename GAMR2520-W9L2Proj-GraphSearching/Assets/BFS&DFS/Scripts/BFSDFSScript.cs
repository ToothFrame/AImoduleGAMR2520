using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSDFSScript : MonoBehaviour
{


    UIControllerScript uiController; //IGNORE

    const int nNodes = 8; // Number of nodes (A, B, C, D, E, F, G, H)

    bool[,] adjMat = new bool[nNodes, nNodes];// Adjacency matrix for out nNodes

    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.Find("ConsoleCanvas").GetComponent<UIControllerScript>(); //IGNORE

        InitAdjMat();
        SetAdjMat();

        uiController.PrintLine("Input node into input field and select search type");
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
        //Create an array list of type bool, of size nNodes (8) to store if the node has been visited.
        bool[] visitedNodes = new bool[nNodes];
        //Stores the nodes queue
        Queue<int> nodeQueue = new Queue<int>();
        //The current node in the while loop.
        int currentNode = rootNode;

        //Initialise all visitedNodes elements to false.
        for (int i = 0; i < nNodes; i++)
        {
            visitedNodes[i] = false;
        }

        //Add the currentNode (which is the rootNode at this point) as the first node
        nodeQueue.Enqueue(currentNode);
        //Set this nodes visit to true
        visitedNodes[currentNode] = true;

        //While the nodeQueue still has elements.
        while (nodeQueue.Count > 0)
        {
            //Get the first element of the list nodeQueue
            currentNode = nodeQueue.Peek();
            //Remove this from the list.
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
        uiController.PrintLine("BFS Done!");
        uiController.PrintLine("-----------------");
    }

    public void BFSPath(int rootNode, int goalNode)
    {
        //clear console
        uiController.Clear();

        //Create an array list of type bool, of size nNodes (8) to store if the node has been visited.
        bool[] visitedNodes = new bool[nNodes];
        //Stores the nodes queue
        Queue<int> nodeQueue = new Queue<int>();
        //The current node in the while loop.
        int currentNode = rootNode;


        //Store parent node for path finding. Used for BFS Path Finding - see BFSAdvanced Scene
        int[] parentNode = new int[nNodes];
        //set perant node of rootNode to -1 (no parent)
        parentNode[currentNode] = -1;
        //store the path
        List<int> path = new List<int>();
    


        //Initialise all visitedNodes elements to false.
        for (int i = 0; i < nNodes; i++)
        {
            visitedNodes[i] = false;
        }

        //Add the currentNode (which is the rootNode at this point) as the first node
        nodeQueue.Enqueue(currentNode);
        //Set this nodes visit to true
        visitedNodes[currentNode] = true;


        //While the nodeQueue still has elements.
        while (nodeQueue.Count > 0)
        {
            //Get the first element of the list nodeQueue
            currentNode = nodeQueue.Peek();

            //Remove this from the list.
            nodeQueue.Dequeue();

            //print node i as ASCII character
            uiController.PrintLine(Convert.ToChar(currentNode + 65).ToString());


            //if the current node is our goal, stop searching. 
            if (currentNode == goalNode)
            {
                break;
            }

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

                        //Store the parent of i as currentNode
                        parentNode[i] = currentNode;

                    }
                }
            }
        }
        //Done
        uiController.PrintLine("BFS Done!");
        uiController.NewLine();


        //Display path 
        uiController.PrintLine("Path");

        //Set goalNode
        int j = goalNode;
        //while parent node is not = -1 (set as parent of the root node)
        while (j != -1)
        {
            //add j node into path list
            path.Add(j);
            //j now equals the parent of current j
            j = parentNode[j];
        }

        //reverse the path list
        path.Reverse();

        //display each node for path 
        foreach (int item in path)
        {
            //print node i as ASCII character
            uiController.PrintLine(Convert.ToChar(item + 65).ToString());
        }

        uiController.PrintLine("-----------------");


    }

    public void DFS(int rootNode)
    {

        /*********************************************
        

        This functon is incomplete


        ********************************************/

        //Create an array list of type bool, of size nNodes (8) to store if the node has been visited.
        bool[] visitedNodes = new bool[nNodes];
        //Store the nodes in a stack, first in last out, last in first out
        Stack<int> nodeStack = new Stack<int>();

        //The current node in the while loop.
        int currentNode = rootNode;

        //Initialise all visitedNodes elements to false.
        for (int i = 0; i < nNodes; i++)
        {
            visitedNodes[i] = false;
        }


        // Visit the root node, and push it onto the stack
        visitedNodes[rootNode] = true;
        nodeStack.Push(rootNode);

        //while nodeStack has items
        while (nodeStack.Count != 0)

        {
            // Grab the top element off of the stack
            currentNode = nodeStack.Peek();

            // Remove it from the top of the stack
            nodeStack.Pop();

            //print node i as ASCII character
            uiController.PrintLine(Convert.ToChar(currentNode + 65).ToString());

            for (int i = 0; i < nNodes; i++)
            {
                // For each neighbour node
                if (adjMat[currentNode, i])
                {
                    // If it's not been visited
                    if (!visitedNodes[i])
                    {
                        //Add node i to the stack
                        nodeStack.Push(i);
                        //set node i to visited.
                        visitedNodes[i] = true;
                    }
                }
            }
        }

        //Done
        uiController.PrintLine("DFS Done!");
        uiController.PrintLine("-----------------");
    }

    public void DFSRecursiveCall(int rootNode)
    {
        bool[] vNodes = new bool[nNodes];

        //send root node to recursive function
        DFSRecursive(rootNode, vNodes);

        //Done
        uiController.PrintLine("DFS Recursive Done!");
        uiController.PrintLine("-----------------");
    }

    public void DFSRecursive(int cNode, bool[] visitedNodes)
    {

        /*********************************************


        This functon is incomplete


        ********************************************/

        //set current node as visited
        visitedNodes[cNode] = true;

        //print node i as ASCII character
        uiController.PrintLine(Convert.ToChar(cNode + 65).ToString());

        //for every node
        for (int i = 0; i < nNodes; i++)
        {
            if (adjMat[cNode, i]) 
            {
            if (!visitedNodes[i])
                {
                    DFSRecursive(i, visitedNodes);
                    // recursively call dfs using the unvisited node as the root
                }
            }
        }

    }

}
