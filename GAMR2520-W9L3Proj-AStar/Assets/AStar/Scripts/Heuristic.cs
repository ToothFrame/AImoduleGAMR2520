using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Heuristic
{
    public static float GetDistanceEuclidean(Node nodeA, Node nodeB)
    {
        float deltaX = nodeB.x - nodeA.x;
        float deltaY = nodeB.y - nodeA.y;

        return Mathf.Sqrt(Mathf.Pow(deltaX, 2) + Mathf.Pow(deltaY, 2));
    }

    public static float GetDistanceEuclideanNoSqr(Node nodeA, Node nodeB)
    {
        return -1;
    }

    public static float GetDistanceManhattan(Node nodeA, Node nodeB)
    {
        return -1;
    }

    public static float GetDistanceDiag(Node nodeA, Node nodeB)
    {
        return -1;
    }
    public static float GetDistanceDiagShort(Node nodeA, Node nodeB)
    {
        return -1;
    }
}
