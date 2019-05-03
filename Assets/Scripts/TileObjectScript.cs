using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObjectScript : MonoBehaviour
{
    int[] Coordinates;

    public void SetCoordinates(int[] coords)
    {
        Coordinates = coords;
    }

    public int[] GetCoordinates()
    {
        return Coordinates;
    }

    private void OnMouseDown()
    {
        GameObject game = transform.root.gameObject;
        GameBehavior gameBehavior = game.GetComponent<GameBehavior>();

        gameBehavior.OnTileRightClick(Coordinates);
    }
}
