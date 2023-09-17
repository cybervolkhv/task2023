using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Vector2Int Position;
    public Vector2Int TargetPosition;
    public Node PreviosNode;
    public int F;
    public int G;
    public int H;

    public Node(int g, Vector2Int nodePosition, Vector2Int targetPosition, Node previosNod)
    {
        Position = nodePosition;
        TargetPosition = targetPosition;
        PreviosNode = previosNod;
        G = g;
        H = Math.Abs(targetPosition.x - Position.x) + Math.Abs(targetPosition.y - Position.y);
        F = G + H;
    }
}
