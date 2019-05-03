using Assets.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Model.Objects.UnitTypes
{
    public class Knight : Unit, IUnit
    {
        public Knight(int armyIndex) : base(armyIndex, "Knight") {}
    }
}
