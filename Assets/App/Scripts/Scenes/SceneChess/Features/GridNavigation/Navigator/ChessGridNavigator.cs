using System;
using System.Linq;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            List<Vector2Int> PathToTarget = new List<Vector2Int>();
            List<Node> CheckedNodes = new List<Node>();
            List<Node> WaitingNodes = new List<Node>();

            if (from == to) { return PathToTarget; }

            Node startNode = new Node(0, from, to, null);
            CheckedNodes.Add(startNode);
            switch (unit)
            {
                case ChessUnitType.King:
                    WaitingNodes.AddRange(GetNeighborsForKing(startNode));
                    break;
                case ChessUnitType.Knight:
                    WaitingNodes.AddRange(GetNeighborsForKnight(startNode));
                    break;
                case ChessUnitType.Pon:
                    WaitingNodes.AddRange(GetNeighborsForPon(startNode));
                    break;
                case ChessUnitType.Queen:
                case ChessUnitType.Bishop:
                case ChessUnitType.Rook:
                    WaitingNodes.AddRange(GetNeighborsForKing(startNode));
                    break;
            }

            while (WaitingNodes.Count > 0)
            {
                Node nodeToCheck = WaitingNodes.Where(x => x.F == WaitingNodes.Min(y => y.F)).FirstOrDefault();
                if (nodeToCheck.Position == to)
                {
                    return CalculatePathFromNode(nodeToCheck);
                }

                bool walkable = true;

                if (!walkable)
                {
                    WaitingNodes.Remove(nodeToCheck);
                    CheckedNodes.Add(nodeToCheck);
                }
                else if (walkable)
                {
                    WaitingNodes.Remove(nodeToCheck);
                    if (!CheckedNodes.Where(x => x.Position == nodeToCheck.Position).Any())
                    {
                        CheckedNodes.Add(nodeToCheck);
                        switch (unit)
                        {
                            case ChessUnitType.King:
                                WaitingNodes.AddRange(GetNeighborsForKing(nodeToCheck));
                                break;
                            case ChessUnitType.Knight:
                                WaitingNodes.AddRange(GetNeighborsForKnight(nodeToCheck));
                                break;
                            case ChessUnitType.Pon:
                                WaitingNodes.AddRange(GetNeighborsForPon(nodeToCheck));
                                break;
                            case ChessUnitType.Queen:
                            case ChessUnitType.Bishop:
                            case ChessUnitType.Rook:
                                WaitingNodes.AddRange(GetNeighborsForKing(nodeToCheck));
                                break;
                        }

                       
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
            //напиши реализацию не меняя сигнатуру функции
            //throw new NotImplementedException();
        }

        public List<Vector2Int> CalculatePathFromNode(Node node)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Node currentNode = node;

            while (currentNode.PreviosNode != null)
            {
                path.Add(new Vector2Int(currentNode.Position.x, currentNode.Position.y));
                currentNode = currentNode.PreviosNode;
            }
            path.Reverse();
            return path;
        }
 
        public List<Node> GetNeighborsForKing(Node node)
        {
            int verticalAndHorizontalMoveCostG = 10;
            int diagonalMoveCostG = 14;
            List<Node> neighbors = new List<Node>();
            neighbors.Add(
               new Node(node.G + verticalAndHorizontalMoveCostG,
               new Vector2Int(node.Position.x - 1, node.Position.y),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + verticalAndHorizontalMoveCostG,
               new Vector2Int(node.Position.x + 1, node.Position.y),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + verticalAndHorizontalMoveCostG,
               new Vector2Int(node.Position.x, node.Position.y - 1),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + verticalAndHorizontalMoveCostG,
               new Vector2Int(node.Position.x, node.Position.y + 1),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + diagonalMoveCostG,
               new Vector2Int(node.Position.x + 1, node.Position.y + 1),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + diagonalMoveCostG,
               new Vector2Int(node.Position.x - 1, node.Position.y - 1),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + diagonalMoveCostG,
               new Vector2Int(node.Position.x + 1, node.Position.y - 1),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + diagonalMoveCostG,
               new Vector2Int(node.Position.x - 1, node.Position.y + 1),
               node.TargetPosition,
               node));

            return neighbors;
        }

        public List<Node> GetNeighborsForPon(Node node)
        {
            int verticalMoveCostG = 10;
            List<Node> neighbors = new List<Node>();          
            neighbors.Add(
               new Node(node.G + verticalMoveCostG,
               new Vector2Int(node.Position.x, node.Position.y - 1),
               node.TargetPosition,
               node));       

            return neighbors;
        }

        public List<Node> GetNeighborsForKnight(Node node)
        {
            int knightMoveCostG = 32;
            List<Node> neighbors = new List<Node>();
            neighbors.Add(
               new Node(node.G + knightMoveCostG,
               new Vector2Int(node.Position.x + 1, node.Position.y + 2),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + knightMoveCostG,
               new Vector2Int(node.Position.x + 2, node.Position.y + 1),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + knightMoveCostG,
               new Vector2Int(node.Position.x + 2, node.Position.y - 1),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + knightMoveCostG,
               new Vector2Int(node.Position.x + 1, node.Position.y - 2),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + knightMoveCostG,
               new Vector2Int(node.Position.x - 1, node.Position.y - 2),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + knightMoveCostG,
               new Vector2Int(node.Position.x - 2, node.Position.y - 1),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + knightMoveCostG,
               new Vector2Int(node.Position.x - 2, node.Position.y + 1),
               node.TargetPosition,
               node));
            neighbors.Add(
               new Node(node.G + knightMoveCostG,
               new Vector2Int(node.Position.x - 1, node.Position.y + 2),
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