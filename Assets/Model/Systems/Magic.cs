using Assets.Model.Enumerators;
using Assets.Model.Objects;
using System.Collections.Generic;

public class Magic : Combat
{
    public Magic(string blueprint, string[][] unitStrings) : base(blueprint, unitStrings) { }

    public Magic(int size, string[][] unitStrings) : base (size, unitStrings) { }

    public bool VerifyCast(int[] from, int[] to, int[] casterId, string spellName)
    {
        int range = Armies[casterId[0]].Spells[spellName].Range;
        int cost = Armies[casterId[0]].Spells[spellName].Cost;
        int distance = Board.GetPureDistance(from, to);
        int manaLeft = Armies[casterId[0]].Units[casterId[1]].GetManaLeft();

        return (range >= distance && manaLeft >= cost);
    }

    public int[][] FindTargets(int[] to, int armyIndex, string spellName)
    {
        List<int[]> targets = new List<int[]>();

        int[][] area = Armies[armyIndex].Spells[spellName].Area;

        foreach (int[] coords in area)
        {
            int[] currTarget = new int[3];
            for (int i = 0; i < 3; i++)
            {
                currTarget[i] = to[i] + coords[i];
            }
            targets.Add(currTarget);
        }
        return targets.ToArray();
    }

    public void Cast(int[] casterIndex, string spellName, int[][] targets)
    { 
        Spell spell = Armies[casterIndex[0]].Spells[spellName];
        Armies[casterIndex[0]].Units[casterIndex[1]].SpendMana(spell.Cost);

        switch (spell.Type)
        {
            case SpellType.Heal:
                foreach (int[] target in targets){
                    if (Board.IsOccupiedAt(target))
                    {
                        int[] targetUnitId = Board.GetUnitIdAt(target);
                        Armies[targetUnitId[0]].Units[targetUnitId[1]].Heal(spell.Value);
                    }
                }
                break;
            case SpellType.Buff:
                foreach (int[] target in targets)
                {
                    if (Board.IsOccupiedAt(target))
                    {
                        int[] targetUnitId = Board.GetUnitIdAt(target);
                        Armies[targetUnitId[0]].Units[targetUnitId[1]].Buff(spell.Value, spell.Stat);
                    }
                }
                break;
            case SpellType.Damage:
                foreach (int[] target in targets)
                {
                    if (Board.IsOccupiedAt(target))
                    {
                        int[] targetUnitId = Board.GetUnitIdAt(target);
                        bool killed = Armies[targetUnitId[0]].Units[targetUnitId[1]].TakeDamage(spell.Value);
                        if (killed)
                        {
                            Armies[targetUnitId[0]].Units.Remove(targetUnitId[1]);
                        }
                    }
                }
                break;
        }
    }
}