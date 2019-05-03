using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObjectScript : MonoBehaviour
{

    int[] UnitId;

    public void SetUnitId(int[] id)
    {
        UnitId = id;
    }

    public int[] GetUnitId()
    {
        return UnitId;
    }

    private void OnMouseDown()
    {
        GameObject game = transform.root.gameObject;
        GameBehavior gameBehavior = game.GetComponent<GameBehavior>();

        int[] coords = this.GetComponentInParent<TileObjectScript>().GetCoordinates();

        gameBehavior.OnUnitLeftClick(UnitId, coords);
    }
}
