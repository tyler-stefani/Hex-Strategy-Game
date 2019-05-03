using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnScript : MonoBehaviour
{
    public void EndTurn()
    {
        GameObject gameObject = transform.root.gameObject;
        gameObject.GetComponent<GameBehavior>().OnEndTurn();
    }
}
