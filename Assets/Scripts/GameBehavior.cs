using Assets.Controller;
using Assets.Model.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    readonly float HEIGHT_MULT = Mathf.Sqrt(3) / 2;

    // Start is called before the first frame update
    void Start()
    {
        string[][] units = new string[2][];
        units[0] = new string[] { "Knight" };
        units[1] = new string[] { "Knight" };
        Game = new Game(2, units);

        GenerateMap();

        for (int armyIndex = 0; armyIndex <= 1; armyIndex++)
        {
            int[] rowPriority = (armyIndex == 0) ? new int[] { 1, 2, 0, 3, -1 } : new int[] { -1, 0, -2, 1, -3 };
            foreach (int row in rowPriority)
            {
                int[] curr = new int[] { (armyIndex == 0) ? -2 : 2, row, -(((armyIndex == 0) ? -2 : 2) + row) };
                if (Game.Board.IsPassableAt(curr))
                {
                    PlaceUnitAt(curr, new int[] { armyIndex, 0 });
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Dictionary<int[], GameObject> Tiles;

    public Game Game;

    public GameObject TilePrefab;
    public GameObject UnitPrefab;

    public Material[] TileMaterials;
    public Material[] UnitMaterials;

    public void GenerateMap()
    {
        Tiles = new Dictionary<int[], GameObject>(new ArrayEqualityComparer());

        Dictionary<int[], Tile> tiles = Game.Board.Tiles;
        foreach (KeyValuePair<int[], Tile> pair in tiles)
        {
            GameObject curr = (GameObject)Instantiate(
                TilePrefab,
                GetPositionFromCoordinates(pair.Key),
                Quaternion.identity,
                this.transform
            );

            MeshRenderer mr = curr.GetComponentInChildren<MeshRenderer>();
            if (pair.Value.IsPassable)
            {
                mr.material = TileMaterials[0];
            }
            else
            {
                mr.material = TileMaterials[1];
            }

            curr.GetComponent<TileObjectScript>().SetCoordinates(pair.Key);

            Tiles.Add(pair.Key, curr);
        }
    }

    public void PlaceUnitAt(int[] coords, int[] unitId)
    {
        Game.PlaceUnitAt(coords, unitId);
        RenderUnit(coords, unitId);
    }

    public void RenderUnit(int[] coords, int[] unitId)
    {
        GameObject tileObject = Tiles[coords];

        GameObject unitObject = (GameObject)Instantiate(
            UnitPrefab,
            tileObject.transform.position,
            Quaternion.identity,
            tileObject.transform
            );

        MeshRenderer mr = unitObject.GetComponentInChildren<MeshRenderer>();
        mr.material = UnitMaterials[unitId[0]];

        unitObject.GetComponent<UnitObjectScript>().SetUnitId(unitId);
    }

    public void RenderMovement(int[] from, int[] to) {
        GameObject fromTile = Tiles[from];
        GameObject toTile = Tiles[to];

        GameObject unit = fromTile.transform.GetChild(1).gameObject;
        RenderUnit(toTile.GetComponent<TileObjectScript>().GetCoordinates(), unit.GetComponent<UnitObjectScript>().GetUnitId());
        Destroy(fromTile.transform.GetChild(1).gameObject);

    }

    public Vector3 GetPositionFromCoordinates(int[] coords)
    {
        float radius = 1f;
        float width = 2 * radius;
        float height = HEIGHT_MULT * width;

        float verticalSpacing = height;
        float horizontalSpacing = width * 0.75f;

        return new Vector3(
            horizontalSpacing * coords[0],
            0,
            verticalSpacing * (coords[1] + (coords[0] / 2f))
            );
    }

    /* CONTROLLER PART */

    public int TurnIndex;
    public int[] SelectedUnit;
    public int[] SelectedUnitCoords;
    public ActionType ActionType;
    public string SelectedSpell;

    public void OnUnitLeftClick(int[] unitId, int[] unitCoords)
    {
     
        if (unitId[0] == TurnIndex)
        {
            SelectedUnit = unitId;
            SelectedUnitCoords = unitCoords;
            ActionType = ActionType.Movement;
            // Display buttons for move, combat, magic, and unit info
            // Display possible tiles for movement (if movement left)
        }
        else
        {
            // Display unit information
        }
    }

    public void OnTileRightClick(int[] coords)
    {
        if (SelectedUnit == null)
        {
            return;
        }
        else
        {
            switch (ActionType)
            {
                case ActionType.Movement:
                    if (Game.VerifyMove(SelectedUnitCoords, coords, SelectedUnit))
                    {
                        Game.MoveUnit(SelectedUnitCoords, coords);
                        RenderMovement(SelectedUnitCoords, coords);
                        Deselect();
                    }
                    break;

                case ActionType.Combat:
                    if (Game.VerifyFight(SelectedUnitCoords, coords, SelectedUnit))
                    {
                        Game.Fight(SelectedUnit, coords);
                        Deselect();
                    }
                    break;

                case ActionType.Magic:
                    if (SelectedSpell == null)
                    {
                        break;
                    }
                    if (Game.VerifyCast(SelectedUnitCoords, coords, SelectedUnit, SelectedSpell))
                    {
                        Game.Cast(SelectedUnit, SelectedSpell, Game.FindTargets(coords, SelectedUnit[0], SelectedSpell));
                        Deselect();
                    }
                    break;

            }
        }
    }

    public void OnEndTurn()
    {
        Game.NewTurn();
        TurnIndex = Game.TurnIndex;
    }

    public void Deselect()
    {
        SelectedUnit = null;
        SelectedUnitCoords = null;
        SelectedSpell = null;
    }
}
