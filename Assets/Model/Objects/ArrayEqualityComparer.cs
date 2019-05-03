using System;
using System.Collections.Generic;

namespace Assets.Model.Objects
{
    class ArrayEqualityComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] x, int[] y)
        {
            bool ret = true;

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    ret = false;
                    break;
                }
            }

            return ret;
        }

        public int GetHashCode(int[] obj)
        {
            int mult = 1;
            int hash = 0;
            foreach (int i in obj)
            {
                hash += Math.Abs(i);
                if (i >= 0)
                {
                    hash += mult * 10;
                }
                mult *= 1000;
            }
            return hash;
        }
    }
}
