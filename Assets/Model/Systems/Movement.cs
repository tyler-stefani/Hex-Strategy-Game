using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Model.Objects;
using System.Linq;

public class Movement : Magic
{
    public Movement (string blueprint, string[][] unitStrings) : base (blueprint, unitStrings) { }

    public Movement (int size, string[][] unitStrings) : base (size, unitStrings) { }

    public int[][] GetPossibleMoves(int[] from, int range)
    {
        List<int[]> visited = new List<int[]>();
        visited.Add(from);
        List<int[][]> inRange = new List<int[][]>
        {
            new[] { from }
        };

        for (int i = 1; i <= range; i++)
        {
            List<int[]> currRange = new List<int[]>();

            for (int j = 0; j < inRange[i-1].Length; j++)
            {
                int[][] currPassableNeighbors = Board.GetPassableNeighborsFor(inRange[i-1][j]);
                for (int k = 0; k < currPassableNeighbors.Length; k++)
                {
                    if (!visited.Contains(currPassableNeighbors[k], new ArrayEqualityComparer()))
                    {
                        visited.Add(currPassableNeighbors[k]);
                        currRange.Add(currPassableNeighbors[k]);
                    }
                }
            }
            inRange.Add(currRange.ToArray());
        }

        return visited.ToArray();
    }

    public bool VerifyMove(int[] from, int[] to, int[] unitId)
    {
        int movementLeft = Armies[unitId[0]].Units[unitId[1]].GetMovementLeft();
        int moveDistance = Board.GetDistanceMovement(from, to);

        return (movementLeft >= moveDistance);
    }

    public int MoveUnit(int[] from, int[] to)
    {
        int[] unitId = this.Board.GetUnitIdAt(from);
        int range = Armies[unitId[0]].Units[unitId[1]].GetMovementLeft();
        int moveDistance = Board.GetDistanceMovement(from, to);

        Board.RemoveUnitFrom(from);
        Board.PlaceUnitAt(to, unitId);

        return range - moveDistance;
    }

}
