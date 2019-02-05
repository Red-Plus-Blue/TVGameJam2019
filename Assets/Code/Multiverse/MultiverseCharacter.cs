

namespace Game.MultiverseData
{
    public enum CharacterStatus
    {
        ALIVE,
        DEAD,
        MISSING,
        TERMINATED,
        ASSASINATED,
        GONE_ROGUE,
        KIA
    }

    public class MultiverseCharacter
    {
        public string Name = "";
        public CharacterStatus Status = CharacterStatus.ALIVE;
    }
}