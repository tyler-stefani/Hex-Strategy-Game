using Assets.Model.Enumerators;
using System;

namespace Assets.Model.Interfaces
{
    public interface IUnit
    {
        int GetPower();
        int GetArmor();
        int GetDamage();
        int GetMovementLeft();
        int GetManaLeft();
        int GetAttackRange();
        void Move(int distance);
        bool TakeDamage(int damage);
        void Heal(int healing);
        void Buff(int buff, BuffStat stat);
        String[] GetSpells();
        void SpendMana(int mana);
        void TurnReset();
    }
}
