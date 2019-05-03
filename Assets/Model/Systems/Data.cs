using UnityEngine;
using System.Collections;
using Atheos;
using System;
using Assets.Model.Interfaces;
using System.Collections.Generic;
using Assets.Model.Objects;

public class Data
{
    private const string UNITNAMESPACE = "Assets.Model.Objects.UnitTypes.";

    public Data(string blueprint, string[][] unitStrings)
    {
        Board = new Board(blueprint);

        ConstructArmies(unitStrings);
    }

    public Data(int size, string[][] unitStrings)
    {
        Board = new Board(size);

        ConstructArmies(unitStrings);
    }

    public void ConstructArmies(string[][] unitStrings)
    {
        IUnit[][] units = new IUnit[2][];
        ToPlace = new List<int>[2];
        ToPlace[0] = new List<int>();
        ToPlace[1] = new List<int>();

        for (int i = 0; i < 2; i++)
        {
            List<IUnit> unitList = new List<IUnit>();
            int unitCounter = 0;
            foreach (string unit in unitStrings[i])
            {
                ToPlace[i].Add(unitCounter);

                Type t = Type.GetType(UNITNAMESPACE + unit);
                unitList.Add((IUnit)Activator.CreateInstance(t, i));

                unitCounter++;
            }
            units[i] = unitList.ToArray();
        }

        Armies = new Army[2];
        Armies[0] = new Army(units[0]);
        Armies[1] = new Army(units[1]);
    }

    public Board Board;
    public Army[] Armies;

    List<int>[] ToPlace;

    public int[][] GetPossiblePlacementsFor(int armyIndex)
    {
        List<int[]> possible = new List<int[]>();

        foreach (int[] coords in Board.Tiles.Keys)
        {
            if (((armyIndex == 0 && coords[0] < -1) || (armyIndex == 1 && coords[0] > 1)) 
                && Board.IsPassableAt(coords) && !Board.IsOccupiedAt(coords))
            {
                possible.Add(coords);
            }
        }

        return possible.ToArray();
    }

    public bool VerifyPlacement(int[] coords, int[] unitId)
    {
        return (((unitId[0] == 0 && coords[0] < -1) || (unitId[0] == 1 && coords[0] > 1))
                && Board.IsPassableAt(coords) && !Board.IsOccupiedAt(coords));
    }

    public void PlaceUnitAt(int[] coords, int[] unitId)
    {
        Board.PlaceUnitAt(coords, unitId);
    }

    public void RemoveFromToPlace(int[] unitId)
    {
        if (ToPlace[unitId[0]].Contains(unitId[1]))
        {
            ToPlace[unitId[0]].Remove(unitId[1]);
        }
    }
}
