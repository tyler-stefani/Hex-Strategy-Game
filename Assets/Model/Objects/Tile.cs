using UnityEngine;
using System.Collections;

public class Tile
{
    // Constructor
    public Tile(int q, int r, int s)
    {
        Coords = new int[] {q, r, s};

        IsPassable = true;
        IsSeeThrough = true;
        IsOccupied = false;
    }

    // Coordinates of tile
    // Using cubic coordinates for hexagonal tiles, sum(Coords) == 0
    public int[] Coords;

    // Status indicators
    public bool IsPassable;
    public bool IsSeeThrough;
    public bool IsOccupied;

    // 0 index is the army index in the game
    // 1 index is the unit key within the army
    public int[] UnitId;

    public void PlaceUnit(int[] unitId)
    {
        IsOccupied = true;
        UnitId = unitId;
    }

    public void RemoveUnit()
    {
        IsOccupied = false;
        UnitId = null;
    }

    public void PlaceTerrain()
    {
        IsPassable = false;
    }
}
