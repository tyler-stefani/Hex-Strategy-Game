using UnityEngine;
using System.Collections;
using Atheos;

public class Game : Turn
{
    public Game(string blueprint, string[][] unitStrings) : base(blueprint, unitStrings) { }

    public Game(int size, string[][] unitStrings) : base(size, unitStrings) { }

}
