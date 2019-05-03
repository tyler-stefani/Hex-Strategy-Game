using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Model.Interfaces;
using Assets.Model.Objects;

public class Army
{
    public Army(IUnit[] units)
    {
        Units = new Dictionary<int, IUnit>();
        Spells = new Dictionary<string, Spell>();
        for (int i = 0; i < units.Length; i++)
        {
            Units.Add(i, units[i]);

            foreach (string spell in units[i].GetSpells())
            {
                if (!Spells.ContainsKey(spell))
                {
                    Spells.Add(spell, new Spell(spell));
                }
            }
        }
    }

    public Dictionary<int, IUnit> Units;
    public Dictionary<string, Spell> Spells;

    public void UpdateTurn()
    {
        foreach (IUnit unit in Units.Values)
        {
            unit.TurnReset();
        }
    }
}
