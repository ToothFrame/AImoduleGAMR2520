﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 nodePos;
    public int x;
    public int y;
    public bool traversable = false;

    public float g;
    public float h;

    public Node parentNode;

    public Node(Vector3 nodePos, bool traversable, int x, int y)
    {
        this.nodePos = nodePos;
        this.traversable = traversable;
        this.x = x;
        this.y = y;
    }

    public float f
    {
        get
        {
            return g + h;
        }
 
    }
}
