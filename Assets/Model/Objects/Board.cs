using System;
using System.Collections.Generic;
using DataStructures;
using Assets.Model.Objects;
using System.Linq;

namespace Atheos
{
    public class Board
    {

        // Blueprinted board generation
        public Board(string blueprint)
        {
            Tiles = new Dictionary<int[], Tile>(new ArrayEqualityComparer());

            Dictionary<int[], bool> coordsAndTerrain = BlueprintConverter.Convert(blueprint);

            foreach (KeyValuePair<int[], bool> pair in coordsAndTerrain)
            {
                AddTile(pair.Key);
                if (pair.Value)
                {
                    AddTerrain(pair.Key);
                }
            }
        }

        // Random terrain
        public Board(int size)
        {
            Tiles = new Dictionary<int[], Tile>(new ArrayEqualityComparer());
            for (int q = -size; q <= size; q++)
            {
                int startRow = -size - (int)Math.Floor((double)q / 2);
                for (int r = startRow; r < startRow + ((q%2==0) ? size * 2 + 1 : size * 2); r++)
                {
                    AddTile(new int[] { q, r, -(q + r) });
                }
            }

            // Separate map into quadrants, roll number of impassable
            Random random = new Random();
            int qRand = 0;
            int rRand = 0;
            int[][] quads = new int[][]
            {
                new int[]{0,size,0,size},
                new int[]{1,size,-size,-1},
                new int[]{-size,0,-size,0},
                new int[]{-size,-1,1,size}
            };

            // Loop through quadrants
            foreach (int[] quad in quads){
                for (int i = 0; i < random.Next(1, size+1); i++)
                {
                    bool canPlace = false;
                    while (!canPlace)
                    {
                        qRand = random.Next(quad[0], quad[1]+1);
                        rRand = random.Next(quad[2], quad[3]+1);
                        int[] coords = new int[] { qRand, rRand, -(qRand + rRand) };
                        if (HasTileAt(coords))
                        {
                            if (IsPassableAt(new int[] { qRand, rRand, -(qRand + rRand) }))
                            {
                                canPlace = true;
                            }
                        }
                    }
                    AddTerrain(new int[] { qRand, rRand, -(qRand + rRand) });
                }
            }
        }

        public Dictionary<int[], Tile> Tiles;

        public void AddTile(int[] coords)
        {
            Tiles.Add(coords, new Tile(coords[0], coords[1], coords[2]));
        }

        public void AddTerrain(int[] coords)
        {
            Tile tile = Tiles[coords];
            tile.PlaceTerrain();
            Tiles[coords] = tile;
        }

        public bool HasTileAt(int[] coords)
        {
            return Tiles.ContainsKey(coords);
        }

        public bool IsOccupiedAt(int[] coords)
        {
            return Tiles[coords].IsOccupied;
        }

        public bool IsPassableAt(int[] coords)
        {
            return Tiles[coords].IsPassable;
        }

        public int[][] GetPassableNeighborsFor(int[] coords)
        {
            List<int[]> passableNeighbors = new List<int[]>();

            int[][] possibleNeighbors = GetPossibleNeighborsFor(coords);

            foreach (int[] neighbor in possibleNeighbors)
            {
                if (HasTileAt(neighbor))
                {
                    if (!IsOccupiedAt(neighbor) && IsPassableAt(neighbor))
                    {
                        passableNeighbors.Add(neighbor);
                    }
                }
            }

            return passableNeighbors.ToArray();
        }

        public int[][] GetPossibleNeighborsFor(int[] coords)
        {
            return new int[][]
            {
            new int[]{coords[0]+1, coords[1]-1, coords[2]},
            new int[]{coords[0]-1, coords[1]+1, coords[2]},
            new int[]{coords[0]+1, coords[1], coords[2]-1},
            new int[]{coords[0]-1, coords[1], coords[2]+1},
            new int[]{coords[0], coords[1]+1, coords[2]-1},
            new int[]{coords[0], coords[1]-1, coords[2]+1}
            };
        }

        public int[] GetUnitIdAt(int[] coords)
        {
            return Tiles[coords].UnitId;
        }

        public void PlaceUnitAt(int[] coords, int[] unitId)
        {
            Tiles[coords].PlaceUnit(unitId);
        }

        public void RemoveUnitFrom(int[] coords)
        {
            Tiles[coords].RemoveUnit();
        }

        public int GetPureDistance(int[] from, int[] to)
        {
            int distance = 0;
            for (int i = 0; i < 3; i++)
            {
                distance += Math.Abs(to[i] - from[i]);
            }
            return distance / 2;
        }

        public int GetDistanceMovement(int[] from, int[] to)
        {
            PriorityQueue<PriorityCoordinates> frontier = new PriorityQueue<PriorityCoordinates>();

            Dictionary<int[], int[]> cameFrom = new Dictionary<int[], int[]>
                (new ArrayEqualityComparer())
        {
            { from, null }
        };
            Dictionary<int[], int> distSoFar = new Dictionary<int[], int>
                (new ArrayEqualityComparer())
        {
            { from, 0 }
        };

            frontier.Add(new PriorityCoordinates(from, 0));
            while (frontier.Count > 0)
            {
                int[] currCoords = frontier.Take().Coordinates;

                if (currCoords.SequenceEqual(to))
                {
                    break;
                }

                int[][] neighbors = GetPassableNeighborsFor(currCoords);
                foreach (int[] next in neighbors)
                {
                    int oldDist = distSoFar[currCoords];
                    int newDist = oldDist + 1;
                    bool foundShorterPath = false;
                    if (!distSoFar.ContainsKey(next))
                    {
                        foundShorterPath = true;
                    }
                    else
                    {
                        foundShorterPath |= distSoFar[next] > newDist;
                    }

                    if (foundShorterPath)
                    {
                        if (distSoFar.ContainsKey(next))
                        {
                            distSoFar.Remove(next);
                        }
                        distSoFar.Add(next, newDist);
                        int priority = newDist + GetPureDistance(next, to);
                        frontier.Add(new PriorityCoordinates(next, priority));
                        cameFrom.Add(next, currCoords);
                    }
                }
            }
            return distSoFar[to];
        }
    }
}
