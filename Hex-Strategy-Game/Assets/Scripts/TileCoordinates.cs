using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TileCoordinates
{
    public readonly int Column;
    public readonly int Row;

    public TileCoordinates (int q, int r)
    {
        Column = q;
        Row = r;
    }
}
