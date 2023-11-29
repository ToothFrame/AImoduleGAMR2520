using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{


    public PMLevelScript pmLevelScript;
    bool[,] adjmat;// Adjacency matrix 

    public Vector2 currentPos = new Vector2(15, 24);
    int width;
    // Start is called before the first frame update
    void Start()
    {
        adjmat = pmLevelScript.GetAdjMat;
        width = pmLevelScript.GetWidth;
        transform.position = pmLevelScript.GetNodePos[(int)currentPos.x, (int)currentPos.y].transform.position;
    }


}
