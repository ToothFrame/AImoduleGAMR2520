using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AStar : MonoBehaviour
{

    public GameObject rootNodeGrid, goalNodeGrid;
    Node[,] grid;
    Vector2 gridSize;
    LayerMask obstacleLayer;
    [HideInInspector]
    public float nodeSize = 0.3f;
    List<Node> openSet;
    List<Node> closedSet;
    Node currentNode;
    Vector2 gridNodes;
    List<Node> path;
    bool allowDiagonal = true;
    [HideInInspector]
    public bool dynamicMode = false;
    [HideInInspector]
    public bool pathFound;
    [HideInInspector]
    public bool searching = false;
    bool showCostValue = false;
    bool pause = false;
    bool step = false;
    bool objVisible = true;
    public List<GameObject> gObjects = new List<GameObject>();
    float minCost = 0;
    float maxCost = 0;
    float searchSpeed = 0.1f;
    Button startStopSearchButton;
    Text outputTxt;
    public enum HeuristicMode
    {
        Euclidean, EuclideanNoSqr, Manhattan, Diagonal, DiagonalShort
    }

    HeuristicMode hMode;

    private void Start()
    {

        startStopSearchButton = GameObject.Find("StarStopBtn").GetComponent<Button>();
        outputTxt = GameObject.Find("OutputText").GetComponent<Text>();

        gridSize = transform.localScale * 10f;
        gridNodes = new Vector2(Mathf.RoundToInt(gridSize.x / nodeSize), Mathf.RoundToInt(gridSize.y / nodeSize));

        obstacleLayer = LayerMask.GetMask("Obstacle");

        if (nodeSize > 0.6f)
        {
            showCostValue = true;
        }
        else
        {
            showCostValue = false;
        }

        CreateGrid();
    }

    private void Update()
    {
        if (dynamicMode && !searching)
        {
            searchSpeed = 0f;
            CreateGrid();
            StartCoroutine(AStarPathFind(rootNodeGrid.transform.position, goalNodeGrid.transform.position));
        }
        else if (!dynamicMode && !searching && !pathFound)
        {
            CreateGrid();
        }


        if (searching)
        {
            outputTxt.text = "Searching... Open: " + openSet.Count + "  Closed: " + closedSet.Count;
        }
        else if (pathFound)
        {
            outputTxt.text = "Path Found! Open: " + openSet.Count + "  Closed: " + closedSet.Count;
        }
        else
        {
            outputTxt.text = "Stopped.";
        }

        GameObject.Find("Canvas").transform.Find("ShowCost").GetComponent<Toggle>().isOn = showCostValue;
        GameObject.Find("Canvas").transform.Find("DiagonalToggle").GetComponent<Toggle>().isOn = allowDiagonal;

    }

    void CreateGrid()
    {
        grid = new Node[(int)gridNodes.x, (int)gridNodes.y];
        Vector3 gridBottomLeft = transform.position - Vector3.right * gridSize.x / 2
                                                    - Vector3.up * gridSize.y / 2;

        for (int i = 0; i < gridNodes.x; i++)
        {
            for (int j = 0; j < gridNodes.y; j++)
            {
                Vector3 nodePos = gridBottomLeft + Vector3.right * (i * nodeSize + (nodeSize / 2))
                                                 + Vector3.up * (j * nodeSize + (nodeSize / 2));
                bool traversable = !(Physics.CheckSphere(nodePos, nodeSize / 2, obstacleLayer));
                grid[i, j] = new Node(nodePos, traversable, i, j);
            }
        }
    }

    public Node NodePositionInGrid(Vector3 gridPosition)
    {
        float pX = Mathf.Clamp01((gridPosition.x - ((gridSize.x / gridNodes.x) / 2) + (gridSize.x / 2)) / gridSize.x);
        float pY = Mathf.Clamp01((gridPosition.y - ((gridSize.y / gridNodes.y) / 2) + (gridSize.y / 2)) / gridSize.y);

        int nX = (int) Mathf.Clamp(Mathf.RoundToInt(gridNodes.x * pX), 0, gridNodes.x - 1);
        int nY = (int) Mathf.Clamp(Mathf.RoundToInt(gridNodes.y * pY), 0, gridNodes.y - 1);

        return grid[nX, nY];
    }

    public void AStarPathFindButton(Slider sldr)
    {
        searchSpeed = 0.1f;
        if (!searching)
        {
            pause = false;
            searchSpeed = sldr.value;
            step = false;
            CreateGrid();
            StartCoroutine(AStarPathFind(rootNodeGrid.transform.position, goalNodeGrid.transform.position));
            startStopSearchButton.gameObject.transform.Find("Text").GetComponent<Text>().text = "Stop";
        }
        else
        {
            pause = false;
            step = false;
            searching = false;
            startStopSearchButton.gameObject.transform.Find("Text").GetComponent<Text>().text = "Start";

        }
    }

    IEnumerator AStarPathFind(Vector3 rootNodePos, Vector3 goalNodePos)
    {
        /*//////////////////////////////////////////////////
        

        //        THIS FUNCTION IS INCOMPLETE


        //////////////////////////////////////////////////*/



        

        //Get the root node and set it to rootNode
        Node rootNode = NodePositionInGrid(rootNodePos);

        //Get the goal node and set it to goalNode
        Node goalNode = NodePositionInGrid(goalNodePos);

        //Create a new List<Node> set for open nodes
        openSet = new List<Node>();

        //Create a new List<Node> set for closed nodes
        closedSet = new List<Node>(); 

        //has path been found? 
        pathFound = false;

        //add root node to the open set
        openSet.Add(rootNode);

        //create a current node variable
        Node currentNode = rootNode;


        ////// IGNORE - UI STUFF /////
        if (!rootNode.traversable || !goalNode.traversable)
        {
            //is it searching
            searching = false;
        }
        else
        {
            searching = true;
        }
        //////////////////////////////


        // while there are still nodes in the open set
        while (openSet.Count > 0 && !pathFound && searching)
        {

            /////// IGNORE - UI STUFF /////////////
            if (pause)
            {
                yield return null;
            }
            else
            {
                if (!dynamicMode)
                {
                    yield return new WaitForSeconds(searchSpeed);
                }
                ///////////////////////////////////////


                //Set the first node from the list to current node
                currentNode = openSet[0];

                //we check the rest of the nodes 
                for (int i = 1; i < openSet.Count; i++)
                {
                    //if the node has less fCost than the current node, or the same but less Hcost
                    if (openSet[i].f < currentNode.f || ((openSet[i].f == currentNode.f) && openSet[i].h < currentNode.h)) 
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

                    //////////////  IGNORE - UI STUFF////////////////////////////
                    startStopSearchButton.gameObject.transform.Find("Text").GetComponent<Text>().text = "Start";
                    ///////////////////////////////////////////////////
                }
                else
                {
                    //For each neighbour of the current node
                    foreach (Node neighbour in GetNeighbours(currentNode))
                    {
                        //if we cannot traverse to the neighbour, or is in the close set
                        if(!neighbour.traversable || closedSet.Contains(neighbour))
                        {
                            //skip this loop
                            continue;
                        }

                        //Calculate the move to neightbor gCost using Hueristic (use getdistance)
                        float newMoveCost = GetDistance(currentNode, neighbour) + currentNode.g;

                        //if the new move costs less or this neighbour isnt in the open set
                        if(newMoveCost < neighbour.g || openSet.Contains(neighbour))
                        {
                            //store (new) move cost
                            //store heuristic to goalNode
                            //store parent

                            //if we dont have this neighbour in the open set
                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }

                        }

                    }
                }


                /////// IGNORE /////////////////
                if (step)
                {
                    step = false;
                    pause = true;
                    GameObject.Find("PauseBtn").gameObject.transform.Find("Text").GetComponent<Text>().text = "Play";
                }
                ///////////////////////////////
            }
        }
        searching = false;
    }

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

    public List<Node> GetNeighbours(Node node)
    {

        List<Node> neighbours = new List<Node>();
        if (allowDiagonal)
        {


            /* 
             * 
             * 
           Generating all the 8 neighbours of this cell for diagonals

               N.W   N   N.E 
                 \   |   / 
                  \  |  / 
              W ---- C ---- E 
                  /  |  \ 
                 /   |   \ 
               S.W   S    S.E 

           C --> Current Cell (i, j) 
           N -->  North       (i-1, j) 
           S -->  South       (i+1, j) 
           E -->  East        (i, j+1) 
           W -->  West           (i, j-1) 
           N.E--> North-East  (i-1, j+1) 
           N.W--> North-West  (i-1, j-1) 
           S.E--> South-East  (i+1, j+1) 
           S.W--> South-West  (i+1, j-1)

            */

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if(i == 0 && j == 0)
                    {
                        continue;
                    }

                    int pX = node.x + i;
                    int pY = node.y + j;

                    if(pX >= 0 && pX < gridNodes.x && pY >= 0 && pY < gridNodes.y)
                    {
                        neighbours.Add(grid[pX, pY]);
                    }
                }
            }
        }
        else{



            /* 
           Generating all the 8 neighbours of this cell for no diagonals

                     N   
                     |   
                     |  
               W---- C ----E 
                     |   
                     |    
                     S  

           C --> Current Cell (i, j) 
           N -->  North       (i-1, j) 
           S -->  South       (i+1, j) 
           E -->  East        (i, j+1) 
           W -->  West           (i, j-1) 

           
             */


            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((i == 0 && j == 0) || (i == -1 && j == -1) || (i == 1 && j == 1) || (i == -1 && j == 1) || (i == 1 && j == -1))
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
        }

        return neighbours;

    }

    public float GetDistance(Node nodeA, Node nodeB)
    {
        float rValue = 0;

        switch (hMode)
        {
            case HeuristicMode.Euclidean:
                rValue = Heuristic.GetDistanceEuclidean(nodeA, nodeB);
                break;

            case HeuristicMode.EuclideanNoSqr:
                rValue = Heuristic.GetDistanceEuclideanNoSqr(nodeA, nodeB);
                break;

            case HeuristicMode.Manhattan:
                rValue = Heuristic.GetDistanceManhattan(nodeA, nodeB);
                break;

            case HeuristicMode.Diagonal:
                rValue = Heuristic.GetDistanceDiag(nodeA, nodeB);
                break;

            case HeuristicMode.DiagonalShort:
                rValue = Heuristic.GetDistanceDiagShort(nodeA, nodeB);
                break;

        }

        if(rValue == -1)
        {
            searching = false;
        }

        return rValue;
    }
  
    public void AllowDiagonal(Toggle tgle)
    {
        allowDiagonal = tgle.isOn;

        searching = false;
        //UI set button text
        startStopSearchButton.gameObject.transform.Find("Text").GetComponent<Text>().text = "Start";
    }

    public void DynamicMode(Toggle tgle)
    {
        if (dynamicMode)
        {
            dynamicMode = false;
            tgle.isOn = false;
            GameObject.Find("Canvas").transform.Find("SlowSearch").gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("SlowSearch").SetActive(false);
            pause = false;
            step = false;
            searching = false;
            startStopSearchButton.gameObject.transform.Find("Text").GetComponent<Text>().text = "Start";
            dynamicMode = true;
            tgle.isOn = true;
        }

        searching = false;
        //UI set button text
        startStopSearchButton.gameObject.transform.Find("Text").GetComponent<Text>().text = "Start";
    }

    public void PauseBtn(Button pBtn)
    {
        if (pause && searching)
        {
            pause = false;
            pBtn.gameObject.transform.Find("Text").GetComponent<Text>().text = "Pause";
        }
        else if (!pause && searching)
        {
            pause = true;
            pBtn.gameObject.transform.Find("Text").GetComponent<Text>().text = "Play";
        }
    }

    public void StepBtn()
    {
        step = true;
        pause = false;
    }

    public void ObjectsVisible(Toggle tgle)
    {
        objVisible = tgle.isOn;

        if (objVisible)
        {
            foreach (GameObject item in gObjects)
            {
                item.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        else
        {
            foreach (GameObject item in gObjects)
            {

                item.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    public void ShowCostValue(Toggle tgle)
    {
        showCostValue = tgle.isOn;
    }

    public void SearchSpeed(Slider sldr)
    {
        searchSpeed = sldr.value;
    }

    public void HeuristicModeDDL(Dropdown ddl)
    {
        if((!searching && !dynamicMode) || dynamicMode)
        {
            hMode = (HeuristicMode)ddl.value;
            if(ddl.value == 2)
            {
                allowDiagonal = false;
            }
            else
            {
                allowDiagonal = true;
            }
        }
        else
        {
            ddl.value = (int)hMode;
        }
    }

    public void NodeSize(Slider sldr)
    {
        if (searching)
        {
            sldr.value = nodeSize;
            sldr.gameObject.transform.Find("NodeSize").GetComponent<Text>().text = "Node Size = " + nodeSize.ToString("0.00");
        }
        else
        {
            nodeSize = sldr.value;
            gridNodes = new Vector2(Mathf.RoundToInt(gridSize.x / nodeSize), Mathf.RoundToInt(gridSize.y / nodeSize));
            rootNodeGrid.transform.localScale = new Vector3(nodeSize, nodeSize, nodeSize);
            goalNodeGrid.transform.localScale = rootNodeGrid.transform.localScale;
            sldr.gameObject.transform.Find("NodeSize").GetComponent<Text>().text = "Node Size = " + nodeSize.ToString("0.00");

            CreateGrid();
        }

        if (sldr.value >= 0.6f && showCostValue == false)
        {
            showCostValue = true;
        }
        else if(sldr.value < 0.59f)
        {
            showCostValue = false;
        }
    }

    private void OnDrawGizmos()
    {

        if (grid != null)
        {
            Node rNode = NodePositionInGrid(rootNodeGrid.transform.position);
            Node gNode = NodePositionInGrid(goalNodeGrid.transform.position);

            foreach (Node node in grid)
            {

                if (node.traversable)
                {
                    Gizmos.color = new Color(0.75f, 0.75f, 0.75f, 0.45f);
                }
                else
                {
                    Gizmos.color = Color.red;
                }



                if (openSet != null)
                {
                    foreach (var item in openSet)
                    {
                        if (item == node)
                        {
                            Gizmos.color = new Color(1, 0.5f, 0.5f, 0.5f);

                        }
                    }
                }

                minCost = 0;
                maxCost = 0;

                if (closedSet != null)
                {
                    foreach (var item in closedSet)
                    {
                        if(minCost == 0 || item.f < minCost)
                        {
                            minCost = item.f;
                        }

                        if(maxCost == 0 || item.f > maxCost)
                        {
                            maxCost = item.f;
                        }
                    }

                    foreach (var item in closedSet)
                    {
                        if (item == node)
                        {
                            Gizmos.color = Color.Lerp(new Color(0, 1, 0, 0.5f), new Color(1, 0.3f, 0.1f, 0.7f), Mathf.InverseLerp(minCost, maxCost, node.f));

                        }
                    }
                }

                if (currentNode == node)
                {
                    Gizmos.color = new Color(0, 0.8f, 0.5f, 0.4f);
                }


                if (path != null && pathFound)
                {
                    foreach (var item in path)
                    {
                        if (item == node)
                        {
                            Gizmos.color = new Color(0, 0, 0.5f, 0.5f);

                        }
                    }
                }


                if (rNode == node)
                {
                    Gizmos.color = new Color(0, 0, 1, 0.4f);
                }

                if (gNode == node)
                {
                    Gizmos.color = new Color(0.2f, 0.5f, 0.2f, 0.5f);
                }

                if (showCostValue)
                {
                    if (node.traversable)
                    {
                        GUIStyle style = new GUIStyle();
                        style.normal.textColor = Color.black;
                        Handles.Label(new Vector3(node.nodePos.x - (nodeSize / 2.5f), node.nodePos.y + (nodeSize / 2.25f), -0.41f), "G " + node.g.ToString("0.00"), style);
                        Handles.Label(new Vector3(node.nodePos.x - (nodeSize / 2.5f), node.nodePos.y + (nodeSize / 4f), -0.41f), "H " + node.h.ToString("0.00"), style);
                        Handles.Label(new Vector3(node.nodePos.x - (nodeSize / 2.5f), node.nodePos.y , -0.41f), "F "+ node.f.ToString("0.00"), style);

                    }
                    else
                    {
                        Handles.Label(new Vector3(node.nodePos.x - (nodeSize / 2.5f), node.nodePos.y + (nodeSize / 3f), -0.41f), "");
                    }
                }



                Gizmos.DrawCube(node.nodePos, new Vector3(nodeSize *0.9f, nodeSize *0.9f, 0.1f));

            }
        }
    }

}
