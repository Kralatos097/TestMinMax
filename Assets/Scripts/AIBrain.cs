using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIBrain {

    public Board Board;
    public PlayerColor Player;
    public int DepthSearch;
    
    private List<Tuple<int, Node>> Nodes = new List<Tuple<int, Node>>();

    public AIBrain(Board board, PlayerColor player, int depthSearch) {
        Board = board;
        Player = player;
        DepthSearch = depthSearch;
    }

    public void Think() {
        Nodes.Clear();
        float startingTime = Time.realtimeSinceStartup;
        foreach (Piece availablePiece in Board.AvailablePieces(Player)) {
            foreach (Coordinate availableMove in availablePiece.AvailableMoves(Board)) {
                Node node = new Node(Board, Player, Player, availablePiece.CurrentCoordinate, availableMove);
                
                int value = MinMax(node, DepthSearch, false);
                //int value = NegaMax(node, DepthSearch, -1);
                //int value = AlphaBeta(node, DepthSearch, false, -100000000, 100000000);
                //int value = NegaAlphaBeta(node, DepthSearch, false, -10000000);
                
                Nodes.Add(new Tuple<int, Node>(value, node));
            }
        }
        Debug.Log("Reflexion took about : " + (Time.realtimeSinceStartup - startingTime) + " seconds");
    }

    public void Act() {
        if (Nodes.Count == 0) throw new Exception("MinMax results is empty");
        int bestValue = Nodes.Max(node => node.Item1);
        Nodes.RemoveAll(node => node.Item1 < bestValue);
        Tuple<int, Node> selectedTuple = Nodes[Random.Range(0, Nodes.Count)];
        Board.GetPiece(selectedTuple.Item2.MoveOrigin).ExecuteMove(Board, selectedTuple.Item2.MoveDestination);
    }
    
    private int MinMax(Node node, int depth, bool isMax)
    {
        if (depth <= 0 || node.IsTerminal)
        {
            return node.HeuristicValue;
        }

        int value;
        if (isMax)
        {
            value = -1000000000;

            foreach (Node nodeChild in node.Children)
            {
                value = Mathf.Max(value, MinMax(nodeChild, depth-1, false));
            }
        }
        else
        {
            value = 1000000000;
            foreach (Node nodeChild in node.Children)
            {
                value = Mathf.Min(value, MinMax(nodeChild, depth-1, true));
            }
        }
        return value;
    }
    
    private int NegaMax(Node node, int depth, int isMax)
    {
        if (depth <= 0 || node.IsTerminal)
        {
            return isMax * node.HeuristicValue;
        }

        int value = -1000000000;

        foreach (Node nodeChild in node.Children)
        {
            value = Mathf.Max(value, -NegaMax(nodeChild, depth-1, -isMax));
        }
        return value;
    }
    
    private int AlphaBeta(Node node, int depth, bool isMax, int alpha, int beta)
    {
        if (depth <= 0 || node.IsTerminal)
        {
            return node.HeuristicValue;
        }

        int value;
        if (isMax)
        {
            value = -1000000000;

            foreach (Node nodeChild in node.Children)
            {
                value = Mathf.Max(value, AlphaBeta(nodeChild, depth-1, true, alpha, beta));
                if (value >= beta)
                {
                    return value;
                }

                alpha = Mathf.Max(alpha, value);
            }
        }
        else
        {
            value = 1000000000;
            foreach (Node nodeChild in node.Children)
            {
                value = Mathf.Min(value, AlphaBeta(nodeChild, depth-1, true, alpha, beta));
                if (value <= alpha)
                {
                    return value;
                }

                beta = Mathf.Min(beta, value);
            }
        }
        return value;
    }
    
    private int NegaAlphaBeta(Node node, int depth, bool isMax, int alpha)
    {
        if (depth <= 0 || node.IsTerminal)
        {
            return node.HeuristicValue;
        }

        int value = -1000000000;
        foreach (Node nodeChild in node.Children)
        {
            value = Mathf.Max(value, NegaAlphaBeta(nodeChild, depth-1, true, alpha));
            if (-value <= alpha)
            {
                return -value;
            }
        }
        return -value;
    }
}