using UnityEngine;
using System.IO;
using Assets.Model.Enumerators;

public class Unit
{
    public Unit(int armyIndex, string type)
    {
        ArmyIndex = armyIndex;
        Type = type;

        //// FILL IN STATS HERE (FROM STATS LIBRARY)
        //string path = Application.streamingAssetsPath + "/UnitStats.csv";
        //string[] stats;
        //using (var reader = new StreamReader(path))
        //{
        //    stats = reader.ReadLine().Split(',');
        //    while (!stats[0].Equals(type))
        //    {
        //        stats = reader.ReadLine().Split(',');
        //    }
        //}

        string[] stats = new string[] { "Knight", "2", "3", "3", "1", "2" };

        Health = int.Parse(stats[1]);
        Power = int.Parse(stats[2]);
        Damage = int.Parse(stats[3]);
        Armor = int.Parse(stats[4]);
        Speed = int.Parse(stats[5]);

        HealthLeft = Health;
        PowerBuff = 0;
        DamageBuff = 0;
        ArmorBuff = 0;
        SpeedBuff = 0;
        MovementLeft = Speed;
        ManaLeft = 0;
        HasActed = false;
    }

    public int ArmyIndex;
    public string Name;
    public string Type;

    // Intrinsic Stats, looked up from dictionary depending on unit type
    public int Health;
    public int Power;
    public int Damage;
    public int Armor;
    public int Speed;
    public int Range;

    // Stats that change over the course of the game
    public int HealthLeft;
    public int PowerBuff;
    public int DamageBuff;
    public int ArmorBuff;
    public int SpeedBuff;
    public int MovementLeft;
    public int ManaLeft;
    public bool HasActed;

    // List of spells they can cast
    public string[] Spells;

    // Movement reset for the start of the turn, the same for every unit type
    public void TurnReset()
    {
        MovementLeft = Speed + SpeedBuff;
        HasActed = false;
    }

    public int GetPower()
    {
        return Power + PowerBuff;
    }

    public int GetArmor()
    {
        return Armor + ArmorBuff;
    }

    public int GetDamage()
    {
        return Damage + DamageBuff;
    }

    public int GetMovementLeft()
    {
        return MovementLeft;
    }

    public int GetManaLeft()
    {
        return ManaLeft;
    }

    public int GetAttackRange()
    {
        return Range;
    }

    public void Move(int distance)
    {
        MovementLeft -= distance;
    }

    public bool TakeDamage(int damage)
    {
        HealthLeft -= damage;
        return (HealthLeft < 1);
    }

    public void Heal(int healing)
    {
        if (HealthLeft + healing >= Health)
        {
            HealthLeft = Health;
        }
        else
        {
            HealthLeft += healing;
        }
    }

    public string[] GetSpells()
    {
        if (Spells == null)
        {
            return new string[0];
        }
        return Spells;
    }

    public void SpendMana(int mana)
    {
        ManaLeft -= mana;
    }

    public void Buff(int buff, BuffStat stat)
    {
        switch (stat)
        {
            case BuffStat.Power:
                PowerBuff += buff;
                break;
            case BuffStat.Armor:
                ArmorBuff += buff;
                break;
            case BuffStat.Damage:
                DamageBuff += buff;
                break;
            case BuffStat.Speed:
                SpeedBuff += buff;
                break;
        }
    }
}
