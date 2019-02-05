using System;

namespace Game.Assets
{
    public class CombatResult
    {
        public int Damage;
        public int LuckRoll;
    }

    public class Combat
    {
        public static CombatResult Attack(Pawn attacker, Pawn defender)
        {
            var result = new CombatResult();
            var random = new Random();

            var luckRoll = random.Next(100 - defender.Defence);
            result.LuckRoll = luckRoll;

            var damage = Damage(attacker.Attack, defender.Defence, attacker.Damage, luckRoll);
            result.Damage = damage;
            
            return result;
        }

        public static int Damage(int attackStat, int defenceStat, int damageStat, int roll)
        {
            var attack = attackStat + roll;
            var defence = defenceStat;

            var damage = (int)(
                (((float)attack) / (12 * defence)) * damageStat
            );

            return damage;
        }
    }
}