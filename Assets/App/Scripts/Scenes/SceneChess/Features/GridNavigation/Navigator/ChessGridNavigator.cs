using System;
using System.Linq;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            List<Vector2Int> PathToTarget = new List<Vector2Int>();
            List<Node> CheckedNodes = new List<Node>();
            List<Node> WaitingNodes = new List<Node>();
            GameObject Target;


            if (from == to) { return PathToTarget; }

            Node startNode = new Node(0, from, to, null);
            CheckedNodes.Add(startNode);
            WaitingNodes.AddRange(GetNeighbors(startNode));

            while(WaitingNodes.Count > 0)
            {
                Node nodeToCheck = WaitingNodes.Where(x => x.F == WaitingNodes.Min(y => y.F)).FirstOrDefault();
                if (nodeToCheck.Position == to) 
                {
                    return CalculatePathFromNode(nodeToCheck);
                }

                bool walkable = true;

                if(!walkable)
                {
                    WaitingNodes.Remove(nodeToCheck);
                    CheckedNodes.Add(nodeToCheck);
                } 
                else if(walkable)
                {
                    WaitingNodes.Remove(nodeToCheck);
                    if (!CheckedNodes.Where(x => x.Position == nodeToCheck.Position).Any())
                    {
                        CheckedNodes.Add(nodeToCheck);
                        WaitingNodes.AddRange(GetNeighbors(nodeToCheck));
                    }
                    //else
                    //{
                    //    var sameNode = CheckedNodes.Where(x => x.Position == nodeToCheck.Position).ToList();
                    //    for (int i = 0; i < sameNode.Count; i++)
                    //    {
                    //        if (sameNode[i].F > nodeToCheck.F)
                    //    }
                    //}
                }

            }

            return PathToTarget;
            // return PathToTarget;
            //напиши реализацию не меняя сигнатуру функции
            //throw new NotImplementedException();
        }

        public List<Vector2Int> CalculatePathFromNode(Node node)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Node currentNode = node;

            while(currentNode.PreviosNode != null)
            {
                path.Add(new Vector2Int(currentNode.Position.x, currentNode.Position.y));
                currentNode = currentNode.PreviosNode;
            }

            return path;
        }

        public List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();
            neighbors.Add(
                new Node(node.G + 1,
                new Vector2Int(node.Position.x - 1, node.Position.y),
                node.TargetPosition,
                node));
            neighbors.Add(
               new Node(node.G + 1,
               new Vector2Int(node.Position.x + 1, node.Position.y),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + 1,
               new Vector2Int(node.Position.x, node.Position.y - 1),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + 1,
               new Vector2Int(node.Position.x, node.Position.y + 1),
               node.TargetPosition,
               node));

            return neighbors;

        }
    }

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

}