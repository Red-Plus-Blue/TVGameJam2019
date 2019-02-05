using System.Collections.Generic;
using System;

namespace Game.MultiverseData
{
    public class Multiverse
    {
        public List<Dimension> Dimensions = new List<Dimension>();

        public static Multiverse Generate()
        {
            var multiverse = new Multiverse();

            var startingDimensions = 30;
            for(int index = 0; index < startingDimensions; index++)
            {
                MultiverseCharacter Rich()
                {
                    return new MultiverseCharacter()
                    {
                        Name = "Rich",
                        Status = CharacterStatus.ALIVE
                    };
                }

                MultiverseCharacter Dorky()
                {
                    return new MultiverseCharacter()
                    {
                        Name = "Dorky",
                        Status = CharacterStatus.ALIVE
                    };
                }

                MultiverseCharacter Simmer()
                {
                    return new MultiverseCharacter()
                    {
                        Name = "Simmer",
                        Status = CharacterStatus.ALIVE
                    };
                }

                MultiverseCharacter Barry()
                {
                    return new MultiverseCharacter()
                    {
                        Name = "Barry",
                        Status = CharacterStatus.ALIVE
                    };
                }

                MultiverseCharacter Jess()
                {
                    return new MultiverseCharacter()
                    {
                        Name = "Jess",
                        Status = CharacterStatus.ALIVE
                    };
                }

                var dimension = new Dimension();

                {
                    var uniqueId = false;
                    do
                    {
                        dimension.ID = GenerateDimensionID();
                        uniqueId = multiverse.Dimensions.Find(otherDimension => otherDimension.ID == dimension.ID) == null;
                    }
                    while (uniqueId == false);
                }

                dimension.Characters.Add(Rich());
                dimension.Characters.Add(Dorky());
                dimension.Characters.Add(Simmer());
                dimension.Characters.Add(Barry());
                dimension.Characters.Add(Jess());

                multiverse.Dimensions.Add(dimension);
            }

            return multiverse;
        }

        protected static string GenerateDimensionID()
        {
            var random = new Random();

            string[] letters = {"A", "B", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            string[] alternateLetters = { "Alpha", "Beta", "Gamma", "Delta", "Omega", "Epsilon", "Theta", "Phi", "Rho", "Pi", "Zeta"};

            var useAlternate = (random.Next(100) < 20);
            var lettersToUse = useAlternate ? alternateLetters : letters;

            var number = random.Next(10, 999);
            var letter = lettersToUse[random.Next(0, lettersToUse.Length)];

            var id = string.Format("{0}-{1}", letter, number);

            return id;
        }

    }
}