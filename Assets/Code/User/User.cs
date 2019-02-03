using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Game.Pathfinding;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Game.Assets
{
    [Serializable]
    public class User
    {
        protected static Regex InterpolationKeyRegex = new Regex(@"\$\{[^${}]+\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public Dictionary<string, string> MetaData = new Dictionary<string, string>();
        public bool FirstMenuLoad = true;

        public string Interpolate(string original)
        {
            var matches = InterpolationKeyRegex.Matches(original);
            var interpolated = original;

            foreach(var match in matches)
            {
                var key = match.ToString().Replace("${", "").Replace("}", "");
                if(MetaData.ContainsKey(key))
                {
                    var value = MetaData[key];
                    interpolated = interpolated.Replace(match.ToString(), value);
                }
            }

            return interpolated;
        }
    }
}
