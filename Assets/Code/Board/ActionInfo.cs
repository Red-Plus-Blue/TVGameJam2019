using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Pathfinding;

namespace Game.Assets
{
    public enum ActionType
    {
        MOVE,
        ATTACK
    }

    public class ActionInfo
    {
        public ActionInfo Prerequisite;
        public Action Exectue;
        public Point Location;
        public ActionType ActionType;

        public ActionInfo(ActionInfo prerequisite, Action exectue, Point location, ActionType type)
        {
            Prerequisite = prerequisite;
            Exectue = exectue;
            Location = location;
            ActionType = type;
        }
    }
}