using UnityEngine;
using System.Collections;

public interface IGameMode
{
    bool UpdateScore();
    int GetWinner();
}
