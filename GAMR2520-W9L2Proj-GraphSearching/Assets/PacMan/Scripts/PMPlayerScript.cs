using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMPlayerScript : MonoBehaviour
{

    public PMLevelScript pmLevelScript;
    bool[,] adjmat;// Adjacency matrix 

    Vector2 currentPos = new Vector2(15, 24);
    int width;
    int height;


    bool goLeft = false;
    bool goRight = false;
    bool goUp = false;
    bool goDown = false;

    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        adjmat = pmLevelScript.GetAdjMat;
        width = pmLevelScript.GetWidth;
        height = pmLevelScript.GetHeight;
        transform.position = pmLevelScript.GetNodePos[(int)currentPos.x, (int)currentPos.y].transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (time >= 0.2f || Input.anyKeyDown)
        {

            if(Input.anyKeyDown)
            {
                goLeft = false;
                goRight = false;
                goUp = false;
                goDown = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || goLeft)
            {
                if (currentPos.x > 0)
                {
                    GoToNode((int)currentPos.x - 1, (int)currentPos.y);
                }
                else
                {
                    GoToNode(width - 1, (int)currentPos.y);
                }
                print("Left");
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));

                goLeft = true;
                goRight = false;
                goUp = false;
                goDown = false;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || goRight)
            {
                if (currentPos.x < width - 1)
                {
                    GoToNode((int)currentPos.x + 1, (int)currentPos.y);
                }
                else
                {
                    GoToNode(0, (int)currentPos.y);
                }
                print("Right");
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));


                goLeft = false;
                goRight = true;
                goUp = false;
                goDown = false;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || goDown)
            {
                GoToNode((int)currentPos.x, (int)currentPos.y + 1);
                print("Down");
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));


                goLeft = false;
                goRight = false;
                goUp = false;
                goDown = true;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || goUp)
            {
                GoToNode((int)currentPos.x, (int)currentPos.y - 1);
                print("Up");
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));


                goLeft = false;
                goRight = false;
                goUp = true;
                goDown = false;
            }

            time = 0;
        }

        time += Time.deltaTime;
    }

    void GoToNode(int x, int y)
    {
        int currentNode = (int)currentPos.y * width + (int)currentPos.x;
        int nodeToGo = y * width + x;
        if (adjmat[currentNode, nodeToGo])
        {
            transform.position = pmLevelScript.GetNodePos[x, y].transform.position;
            currentPos = new Vector2(x, y);
            pmLevelScript.PMTouchingNode((int)currentPos.y, (int)currentPos.x);
        }
    }

}
