using TreasureHunt.Enums;

namespace TreasureHunt.Classes
{
    public abstract class StageBase
    {
        public abstract TreasureStage NextStage { get; }

        public abstract void Init(bool scriptStart);
        public abstract bool Update();
        public abstract void Destroy(bool scriptExit);
    }
}
