using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;

public static class BlueprintConverter
{

    public static Dictionary<int[], bool> Convert(string file)
    {
        Dictionary<int[], bool> coordsAndTerrain = new Dictionary<int[], bool>(0);

        var lines = System.IO.File.ReadAllLines("\\Projects\\Atheos\\Assets\\Model\\Objects\\Blueprints\\" + file + ".txt");

        foreach (string line in lines)
        {
            string[] columns = line.Split(',');

            int[] coords = {
                int.Parse(columns[0]),
                int.Parse(columns[1]),
                int.Parse(columns[2])
            };

            coordsAndTerrain.Add(coords, columns[3].Equals("T"));
        }

        return coordsAndTerrain;
    }
   
}
