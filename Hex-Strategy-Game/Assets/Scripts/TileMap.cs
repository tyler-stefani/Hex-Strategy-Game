using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();

        // List of coordinates of initial spawns, Remove this when implementing turn based placement
        int[,] initSpawnLocations =
        {
            {0, -4},
            {0, -2},
            {0, 0},
            {0, 2},
            {0, 4},
        };

        //Remove this when implementing turn based placement
        SpawnInitUnits(initSpawnLocations);

    }

    public GameObject TilePrefab;

    // Placeholder for units, there will need to be a different prefab for each unit
    public GameObject UnitPrefab;

    // Dictionaries to store tiles and game objects for ease of lookup
    private Dictionary<TileCoordinates, Tile> MapTiles;
    private Dictionary<Tile, GameObject> TileGameObjectDict;

    public Tile GetTileAt(int q, int r)
    {
        TileCoordinates coords = new TileCoordinates(q, r);
        Tile output;
        if (MapTiles.TryGetValue(coords, out output)){
            return output;
        }
        else
        {
            // There is no tile at these coordinates
            // TODO: find a better way of returning this maybe
            return null;
        }
    }

    public GameObject GetGameObjectFromTile(Tile tile)
    {
        return TileGameObjectDict[tile];
    }

    private void GenerateMap()
    {
        MapTiles = new Dictionary<TileCoordinates, Tile>();
        TileGameObjectDict = new Dictionary<Tile, GameObject>();

        // Add the center line tiles
        for (int r = -4; r<=4; r++)
        {
            Tile currentTile = new Tile(0, r);

            GameObject currentGameObject = (GameObject)Instantiate(
                TilePrefab,
                currentTile.Position(),
                Quaternion.identity,
                this.transform);

            MapTiles[new TileCoordinates(0, r)] = currentTile;
            TileGameObjectDict[currentTile] = currentGameObject;
        }

        // Add tiles to the right as well as their mirrored tiles on the left
        for (int q = 1; q <= 5; q++)
        {

            for (int r = (-4 - (q / 2)); r <= (4 - (q / 2)) - (q % 2); r++)
            {
                // Initial right tile
                Tile currentTile = new Tile(q, r);

                GameObject currentGameObject = (GameObject)Instantiate(
                    TilePrefab,
                    currentTile.Position(),
                    Quaternion.identity,
                    this.transform);

                MapTiles[new TileCoordinates(q, r)] = currentTile;
                TileGameObjectDict[currentTile] = currentGameObject;

                // Mirrored left tile
                Tile mirroredTile = new Tile(-q, -(currentTile.Sum));

                GameObject mirroredGameObject = (GameObject)Instantiate(
                    TilePrefab,
                    mirroredTile.Position(),
                    Quaternion.identity,
                    this.transform);

                MapTiles[new TileCoordinates(mirroredTile.Column, mirroredTile.Row)] = mirroredTile;
                TileGameObjectDict[mirroredTile] = mirroredGameObject;
            }
        }

        // Add some Units
        // TODO: call this at the start of the game when placing units
    }

    // Remove this when implementing turn based placement
    public void SpawnInitUnits(int[,] coords)
    {
        for (int i = 0; i < coords.GetLength(0); i++)
        {
            SpawnUnit(UnitPrefab, coords[i, 0], coords[i, 1]);
        }
    }

    public void SpawnUnit (GameObject unit, int q, int r)
    {
        Tile tile = GetTileAt(q, r);
        GameObject tileGameObject = GetGameObjectFromTile(tile);

        Instantiate(
            unit,
            tileGameObject.transform.position,
            Quaternion.identity,
            tileGameObject.transform
            );
    }

    public List<Tile> GetTilesInRangeMovement (Tile origin, int range)
    {
        return RecursiveRangeHelper(origin, origin.Column, origin.Row, origin.Sum, range, 0);
    }

    // Finds passable neighbors for each tile
    // Base Case: full range away
    public List<Tile> RecursiveRangeHelper (Tile currTile, int originalColumn, int originalRow, int originalSum, int range, int distanceFromOrigin)
    {
        List<Tile> inRange = new List<Tile>();

        // Base Case
        if (distanceFromOrigin == range)
        {
            inRange.Add(currTile);
            return inRange;
        }

        List<Tile> neighbors = GetNeighbors(currTile);

        // Check if each tile is passable and also if it is new in the recursion
        foreach (Tile t in neighbors)
        {
            // Raw distance from origin, not considering impassible tiles
            int ring =
                (Mathf.Abs(t.Column - originalColumn) +
                Mathf.Abs(t.Row - originalRow) +
                Mathf.Abs(t.Sum - originalSum)) / 2;

            if (t.Impassable == false && ring > distanceFromOrigin)
            {
                inRange.AddRange(RecursiveRangeHelper(t, originalColumn, originalRow, originalSum, range, distanceFromOrigin + 1));
            }
        }

        return inRange;
    }

    // Finds neighbors of each tile
    public List<Tile> GetNeighbors (Tile tile)
    {
        int q = tile.Column;
        int r = tile.Row;

        // 6 pairs of +1 and -1 to each cubic coordinate
        return new List<Tile>(
            new Tile[]
            {
                GetTileAt(q + 1, r),
                GetTileAt(q + 1, r - 1),
                GetTileAt(q, r + 1),
                GetTileAt(q, r - 1),
                GetTileAt(q - 1, r + 1),
                GetTileAt(q - 1, r)
            }
        );
    }
}
