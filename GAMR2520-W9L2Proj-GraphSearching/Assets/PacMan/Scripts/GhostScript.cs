using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{


    public PMLevelScript pmLevelScript;
    bool[,] adjmat;// Adjacency matrix 

    public Vector2 currentPos = new Vector2(15, 24);
    int width;
    int height;

    List<int> path = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        adjmat = pmLevelScript.GetAdjMat;
        width = pmLevelScript.GetWidth;
        height = pmLevelScript.GetHeight;
        transform.position = pmLevelScript.GetNodePos[(int)currentPos.x, (int)currentPos.y].transform.position;

        StartCoroutine(FindRandomPos());

    }

    IEnumerator FindRandomPos()
    {
        int randomX = Random.Range(0, width - 1);
        int randomY = Random.Range(0, height - 1);
        bool traverable = pmLevelScript.Traversable(pmLevelScript.GetMapChar(randomX, randomY));

        while (!traverable)
        {
            randomX = Random.Range(0, width - 1);
            randomY = Random.Range(0, height - 1);
            traverable = pmLevelScript.Traversable(pmLevelScript.GetMapChar(randomX, randomY));
            yield return new WaitForEndOfFrame();
        }

        int currentNode = (int)currentPos.y * width + (int)currentPos.x;
        int randomNode = randomY * width + randomX;

        path.Clear();
        path = pmLevelScript.BFSPath(currentNode, randomNode);

        if(path.Count > 2)
        {
            StartCoroutine(TraversePath(randomX, randomY));
        }
        else
        {
            StartCoroutine(FindRandomPos());
        }

    }

    IEnumerator TraversePath(int randomX, int randomY)
    {

        for (int i = path.Count-1; i >= 0; i--)
        {
            transform.position = pmLevelScript.GetNodeGameObject(path[i]).transform.position;
            yield return new WaitForSeconds(0.2f);
        }

        currentPos = new Vector2(randomX, randomY);
        StartCoroutine(FindRandomPos());

    }




}
