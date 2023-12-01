using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Heuristic
{
    public static float GetDistanceEuclidean(Node nodeA, Node nodeB) //slowest (more accurate)
    {
        float deltaX = nodeB.x - nodeA.x;
        float deltaY = nodeB.y - nodeA.y;

        return Mathf.Sqrt(Mathf.Pow(deltaX, 2) + Mathf.Pow(deltaY, 2));
    }

    public static float GetDistanceEuclideanNoSqr(Node nodeA, Node nodeB) //fastest (less accurate)
    {
        float deltaX = nodeB.x - nodeA.x;
        float deltaY = nodeB.y - nodeA.y;
        return Mathf.Pow(deltaX, 2) + Mathf.Pow(deltaY, 2);
    }

    public static float GetDistanceManhattan(Node nodeA, Node nodeB) // slow,  straighter paths
    {
        float deltaX = nodeB.x - nodeA.x;
        float deltaY = nodeB.y - nodeA.y;

        return Mathf.Abs(deltaX) + Mathf.Abs(deltaY);
    }

    public static float GetDistanceDiag(Node nodeA, Node nodeB) //slow (diagonal paths)
    {
        float deltaX = nodeB.x - nodeA.x;
        float deltaY = nodeB.y - nodeA.y;
        return Mathf.Max(Mathf.Abs(deltaX) , Mathf.Abs(deltaY));
    }
    public static float GetDistanceDiagShort(Node nodeA, Node nodeB) //decent balance quite accurate
    {
        float deltaX = Mathf.Abs(nodeB.x - nodeA.x);
        float deltaY = Mathf.Abs(nodeB.y - nodeA.y);

        if(deltaX > deltaY)
        {
            return (deltaY * 1.41f) + deltaX - deltaY;
        }
        else
        {
            return (deltaX * 1.41f) + deltaY - deltaX;
        }
    }
}
