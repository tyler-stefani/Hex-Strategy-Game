using System.Collections.Generic;

namespace Assets.Controller
{
    public class InGameController
    {
        
        public InGameController(int size, string[][] unitStrings)
        {
            Game = new Game(size, unitStrings);
            ConstructorHelper();
        }

        public InGameController(string blueprint, string[][] unitStrings)
        {
            Game = new Game(blueprint, unitStrings);
            ConstructorHelper();
        }

        public void ConstructorHelper()
        {
            PossibleTileCache = new List<int[]>[2];
            PossibleTileCache[0] = new List<int[]>();
            PossibleTileCache[1] = new List<int[]>();
        }

        public Game Game;

        public bool InSetupPhase;

        // I should cache the viable tile lookup so I don't have to call it every time a unit is placed
        public List<int[]>[] PossibleTileCache;

        public int TurnIndex;
        public int[] SelectedUnit;
        public int[] SelectedUnitCoords;
        public ActionType ActionType;
        public string SelectedSpell;

        public int FirstMoveIndex;

        public void InitTurn()
        {
            if (InSetupPhase)
            {
                int[][] possible = Game.GetPossiblePlacementsFor(TurnIndex);
                // Highlight deployment zone tiles in the view
                // Display list of units to place
            }
            else
            {
                Game.NewTurn();
            }
        }

        public void OnUnitLeftClick(int[] unitId, int[] unitCoords)
        {
            if (InSetupPhase)
            {
                SelectedUnit = unitId;
                SelectedUnitCoords = unitCoords;
            }
            else
            {
                if (unitId[0] == TurnIndex)
                {
                    // Display buttons for move, combat, magic, and unit info
                    // Display possible tiles for movement (if movement left)
                }
                else
                {
                    // Display unit information
                }
            }
        }

        public void OnActionSelect(string action)
        {
            switch (action)
            {
                case "Movement":
                    ActionType = ActionType.Movement;
                    break;
                case "Combat":
                    ActionType = ActionType.Combat;
                    break;
                case "Magic":
                    ActionType = ActionType.Magic;
                    break;
            }
        }

        public void OnTileRightClick(int[] coords)
        {
            if (SelectedUnit == null)
            {
                return;
            }
            if (InSetupPhase)
            {
                if (Game.VerifyPlacement(coords, SelectedUnit))
                {
                    Game.PlaceUnitAt(coords, SelectedUnit);
                    SelectedUnit = null;
                    if (TurnIndex == 0)
                    {
                        TurnIndex = 1;
                    }
                    else
                    {
                        TurnIndex = 0;
                    }
                    InitTurn();
                }

            }
            else
            {
                switch (ActionType)
                {
                    case ActionType.Movement:
                        if (Game.VerifyMove(SelectedUnitCoords, coords, SelectedUnit))
                        {
                            Game.MoveUnit(SelectedUnitCoords, coords);
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

        public void OnSpellClick(string spell)
        {
            SelectedSpell = spell;
        }

        public void Deselect()
        {
            SelectedUnit = null;
            SelectedUnitCoords = null;
            SelectedSpell = null;
        }
    }

    public enum ActionType
    {
        Movement,
        Combat,
        Magic
    }
}
