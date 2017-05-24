using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawlers.Utils.SpawningTools
{
    public enum SortBy
    {
        Hot = 'h',
        Top = 't',
        New = 'r',
        Alphabetical = 'a'
    }

    public enum BuildType
    {
        All,
        Cheese = 1,
        AllIn = 2,
        TimingAttack = 3,
        Economic = 4
    }

    public enum Patch
    {
        Last = 110,
        Pre = 100
    }
}
