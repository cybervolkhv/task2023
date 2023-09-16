using System;
using System.Linq;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Piece;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            List<Vector2Int> PathToTarget = new List<Vector2Int>();
            List<Node> CheckedNodes = new List<Node>();
            List<Node> WaitingNodes = new List<Node>();
            int startG = 0;
            if (from == to) { return PathToTarget; }

            Node startNode = new Node(startG, from, to, null);
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
                    WaitingNodes.AddRange(GetNeighborsForQueen(startNode));
                    break;
                case ChessUnitType.Bishop:
                    WaitingNodes.AddRange(GetNeighborsForBishop(startNode));
                    break;
                case ChessUnitType.Rook:
                    WaitingNodes.AddRange(GetNeighborsForRook(startNode));
                    break;
            }

            while (WaitingNodes.Count > 0)
            {
                //Node nodeToCheck = WaitingNodes.Where(x => x.F == WaitingNodes.Min(y => y.F)).FirstOrDefault();
                int minValueF = int.MaxValue;
                Node nodeToCheck = null;
                foreach (Node node in WaitingNodes)
                {
                    if (node.F < minValueF)
                    {
                        minValueF = node.F;
                        nodeToCheck = node;
                    }
                }

                if (nodeToCheck.Position == to)
                {
                    return CalculatePathFromNode(nodeToCheck);
                }
                Debug.Log(nodeToCheck.Position.x + " " + nodeToCheck.Position.y);

                // ChessUnit chessUnit = grid.Get(nodeToCheck.Position);
                //bool walkable = chessUnit.IsAvailable;
                //grid.RemoveAt(nodeToCheck.Position);
                bool walkable = true;

                if (!walkable)
                {
                    WaitingNodes.Remove(nodeToCheck);
                    CheckedNodes.Add(nodeToCheck);
                }
                else if (walkable)
                {
                    WaitingNodes.Remove(nodeToCheck);

                    bool isContains = false;
                    foreach (Node node in CheckedNodes)
                    {
                        if (node.Position == nodeToCheck.Position)
                        {
                            isContains = true;
                        }
                    }

                    if (!isContains)        //((!CheckedNodes.Where(x => x.Position == nodeToCheck.Position).Any())) // если нету то добавь(!CheckedNodes.Contains(nodeToCheck)) 
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
                                WaitingNodes.AddRange(GetNeighborsForQueen(nodeToCheck));
                                break;
                            case ChessUnitType.Bishop:
                                WaitingNodes.AddRange(GetNeighborsForBishop(nodeToCheck));
                                break;
                            case ChessUnitType.Rook:
                                WaitingNodes.AddRange(GetNeighborsForRook(nodeToCheck));
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
            if (!(node.Position.x - 1 < 0))
            {
                neighbors.Add(
                   new Node(node.G + verticalAndHorizontalMoveCostG,
                   new Vector2Int(node.Position.x - 1, node.Position.y),
                   node.TargetPosition,
                   node));     
            }
            if (!(node.Position.x + 1 > 7))
            {
                neighbors.Add(
                   new Node(node.G + verticalAndHorizontalMoveCostG,
                   new Vector2Int(node.Position.x + 1, node.Position.y),
                   node.TargetPosition,
                   node));
            }
            if (!(node.Position.y - 1 < 0))
            {
                neighbors.Add(
                   new Node(node.G + verticalAndHorizontalMoveCostG,
                   new Vector2Int(node.Position.x, node.Position.y - 1),
                   node.TargetPosition,
                   node));
            }
            if (!(node.Position.y + 1 > 7))
            {
                neighbors.Add(
                   new Node(node.G + verticalAndHorizontalMoveCostG,
                   new Vector2Int(node.Position.x, node.Position.y + 1),
                   node.TargetPosition,
                   node));
            }
            if (!((node.Position.x + 1 > 7) || (node.Position.y + 1 > 7)))
            {
                neighbors.Add(
                   new Node(node.G + diagonalMoveCostG,
                   new Vector2Int(node.Position.x + 1, node.Position.y + 1),
                   node.TargetPosition,
                   node));
            }
            if (!((node.Position.x - 1 < 0) || (node.Position.y - 1 < 0)))
            {
                neighbors.Add(
                   new Node(node.G + diagonalMoveCostG,
                   new Vector2Int(node.Position.x - 1, node.Position.y - 1),
                   node.TargetPosition,
                   node));
            }
            if (!((node.Position.x + 1 > 7) || (node.Position.y - 1 < 0)))
            {
                neighbors.Add(
                   new Node(node.G + diagonalMoveCostG,
                   new Vector2Int(node.Position.x + 1, node.Position.y - 1),
                   node.TargetPosition,
                   node));
            }
            if (!((node.Position.x - 1 < 0) || (node.Position.y + 1 > 7)))
            {
                neighbors.Add(
                   new Node(node.G + diagonalMoveCostG,
                   new Vector2Int(node.Position.x - 1, node.Position.y + 1),
                   node.TargetPosition,
                   node));
            }   
            return neighbors;
        }

        public List<Node> GetNeighborsForPon(Node node)
        {
            int verticalMoveCostG = 10;
            List<Node> neighbors = new List<Node>();
            if (!(node.Position.y - 1 < 0))
            {
                neighbors.Add(
                   new Node(node.G + verticalMoveCostG,
                   new Vector2Int(node.Position.x, node.Position.y - 1),
                   node.TargetPosition,
                   node));
            }
            return neighbors;
        }

        public List<Node> GetNeighborsForKnight(Node node)
        {
            int knightMoveCostG = 32;
            List<Node> neighbors = new List<Node>();
            if (!((node.Position.x + 1 > 7) || (node.Position.y + 2 > 7)))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x + 1, node.Position.y + 2),
                   node.TargetPosition,
                   node));
            }

            if (!((node.Position.x + 2 > 7) || (node.Position.y + 1 > 7)))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x + 2, node.Position.y + 1),
                   node.TargetPosition,
                   node));
            }
            if (!((node.Position.x + 2 > 7) || (node.Position.y - 1 < 0)))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x + 2, node.Position.y - 1),
                   node.TargetPosition,
                   node));
            }
            if (!((node.Position.x + 1 > 7) || (node.Position.y - 2 < 0)))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x + 1, node.Position.y - 2),
                   node.TargetPosition,
                   node));
            }
            if (!((node.Position.x - 1 < 0) || (node.Position.y - 2 < 0)))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x - 1, node.Position.y - 2),
                   node.TargetPosition,
                   node));
            }

            if (!((node.Position.x - 2 < 0) || (node.Position.y - 1 < 0)))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x - 2, node.Position.y - 1),
                   node.TargetPosition,
                   node));
            }
            if (!((node.Position.x - 2 < 0) || (node.Position.y + 1 > 7)))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x - 2, node.Position.y + 1),
                   node.TargetPosition,
                   node));
            }
            if (!((node.Position.x - 1 < 0) || (node.Position.y + 2 > 7)))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x - 1, node.Position.y + 2),
                   node.TargetPosition,
                   node));
            }
            return neighbors;
        }

        public List<Node> GetNeighborsForRook(Node node)
        {
            int verticalAndHorizontalMoveCostG = 10;
            List<Node> neighbors = new List<Node>();
            int chessBoardSize = 8;
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (!(node.Position.x + i > 7))
                {
                    neighbors.Add(
                       new Node(node.G + verticalAndHorizontalMoveCostG,
                       new Vector2Int(node.Position.x + i, node.Position.y),
                       node.TargetPosition,
                       node));
                }                  
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (!(node.Position.x - i < 0))
                {
                    neighbors.Add(
                       new Node(node.G + verticalAndHorizontalMoveCostG,
                       new Vector2Int(node.Position.x - i, node.Position.y),
                       node.TargetPosition,
                       node));
                }                
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (!(node.Position.y + i > 7))
                {
                    neighbors.Add(
                       new Node(node.G + verticalAndHorizontalMoveCostG,
                       new Vector2Int(node.Position.x, node.Position.y + i),
                       node.TargetPosition,
                       node));
                }               
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (!(node.Position.y - i < 0))
                {
                    neighbors.Add(
                       new Node(node.G + verticalAndHorizontalMoveCostG,
                       new Vector2Int(node.Position.x, node.Position.y - i),
                       node.TargetPosition,
                       node));
                }              
            }
            return neighbors;
        }

        public List<Node> GetNeighborsForBishop(Node node)
        {
            int diagonalMoveCostG = 14;
            int chessBoardSize = 8;
            List<Node> neighbors = new List<Node>();

            for (int i = 1; i < chessBoardSize; i++)
            {
                if (!((node.Position.x + i > 7) || (node.Position.y + i > 7)))
                {
                    neighbors.Add(
                 new Node(node.G + diagonalMoveCostG,
                 new Vector2Int(node.Position.x + i, node.Position.y + i),
                 node.TargetPosition,
                 node));
                }
                  
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (!((node.Position.x - i < 0) || (node.Position.y - i < 0)))
                {
                    neighbors.Add(
                 new Node(node.G + diagonalMoveCostG,
                 new Vector2Int(node.Position.x - i, node.Position.y - i),
                 node.TargetPosition,
                 node));
                }              
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (!((node.Position.x - i < 0) || (node.Position.y + i > 7)))
                {
                    neighbors.Add(
                       new Node(node.G + diagonalMoveCostG,
                       new Vector2Int(node.Position.x - i, node.Position.y + i),
                       node.TargetPosition,
                       node));
                }

            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (!((node.Position.x + i > 7) || (node.Position.y - i < 0)))
                {
                    neighbors.Add(
                 new Node(node.G + diagonalMoveCostG,
                 new Vector2Int(node.Position.x + i, node.Position.y - i),
                 node.TargetPosition,
                 node));
                }           
            }
            return neighbors;
        }

        public List<Node> GetNeighborsForQueen(Node node)
        {
            int verticalAndHorizontalMoveCostG = 10;
            int diagonalMoveCostG = 14;
            int chessBoardSize = 8;
            List<Node> neighbors = new List<Node>();

            for (int i = 1; i < chessBoardSize; i++)
            {
                if (!(node.Position.x + i > 7))
                {
                    neighbors.Add(
              new Node(node.G + verticalAndHorizontalMoveCostG,
              new Vector2Int(node.Position.x + i, node.Position.y),
              node.TargetPosition,
              node));
                }
               
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (!(node.Position.x - i < 0))
                {
                    neighbors.Add(
                 new Node(node.G + verticalAndHorizontalMoveCostG,
                 new Vector2Int(node.Position.x - i, node.Position.y),
                 node.TargetPosition,
                 node));
                }
            }
                  
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (!(node.Position.y + i > 7))
                {
                    neighbors.Add(
                new Node(node.G + verticalAndHorizontalMoveCostG,
                new Vector2Int(node.Position.x, node.Position.y + i),
                node.TargetPosition,
                node));
                }
            }
                 
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (!(node.Position.y - i < 0))
                {
                    neighbors.Add(
                 new Node(node.G + verticalAndHorizontalMoveCostG,
                 new Vector2Int(node.Position.x, node.Position.y - i),
                 node.TargetPosition,
                 node));
                }
                  
            }
            for (int i = 1; i < chessBoardSize; i++)
            {

                if (!((node.Position.x + i > 7) || (node.Position.y + i > 7)))
                {
                    neighbors.Add(
                   new Node(node.G + diagonalMoveCostG,
                   new Vector2Int(node.Position.x + i, node.Position.y + i),
                   node.TargetPosition,
                   node));
                }
                  
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (!((node.Position.x - i < 0) || (node.Position.y - i < 0)))
                {
                    neighbors.Add(
                   new Node(node.G + diagonalMoveCostG,
                   new Vector2Int(node.Position.x - i, node.Position.y - i),
                   node.TargetPosition,
                   node));
                }
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (!((node.Position.x - i < 0) || (node.Position.y + i > 7)))
                {
                    neighbors.Add(
                   new Node(node.G + diagonalMoveCostG,
                   new Vector2Int(node.Position.x - i, node.Position.y + i),
                   node.TargetPosition,
                   node));
                }
                  
            }
            for (int i = 1; i < chessBoardSize; i++)
            {

                if (!((node.Position.x + i > 7) || (node.Position.y - i < 0)))
                {
                    neighbors.Add(
                   new Node(node.G + diagonalMoveCostG,
                   new Vector2Int(node.Position.x + i, node.Position.y - i),
                   node.TargetPosition,
                   node));
                }
                  
            }
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