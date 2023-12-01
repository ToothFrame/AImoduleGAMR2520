using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMLevelScript : MonoBehaviour
{
    UIControllerScript uiController;

    public GameObject[] levelComponents;
    private const int width = 30;
    private const int height = 33;
    private const int nNodes = width * height;
    private string levelString;
    char[,] map;
    bool[,] adjmat;// Adjacency matrix 
    GameObject[,] nodePos;
    List<GameObject> nodeList = new List<GameObject>();
    public bool searchingPath;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.Find("ConsoleCanvas").GetComponent<UIControllerScript>(); //IGNORE

        ReadLevelFile();
        StoreMap();
        InitAdjMat();
        DrawLevelBoard();
        CompleteAdjMat();
        PrintAdjMatTraversalTotal();
    }


    //Read Level file
    void ReadLevelFile()
    {
        levelString = "";
        levelString = FileReader.ReadString("Level1.txt");
    }

    //Store Map Using levelstring
    void StoreMap()
    {
        map = new char[width, height];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                map[j, i] = levelString[((i * width) + j)];
            }
        }
    }

    // Initialise the adjacency matrix
    void InitAdjMat()
    {
        adjmat = new bool[nNodes, nNodes];
        // Initialise all nodes to zero
        for (int i = 0; i < nNodes; i++)
        {
            for (int j = 0; j < nNodes; j++)
            {
                adjmat[i, j] = false;
            }
        }
    }

    /* Draw the map

The map is represented by an array of characters
Each of this characters is represented by a 19x19 pixel square
Each map square is drawn differently dependant on the character in the map
0 - nothing
1 - a dot
P - proton pill
- - horizontal line
| - vertical line
/ - top left corner
\ - top right corner
L - bottom left corner
I - bottom right corner
= - horizonal pink line

*/
    void DrawLevelBoard()
    {
        Vector2 boardPos = new Vector2(0, 0);
        nodePos = new GameObject[width, height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                boardPos.x = (j * 0.25f) - 8;
                boardPos.y = (i * -0.25f) + 4;
                switch (map[j, i])
                {
                    case '0': // Draw blank
                        nodePos[j, i] = Instantiate(levelComponents[0], boardPos, levelComponents[0].transform.rotation, transform);
                        nodeList.Add(nodePos[j, i]);

                        break;
                    case '1': // Draw edible dot
                        nodePos[j, i] = Instantiate(levelComponents[1], boardPos, levelComponents[1].transform.rotation, transform);
                        nodeList.Add(nodePos[j, i]);

                        break;
                    case 'P': // Draw a proton pill
                        nodePos[j, i] = Instantiate(levelComponents[2], boardPos, levelComponents[2].transform.rotation, transform);
                        nodeList.Add(nodePos[j, i]);

                        break;
                    case '-': // Draw a horizontal line
                        nodePos[j, i] = Instantiate(levelComponents[3], boardPos, levelComponents[3].transform.rotation, transform);
                        nodeList.Add(nodePos[j, i]);

                        break;
                    case '=': // Draw a horizontal line with the alternative colour
                        nodePos[j, i] = Instantiate(levelComponents[4], boardPos, levelComponents[4].transform.rotation, transform);
                        nodeList.Add(nodePos[j, i]);

                        break;
                    case '|': // Draw a vertical line
                        nodePos[j, i] = Instantiate(levelComponents[5], boardPos, levelComponents[5].transform.rotation, transform);
                        nodeList.Add(nodePos[j, i]);

                        break;
                    case '/': // Draw a top left corner
                        nodePos[j, i] = Instantiate(levelComponents[6], boardPos, levelComponents[6].transform.rotation, transform);
                        nodeList.Add(nodePos[j, i]);

                        break;
                    case '\\': // Draw a top right corner
                        nodePos[j, i] = Instantiate(levelComponents[7], boardPos, levelComponents[7].transform.rotation, transform);
                        nodeList.Add(nodePos[j, i]);

                        break;
                    case 'L': // Draw a bottom left corner
                        nodePos[j, i] = Instantiate(levelComponents[8], boardPos, levelComponents[8].transform.rotation, transform);
                        nodeList.Add(nodePos[j, i]);

                        break;
                    case 'I': // Draw a bottom right corner
                        nodePos[j, i] = Instantiate(levelComponents[9], boardPos, levelComponents[9].transform.rotation, transform);
                        nodeList.Add(nodePos[j, i]);

                        break;
                }

            }
        }
    }


    void CompleteAdjMat()
    {
        /* Test for adjacency	*/
        for (int i = 0; i < height; i++)
        {
            // Traverse the map top to bottom...
            for (int j = 0; j < width; j++)
            {
                // ...and left to right
                if (Traversable(map[j, i])) // Current square is traversable
                {
                    // Check if the square to the left is traversable
                    // if there is a square to the left
                    // and the square to the left is traversable
                    // Mark the path in adjacency matrix
                    // Check if the square to the right is traversable
                    // if there is a square to the right
                    // and the square to the right is traversable
                    // mark path in adjacency matrix
                    // Check if the square above is traversable
                    // there is a square above
                    // Square above is traversable
                    // Mark path in adjacency matrix
                    // Check if the square below is traversable
                    // there is a square below
                    // Square above is traversable
                    // Mark path in adjacency matrix
                    // If the square being the traversed is the
                    // left most column
                    // right most column
                    // top most row
                    // bottom most row
                    // Check for wrap around passages horizontally and vertically


                    // Position in matrix = number of rows traversed times row size + position in the column
                    int position = i * width + j;

                    // Check if the square to the left is traversable
                    // if there is a square to the left
                    if (j > 0)
                    {
                        // and the square to the left is traversable
                        if (Traversable(map[j - 1, i]))
                        {
                            // Mark the path in adjacency matrix
                            adjmat[position, position - 1] = true;
                        }
                    }
                    // Check if the square to the right is traversable
                    // if there is a square to the right
                    if (j < width - 1)
                    {
                        // and the square to the right is traversable
                        if (Traversable(map[j + 1, i]))
                        {
                            // Mark the path in adjacency matrix
                            adjmat[position, position + 1] = true;
                        }
                    }
                    // Check if the square above is traversable
                    // there is a square above
                    if (i > 0)
                    {
                        // Square above is traversable
                        if (Traversable(map[j, i - 1]))
                        {
                            int abovePosition = (i - 1) * width + j;
                            // Mark the path in adjacency matrix
                            adjmat[position, abovePosition] = true;
                        }
                    }

                    // Check if the square below is traversable
                    // there is a square below
                    if (i < height - 1)
                    {
                        // Square below is traversable
                        if (Traversable(map[j, i + 1]))
                        {
                            int belowPosition = (i + 1) * width + j;
                            // Mark the path in adjacency matrix
                            adjmat[position, belowPosition] = true;
                        }
                    }

                    // If the square being the traversed is the
                    // Check for wrap around passages horizontally and vertically
                    // left most column
                    if (j == 0)
                    {
                        if (Traversable(map[width - 1, i]))
                        {
                            // Mark the path in adjacency matrix
                            adjmat[position, position + width - 1] = true;
                        }
                    }
                    // right most column
                    if (j == width - 1)
                    {
                        if (Traversable(map[0, i]))
                        {
                            // Mark the path in adjacency matrix
                            adjmat[position, position - width + 1] = true;
                        }
                    }
                    // top most row
                    if (i == 0)
                    {
                        if (Traversable(map[j, height - 1]))
                        {
                            // Mark the path in adjacency matrix
                            adjmat[position, position + width * (height - 1)] = true;
                        }
                    }
                    // bottom most row
                    if (i == height - 1)
                    {
                        if (Traversable(map[0, j]))
                        {
                            // Mark the path in adjacency matrix
                            adjmat[position, j] = true;
                        }
                    }

                }

            }
        }
    }

    public bool Traversable(char pos)
    {
        // Yes if it's empty, a dot or a proton pill, no otherwise
        return (pos == '0' || pos == '1' || pos == 'P');
    }

    public void PMTouchingNode(int x, int y)
    {
        nodePos[y, x].GetComponent<SpriteRenderer>().sprite = levelComponents[0].GetComponent<SpriteRenderer>().sprite;
    }

    void PrintAdjMatTraversalTotal()
    {
        int traversalTotal = 0;

        //Check all nodes for traverability to another
        for (int i = 0; i < nNodes; i++)
        {
            for (int j = 0; j < nNodes; j++)
            {
                //if node i can traverse to node j
                if (adjmat[i, j] == true)
                {
                    //count it
                    traversalTotal++;
                }
            }

        }

        uiController.PrintLine("Total Traversable Nodes: " + traversalTotal);
    }

    public List<int> BFSPath(int rootNode, int goalNode)
    {

        searchingPath = true;

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

            //if the current node is our goal, stop searching. 
            if (currentNode == goalNode)
            {
                break;
            }

            //Iterate through the nodes
            for (int i = 0; i < nNodes; i++)
            {
                //if currentNode can visit node i...
                if (adjmat[currentNode, i])
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

        int j = goalNode;

        do
        {
            //add j node into path list
            path.Add(j);
            
            if (parentNode[j] == 0)
            {
                break;
            }
            //j now equals the parent of current j
            j = parentNode[j];
        }
        while (j != -1);


        searchingPath = false;
        return path;

    }

    public bool[,] GetAdjMat
    {
        get
        {
            return adjmat;
        }
    }

    public GameObject[,] GetNodePos
    {
        get
        {
            return nodePos;
        }
    }

    public int GetWidth
    {
        get
        {
            return width;
        }
    }

    public int GetHeight
    {
        get
        {
            return height;
        }
    }

    public GameObject GetNodeGameObject(int nodePos)
    {
        return nodeList[nodePos];
    }

    public char GetMapChar(int j, int i)
    {
        return map[j, i];
    }
}