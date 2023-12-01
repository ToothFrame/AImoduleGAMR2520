using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AStarShip : MonoBehaviour
{

    Node[,] grid;
    Vector2 gridSize;
    LayerMask obstacleLayer;
    float nodeSize = 0.1f;
    List<Node> openSet;
    List<Node> closedSet;
    Node currentNode;
    Vector2 gridNodes;
    List<Node> path;
    bool pathFound;
    bool searching = false;
    Vector3 rootNodePos, goalNodePos;
    public bool showPath = true;
    private void Start()
    {
        //Set grid size based on Gameobject scale (plane)
        gridSize = transform.localScale * 10f;
        //Set gridNodes, how many nodes along x, how many along y
        gridNodes = new Vector2(Mathf.RoundToInt(gridSize.x / nodeSize), Mathf.RoundToInt(gridSize.y / nodeSize));
        //layermark for obstacle search
        obstacleLayer = LayerMask.GetMask("Obstacle");
        //Create grid function
        CreateGrid();
    }

    //returns the path from objectA to objectB
    public List<Node> RequestPath(GameObject objectA, GameObject objectB)
    {
        if (searching)
        {
            AStarPathFind();
        }
        //set rootNodePos position and goalNode
        rootNodePos = objectA.transform.position;
        goalNodePos = objectB.transform.position;

        //redo the grid with CreateGrid function (in case of moved root goal or obstacles)
        CreateGrid();

        //run path search
        AStarPathFind();

        return path;
    }

    void CreateGrid()
    {
        //Create 2d matrix to store nodes
        grid = new Node[(int)gridNodes.x, (int)gridNodes.y];

        //location bottom left of the grid space
        Vector3 gridBottomLeft = transform.position - Vector3.right * gridSize.x / 2
                                                    - Vector3.forward * gridSize.y / 2;

        //for each node in x axis
        for (int i = 0; i < gridNodes.x; i++)
        {
            //for each node in y axis
            for (int j = 0; j < gridNodes.y; j++)
            {
                //find position in the grid this node needs to be
                Vector3 nodePos = gridBottomLeft + Vector3.right * (i * nodeSize + (nodeSize / 2))
                                                 + Vector3.forward * (j * nodeSize + (nodeSize / 2));


                //cheack for obstacles, is it traverable?
                //Adjacancy Matrix could also be used to add traversability
                bool traversable = !(Physics.CheckSphere(nodePos, nodeSize / 2, obstacleLayer));

                //Add node to grid.
                grid[i, j] = new Node(nodePos, traversable, i, j);
            }
        }
    }

    //Function uses transform.position to return node in grid matrix.
    public Node NodePositionInGrid(Vector3 gridPosition)
    {
        float pX = Mathf.Clamp01((gridPosition.x - ((gridSize.x / gridNodes.x) / 2) + (gridSize.x / 2)) / gridSize.x);
        float pY = Mathf.Clamp01((gridPosition.z - ((gridSize.y / gridNodes.y) / 2) + (gridSize.y / 2)) / gridSize.y);

        int nX = (int)Mathf.Clamp(Mathf.RoundToInt(gridNodes.x * pX), 0, gridNodes.x - 1);
        int nY = (int)Mathf.Clamp(Mathf.RoundToInt(gridNodes.y * pY), 0, gridNodes.y - 1);

        return grid[nX, nY];
    }


    void AStarPathFind()
    {
        //Get Root Node (ship)
        Node rootNode = NodePositionInGrid(rootNodePos);
        //Get Target Node (Player)
        Node goalNode = NodePositionInGrid(goalNodePos);
        //Create set for open nodes
        openSet = new List<Node>();
        //Create set for closed nodes
        closedSet = new List<Node>();
        //Has path been found
        pathFound = false;
        //is searching?
        searching = true;
        //Add root node to open set
        openSet.Add(rootNode);
        //set currentnode variable
        currentNode = new Node(Vector3.zero, false, -1, -1);
        //store new move costs
        float newMoveCost;
        while (openSet.Count > 0 && !pathFound && searching)
        {

            //Set the first node from the list to current node
            currentNode = openSet[0];

            //we check the rest of the nodes 
            for (int i = 1; i < openSet.Count; i++)
            {
                //if the node has less fCost than the current node, or the same but less Hcost
                if (openSet[i].f < currentNode.f || (openSet[i].f == currentNode.f && openSet[i].h < currentNode.h))
                {
                    currentNode = openSet[i];
                }


            }

            //as we are checking current node, remove the currnet node from the open set
            openSet.Remove(currentNode);

            //put it into the closed set
            closedSet.Add(currentNode);

            //if its the goal node, we are done
            if (currentNode == goalNode)
            {
                //retrace the path from root to goal;
                RetracePath(rootNode, goalNode);
                //Path is found
                pathFound = true;
                //No Longer Searching
                searching = false;

                break;
            }
            else
            {
                //For each neighbour of the current node
                foreach (Node neighbour in GetNeighbours(currentNode))
                {
                    //if we cannot traverse to the neighbour, or is in the close set
                    if (!neighbour.traversable || closedSet.Contains(neighbour))
                    {
                        //skip this loop
                        continue;
                    }

                    //Calculate the move to neightbor gCost using Hueristic (use getdistance)
                    newMoveCost = GetDistance(currentNode, neighbour) + currentNode.g;

                    //if the new move costs less or this neighbour isnt in the open set
                    if (newMoveCost < neighbour.g || !openSet.Contains(neighbour))
                    {

                        //store (new) move cost
                        neighbour.g = newMoveCost;

                        //store heuristic hCost from neighbour to goal
                        neighbour.h = GetDistance(neighbour, goalNode);

                        //store parent node for path retrace
                        neighbour.parentNode = currentNode;

                        //if we dont have this neighbour in the open set
                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }

                    }

                }
            }
        }
        searching = false;
    }

    //returns path
    void RetracePath(Node rNode, Node gNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = gNode;

        while (currentNode != rNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        path.Reverse();
        this.path = path;
    }

    //searchs neighbours
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                int pX = node.x + i;
                int pY = node.y + j;

                if (pX >= 0 && pX < gridNodes.x && pY >= 0 && pY < gridNodes.y)
                {
                    neighbours.Add(grid[pX, pY]);
                }
            }
        }
        return neighbours;
    }

    //returns distance between nodeA and nodeB based on heuristic class
    public float GetDistance(Node nodeA, Node nodeB)
    {
        float rValue = 0;
        rValue = Heuristic.GetDistanceEuclidean(nodeA, nodeB);
        return rValue;
    }


    //visuals
    private void OnDrawGizmos()
    {
        if (showPath)
        {
            if (grid != null)
            {
                foreach (Node node in grid)
                {
                    if (path != null && pathFound)
                    {
                        foreach (var item in path)
                        {
                            if (item == node)
                            {
                                Gizmos.color = new Color(0, 0, 0.5f, 0.5f);
                                Gizmos.DrawCube(node.nodePos, new Vector3(nodeSize * 0.9f, 0.1f, nodeSize * 0.9f));
                            }
                        }
                    }
                }
            }
        }
    }

}
