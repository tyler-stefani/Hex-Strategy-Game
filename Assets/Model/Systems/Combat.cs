using System;

public class Combat : Data
{
    public Combat(string blueprint, string[][] unitStrings) : base(blueprint, unitStrings) { }

    public Combat(int size, string[][] unitStrings) : base (size, unitStrings) { }

    public bool VerifyFight(int[] from, int[] to, int[] attackerId)
    {
        int range = Armies[attackerId[0]].Units[attackerId[1]].GetAttackRange();
        int distance = Board.GetPureDistance(from, to);
        int[] defenderId = Board.GetUnitIdAt(to);

        return (attackerId[0] != defenderId[0] && range >= distance);
    }

    public bool Fight(int[] attackerId, int[] defenderCoords)
    {
        int[] defenderId = Board.GetUnitIdAt(defenderCoords);
        int power = Armies[attackerId[0]].Units[attackerId[1]].GetPower();
        int armor = Armies[defenderId[0]].Units[defenderId[1]].GetArmor();

        int powerRoll = 0;
        int armorRoll = 0;

        Random random = new Random();
        while (powerRoll == armorRoll)
        {
            powerRoll = random.Next(0, power);
            armorRoll = random.Next(0, armor);
        }

        if (powerRoll > armorRoll)
        {
            int damage = Armies[attackerId[0]].Units[attackerId[1]].GetDamage();
            bool killed = Armies[defenderId[0]].Units[defenderId[1]].TakeDamage(damage);
            if (killed)
            {
                Armies[defenderId[0]].Units.Remove(defenderId[1]);
            }
        }
        return (powerRoll > armorRoll);
    }
}