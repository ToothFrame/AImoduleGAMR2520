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
                map[j, i] = levelString[((i * width) + j)]; // assign level1 values to map matrix
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
                adjmat[i,j] = false;  // set all values of the adjacency matrix (990*990) to faLse
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
                        break;
                    case '1': // Draw edible dot
                        nodePos[j, i] = Instantiate(levelComponents[1], boardPos, levelComponents[1].transform.rotation, transform);
                        break;
                    case 'P': // Draw a proton pill
                        nodePos[j, i] = Instantiate(levelComponents[2], boardPos, levelComponents[2].transform.rotation, transform);
                        break;
                    case '-': // Draw a horizontal line
                        nodePos[j, i] = Instantiate(levelComponents[3], boardPos, levelComponents[3].transform.rotation, transform);
                        break;
                    case '=': // Draw a horizontal line with the alternative colour
                        nodePos[j, i] = Instantiate(levelComponents[4], boardPos, levelComponents[4].transform.rotation, transform);
                        break;
                    case '|': // Draw a vertical line
                        nodePos[j, i] = Instantiate(levelComponents[5], boardPos, levelComponents[5].transform.rotation, transform);
                        break;
                    case '/': // Draw a top left corner
                        nodePos[j, i] = Instantiate(levelComponents[6], boardPos, levelComponents[6].transform.rotation, transform);
                        break;
                    case '\\': // Draw a top right corner
                        nodePos[j, i] = Instantiate(levelComponents[7], boardPos, levelComponents[7].transform.rotation, transform);
                        break;
                    case 'L': // Draw a bottom left corner
                        nodePos[j, i] = Instantiate(levelComponents[8], boardPos, levelComponents[8].transform.rotation, transform);
                        break;
                    case 'I': // Draw a bottom right corner
                        nodePos[j, i] = Instantiate(levelComponents[9], boardPos, levelComponents[9].transform.rotation, transform);
                        break;
                }
                
            }
        }
    }


    void CompleteAdjMat()
    {
        /* Test for adjacency	*/
        for (int i = 0; i < height; i++) // Traverse the map top to bottom...
        {
            for (int j = 0; j < width; j++) // ...and left to right
            {
                if (Traversable(map[j, i])) // Current square is traversable
                {
                    int position = i * width + j;
                    if (j > 0)
                    {
                        if (Traversable(map[j - 1, i])) // Check if the square to the left is traversable
                        {
                            adjmat[position, position - 1] = true;
                        }
                    }

                    if (j < width - 1) 
                    {
                        if (Traversable(map[j + 1, i])) // Check if the square to the right is traversable
                        {
                            adjmat[position, position + 1] = true;
                        }
                    }

                    if (i > 0)
                    {
                        if (Traversable(map[j, i - 1]))// Check if the square above is traversable
                        {
                            adjmat[position, (i - 1) * width + j] = true;
                        }
                    }

                    if(i<height - 1) 
                    {
                        if(Traversable(map[j, i + 1])) // Check if the square below is traversable
                        {
                            adjmat[position + 1, (i + 1) * width + j] = true;
                        }
                    }

                    if(j == 0) // If the square being the traversed is the left most column
                    {
                        if (Traversable(map[width - 1, i]))
                        {
                            adjmat[position, position + width - 1] = true;
                        }
                    }

                    if(j == width -1) // right most column
                    {
                        if (Traversable(map[0, i])) // if the square at the opposite side is traversible
                        {
                            adjmat[position, position - width + 1] = true;
                        }
                    }
                    
                    if(i == 0) // top most row
                    {
                        if (Traversable(map[j, 0]))
                        {
                            adjmat[position, position + width * (height - 1)] = true; // multiplying width by height -1 selects the first square of the last row so by adding position we get the correct square
                        }
                    }

                    if(i == height - 1) // bottom most row
                    {
                        if (Traversable(map[j, 0]))
                        {
                            adjmat[position, j] = true;
                        }
                    }               
                    // Check for wrap around passages horizontally and vertically
                }
            }
        }
    }

    bool Traversable(char pos)
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
                if(adjmat[i, j] == true)
                {
                    //count it
                    traversalTotal++;
                }
            }

        }

        uiController.PrintLine("Total Traversable Nodes: " + traversalTotal);
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
}