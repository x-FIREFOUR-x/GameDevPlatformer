
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StatsSystem.Enum;

namespace StatsSystem
{
    public interface IStatValueGiver
    {
        float GetStatValue(StatType statType);
    }
}
