using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class contains information about the tiles of the game board

public class Tile
{
    // Constructor
    public Tile(int q, int r)
    {
        this.Column = q;
        this.Row = r;
        this.Sum = -(q + r);
        this.Impassable = false;
        this.Opaque = false;
        this.Occupied = false;
    }

    // Cubic coordinates of tile
    public readonly int Column;
    public readonly int Row;
    public readonly int Sum;

    // Status indicators
    public bool Impassable;
    public bool Opaque;
    public bool Occupied;

    // static values used in mutiple loops
    static readonly float HEIGHT_MULT = Mathf.Sqrt(3) / 2;

    // Returns the world position based the coordinates
    public Vector3 Position()
    {
        float radius = 1f;
        float width = 2 * radius;
        float height = HEIGHT_MULT * width;

        float vertical_spacing = height;
        float horizontal_spacing = width * 0.75f;

        return new Vector3(
            horizontal_spacing * this.Column,
            0,
            vertical_spacing * (this.Row + (this.Column / 2f))
        );
    }

    public void UnitArrives()
    {
        this.Impassable = true;
        this.Occupied = true;
    }

    public void UnitDeparts()
    {
        this.Impassable = false;
        this.Occupied = false;
    }
}
