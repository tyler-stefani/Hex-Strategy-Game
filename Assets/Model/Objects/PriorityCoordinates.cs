using System.Collections;
using System;

public class PriorityCoordinates : IComparable
{

    public PriorityCoordinates(int[] coordinates, int priority)
    {
        Coordinates = coordinates;
        Priority = priority;
    }

    public int[] Coordinates;
    public int Priority;

    public int CompareTo(object coords)
    {
        PriorityCoordinates other = (PriorityCoordinates)coords;
        return Priority - other.Priority;
    }

}
