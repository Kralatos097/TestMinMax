using System;
using System.Collections.Generic;

public abstract class Piece : ICloneable {

    public Coordinate CurrentCoordinate;
    public PlayerColor Player;
    
    public PlayerColor OtherPlayer => Player == PlayerColor.Black ? PlayerColor.White : PlayerColor.Black;
    
    public abstract int Value { get; }
    
    protected Piece(Coordinate currentCoordinate, PlayerColor player) {
        CurrentCoordinate = currentCoordinate;
        Player = player;
    }
    
    public abstract List<Coordinate> AvailableMoves(Board board);

    public int AvailableNomNom(Board board, List<Coordinate> availableList)
    {
        int som = 0;
        foreach (Coordinate coordinate in availableList)
        {
            if(!board.CanNomNomCoordinate(coordinate, Player)) som += Value;
        }

        return som;
    }
        
    public abstract void ExecuteMove(Board board, Coordinate destination);
    public abstract object Clone();
        
}