using Assets.Model.Enumerators;
using System.IO;
using UnityEngine;

namespace Assets.Model.Objects
{
    public class Spell
    {
        public Spell(string name)
        {
            string path = Application.streamingAssetsPath + "/SpellStats.csv";
            string[] stats;
            using (var reader = new StreamReader(path))
            {
                stats = reader.ReadLine().Split(',');
                while (!stats[0].Equals(name))
                {
                    stats = reader.ReadLine().Split(',');
                }
            }
            switch (stats[1])
            {
                case "heal":
                    Type = SpellType.Heal;
                    break;
                case "damage":
                    Type = SpellType.Damage;
                    break;
                case "buff":
                    Type = SpellType.Buff;
                    break;
                case "movement":
                    Type = SpellType.Movement;
                    break;
            }
            Cost = int.Parse(stats[2]);
            Range = int.Parse(stats[3]);
            Duration = int.Parse(stats[4]);
            Targeted = bool.Parse(stats[5]);
            Value = int.Parse(stats[7]);
        }

        public SpellType Type;
        public int Cost;
        public int Range;
        public int Duration;
        public bool Targeted;
        public int[][] Area;
        public int Value;
        public BuffStat Stat;

        public bool CanCast(int mana)
        {
            return Cost <= mana;
        }
    }
}
