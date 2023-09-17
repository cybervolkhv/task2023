using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Piece;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using System.Drawing;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            List<Vector2Int> PathToTarget = new List<Vector2Int>();
            List<Node> checkedNodes = new List<Node>();
            List<Node> waitingNodes = new List<Node>();
            int startG = 0;

            if (from == to) { return PathToTarget; }

            Node startNode = new Node(startG, from, to, null);
            checkedNodes.Add(startNode);

            GetNeighbors(unit, startNode, grid, waitingNodes);

            while (waitingNodes.Count > 0)
            {
                Node nodeToCheck = SelectNodeWithMinFCost(waitingNodes);

                if (nodeToCheck.Position == to)
                {
                    return CalculatePathFromNode(nodeToCheck);
                }

                ChessUnit chessUnit = grid.Get(nodeToCheck.Position);
                bool isAvailable = chessUnit == null;

                if (!isAvailable)
                {
                    waitingNodes.Remove(nodeToCheck);
                    checkedNodes.Add(nodeToCheck);
                }
                else if (isAvailable)
                {
                    waitingNodes.Remove(nodeToCheck);

                    bool isContains = IsContains(nodeToCheck, checkedNodes);

                    if (!isContains)
                    {
                        checkedNodes.Add(nodeToCheck);
                        GetNeighbors(unit, nodeToCheck, grid, waitingNodes);
                    }
                }
            }
            return null;
        }
        public Node SelectNodeWithMinFCost(List<Node> waitingNodes)
        {
            int minValueF = int.MaxValue;
            Node nodeToCheck = null;
            foreach (Node node in waitingNodes)
            {
                if (node.F < minValueF)
                {
                    minValueF = node.F;
                    nodeToCheck = node;
                }
            }
            return nodeToCheck;
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

        public bool IsContains(Node nodeToCheck, List<Node> checkedNodes)
        {
            bool isContains = false;
            foreach (Node node in checkedNodes)
            {
                if (node.Position == nodeToCheck.Position)
                {
                    isContains = true;
                }
            }
            return isContains;
        }

        public void GetNeighbors(ChessUnitType unit, Node node, ChessGrid grid, List<Node> waitingNodes)
        {
            switch (unit)
            {
                case ChessUnitType.King:
                    waitingNodes.AddRange(GetNeighborsForKing(node));
                    break;
                case ChessUnitType.Knight:
                    waitingNodes.AddRange(GetNeighborsForKnight(node));
                    break;
                case ChessUnitType.Pon:
                    waitingNodes.AddRange(GetNeighborsForPon(node));
                    break;
                case ChessUnitType.Queen:
                    waitingNodes.AddRange(GetNeighborsForQueen(node, grid));
                    break;
                case ChessUnitType.Bishop:
                    waitingNodes.AddRange(GetNeighborsForBishop(node, grid));
                    break;
                case ChessUnitType.Rook:
                    waitingNodes.AddRange(GetNeighborsForRook(node, grid));
                    break;
            }
        }

        public List<Node> GetNeighborsForKing(Node node)
        {
            int verticalAndHorizontalMoveCostG = 10;
            int diagonalMoveCostG = 14;
            int boardSize = 8;
            List<Node> neighbors = new List<Node>();
            if (node.Position.x - 1 >= 0)
            {
                neighbors.Add(
                   new Node(node.G + verticalAndHorizontalMoveCostG,
                   new Vector2Int(node.Position.x - 1, node.Position.y),
                   node.TargetPosition,
                   node));
            }
            if (node.Position.x + 1 < boardSize)
            {
                neighbors.Add(
                   new Node(node.G + verticalAndHorizontalMoveCostG,
                   new Vector2Int(node.Position.x + 1, node.Position.y),
                   node.TargetPosition,
                   node));
            }
            if (node.Position.y - 1 >= 0)
            {
                neighbors.Add(
                   new Node(node.G + verticalAndHorizontalMoveCostG,
                   new Vector2Int(node.Position.x, node.Position.y - 1),
                   node.TargetPosition,
                   node));
            }
            if (node.Position.y + 1 < boardSize)
            {
                neighbors.Add(
                   new Node(node.G + verticalAndHorizontalMoveCostG,
                   new Vector2Int(node.Position.x, node.Position.y + 1),
                   node.TargetPosition,
                   node));
            }
            if ((node.Position.x + 1 < boardSize) && (node.Position.y + 1 < boardSize))
            {
                neighbors.Add(
                   new Node(node.G + diagonalMoveCostG,
                   new Vector2Int(node.Position.x + 1, node.Position.y + 1),
                   node.TargetPosition,
                   node));
            }
            if ((node.Position.x - 1 >= 0) && (node.Position.y - 1 >= 0))
            {
                neighbors.Add(
                   new Node(node.G + diagonalMoveCostG,
                   new Vector2Int(node.Position.x - 1, node.Position.y - 1),
                   node.TargetPosition,
                   node));
            }
            if ((node.Position.x + 1 < boardSize) && (node.Position.y - 1 >= 0))
            {
                neighbors.Add(
                   new Node(node.G + diagonalMoveCostG,
                   new Vector2Int(node.Position.x + 1, node.Position.y - 1),
                   node.TargetPosition,
                   node));
            }
            if ((node.Position.x - 1 >= 0) && (node.Position.y + 1 < boardSize))
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
            int boardSize = 8;
            List<Node> neighbors = new List<Node>();
            if (node.Position.y - 1 >= 0)
            {
                neighbors.Add(
                   new Node(node.G + verticalMoveCostG,
                   new Vector2Int(node.Position.x, node.Position.y - 1),
                   node.TargetPosition,
                   node));
            }
            if (node.Position.y + 1 < boardSize)
            {
                neighbors.Add(
                   new Node(node.G + verticalMoveCostG,
                   new Vector2Int(node.Position.x, node.Position.y + 1),
                   node.TargetPosition,
                   node));
            }
            return neighbors;
        }

        public List<Node> GetNeighborsForKnight(Node node)
        {
            int knightMoveCostG = 32;
            int boardSize = 8;
            List<Node> neighbors = new List<Node>();
            if ((node.Position.x + 1 < boardSize) && (node.Position.y + 2 < boardSize))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x + 1, node.Position.y + 2),
                   node.TargetPosition,
                   node));
            }
            if ((node.Position.x + 2 < boardSize) && (node.Position.y + 1 < boardSize))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x + 2, node.Position.y + 1),
                   node.TargetPosition,
                   node));
            }
            if ((node.Position.x + 2 < boardSize) && (node.Position.y - 1 >= 0))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x + 2, node.Position.y - 1),
                   node.TargetPosition,
                   node));
            }
            if ((node.Position.x + 1 < boardSize) && (node.Position.y - 2 >= 0))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x + 1, node.Position.y - 2),
                   node.TargetPosition,
                   node));
            }
            if ((node.Position.x - 1 >= 0) && (node.Position.y - 2 >= 0))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x - 1, node.Position.y - 2),
                   node.TargetPosition,
                   node));
            }
            if ((node.Position.x - 2 >= 0) && (node.Position.y - 1 >= 0))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x - 2, node.Position.y - 1),
                   node.TargetPosition,
                   node));
            }
            if ((node.Position.x - 2 >= 0) && (node.Position.y + 1 < boardSize))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x - 2, node.Position.y + 1),
                   node.TargetPosition,
                   node));
            }
            if ((node.Position.x - 1 >= 0) && (node.Position.y + 2 < boardSize))
            {
                neighbors.Add(
                   new Node(node.G + knightMoveCostG,
                   new Vector2Int(node.Position.x - 1, node.Position.y + 2),
                   node.TargetPosition,
                   node));
            }
            return neighbors;
        }

        public List<Node> GetNeighborsForRook(Node node, ChessGrid grid)
        {
            int verticalAndHorizontalMoveCostG = 10;
            List<Node> neighbors = new List<Node>();
            int chessBoardSize = 8;
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (node.Position.x + i < chessBoardSize)
                {
                    Node cheakNode = new Node(
                        node.G + verticalAndHorizontalMoveCostG,
                        new Vector2Int(node.Position.x + i, node.Position.y),
                        node.TargetPosition,
                        node);
                    ChessUnit chessUnit = grid.Get(cheakNode.Position);

                    if (chessUnit != null) 
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(cheakNode);
                    }
                }
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (node.Position.x - i >= 0)
                {
                    Node cheakNode = new Node(
                        node.G + verticalAndHorizontalMoveCostG,
                        new Vector2Int(node.Position.x - i, node.Position.y),
                        node.TargetPosition,
                        node);
                    ChessUnit chessUnit = grid.Get(cheakNode.Position);

                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(cheakNode);
                    }
                }
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (node.Position.y + i < chessBoardSize)
                {
                    Node cheakNode = new Node(
                        node.G + verticalAndHorizontalMoveCostG, 
                        new Vector2Int(node.Position.x, node.Position.y + i),
                        node.TargetPosition, 
                        node);
                    ChessUnit chessUnit = grid.Get(cheakNode.Position);

                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(cheakNode);
                    }
                }
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (node.Position.y - i >= 0)
                {
                    Node checkNode = new Node(
                        node.G + verticalAndHorizontalMoveCostG,
                        new Vector2Int(node.Position.x, node.Position.y - i),
                        node.TargetPosition, 
                        node);
                    ChessUnit chessUnit = grid.Get(checkNode.Position);

                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(checkNode);
                    }
                }
            }
            return neighbors;
        }

        public List<Node> GetNeighborsForBishop(Node node, ChessGrid grid)
        {
            int diagonalMoveCostG = 14;
            int chessBoardSize = 8;
            List<Node> neighbors = new List<Node>();

            for (int i = 1; i < chessBoardSize; i++)
            {
                if ((node.Position.x + i < chessBoardSize) && (node.Position.y + i < chessBoardSize))
                {
                    Node checkNode = new Node(
                        node.G + diagonalMoveCostG,
                        new Vector2Int(node.Position.x + i, node.Position.y + i),
                        node.TargetPosition,
                        node);
                    ChessUnit chessUnit = grid.Get(checkNode.Position);

                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(checkNode);
                    }
                }
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if ((node.Position.x - i >= 0) && (node.Position.y - i >= 0))
                {
                    Node checkNode = new Node(node.G + diagonalMoveCostG,
                        new Vector2Int(node.Position.x - i, node.Position.y - i),
                        node.TargetPosition,
                        node);
                    ChessUnit chessUnit = grid.Get(checkNode.Position);

                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(checkNode);
                    }
                }
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if ((node.Position.x - i >= 0) && (node.Position.y + i < chessBoardSize))
                {
                    Node checkNode = new Node(
                        node.G + diagonalMoveCostG,
                        new Vector2Int(node.Position.x - i, node.Position.y + i),
                        node.TargetPosition,
                        node);
                    ChessUnit chessUnit = grid.Get(checkNode.Position);

                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(checkNode);
                    }
                }
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if ((node.Position.x + i < chessBoardSize) && (node.Position.y - i >= 0))
                {
                    Node checkNode = new Node(
                        node.G + diagonalMoveCostG,
                        new Vector2Int(node.Position.x + i, node.Position.y - i),
                        node.TargetPosition,
                        node);
                    ChessUnit chessUnit = grid.Get(checkNode.Position);

                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(checkNode);
                    }
                }
            }
            return neighbors;
        }

        public List<Node> GetNeighborsForQueen(Node node, ChessGrid grid)
        {
            int verticalAndHorizontalMoveCostG = 10;
            int diagonalMoveCostG = 14;
            int chessBoardSize = 8;
            List<Node> neighbors = new List<Node>();

            for (int i = 1; i < chessBoardSize; i++)
            {
                if (node.Position.x + i < chessBoardSize)
                {
                    Node cheakNode = new Node(
                        node.G + verticalAndHorizontalMoveCostG,
                        new Vector2Int(node.Position.x + i, node.Position.y),
                        node.TargetPosition,
                        node);
                    ChessUnit chessUnit = grid.Get(cheakNode.Position);

                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(cheakNode);
                    }
                }
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (node.Position.x - i >= 0)
                {
                    Node cheakNode = new Node(
                        node.G + verticalAndHorizontalMoveCostG,
                        new Vector2Int(node.Position.x - i, node.Position.y),
                        node.TargetPosition,
                        node);
                    ChessUnit chessUnit = grid.Get(cheakNode.Position);

                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(cheakNode);
                    }
                }
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (node.Position.y + i < chessBoardSize)
                {
                    Node cheakNode = new Node(node.G + verticalAndHorizontalMoveCostG, new Vector2Int(node.Position.x, node.Position.y + i), node.TargetPosition, node);
                    ChessUnit chessUnit = grid.Get(cheakNode.Position);

                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(cheakNode);
                    }
                }
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if (node.Position.y - i >= 0)
                {
                    Node checkNode = new Node(node.G + verticalAndHorizontalMoveCostG, new Vector2Int(node.Position.x, node.Position.y - i), node.TargetPosition, node);
                    ChessUnit chessUnit = grid.Get(checkNode.Position);

                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(checkNode);
                    }
                }
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if ((node.Position.x + i < chessBoardSize) && (node.Position.y + i < chessBoardSize))
                {
                    Node checkNode = new Node(node.G + diagonalMoveCostG, new Vector2Int(node.Position.x + i, node.Position.y + i), node.TargetPosition, node);
                    ChessUnit chessUnit = grid.Get(checkNode.Position);
                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(checkNode);
                    }
                }
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if ((node.Position.x - i >= 0) && (node.Position.y - i >= 0))
                {
                    Node checkNode = new Node(node.G + diagonalMoveCostG, new Vector2Int(node.Position.x - i, node.Position.y - i), node.TargetPosition, node);
                    ChessUnit chessUnit = grid.Get(checkNode.Position);
                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(checkNode);
                    }
                }
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if ((node.Position.x - i >= 0) && (node.Position.y + i < chessBoardSize))
                {
                    Node checkNode = new Node(node.G + diagonalMoveCostG, new Vector2Int(node.Position.x - i, node.Position.y + i), node.TargetPosition, node);
                    ChessUnit chessUnit = grid.Get(checkNode.Position);
                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(checkNode);
                    }
                }
            }
            for (int i = 1; i < chessBoardSize; i++)
            {
                if ((node.Position.x + i < chessBoardSize) && (node.Position.y - i >= 0))
                {
                    Node checkNode = new Node(node.G + diagonalMoveCostG, new Vector2Int(node.Position.x + i, node.Position.y - i), node.TargetPosition, node);
                    ChessUnit chessUnit = grid.Get(checkNode.Position);
                    if (chessUnit != null)
                    {
                        break;
                    }
                    else
                    {
                        neighbors.Add(checkNode);
                    }
                }
            }
            return neighbors;
        }
    }
}