using GTA;
using GTA.Native;
using TreasureHunt.Enums;
using TreasureHunt.Managers;

namespace TreasureHunt.Classes
{
    public class FoundStage : StageBase
    {
        #region Constants
        private const int MaxRewards = 5;
        private readonly int PickupHash = Game.GenerateHash("PICKUP_WEAPON_DOUBLEACTION");
        #endregion

        private readonly int[] _pickups = new int[MaxRewards];

        #region Properties
        public override TreasureStage NextStage => TreasureStage.None;
        #endregion

        #region Methods
        public override void Init(bool scriptStart)
        {
            for (int i = 0; i < MaxRewards; i++)
            {
                Location location = LocationManager.GetRewardLocation(i);

                _pickups[i] = Function.Call<int>(
                    Hash.CREATE_PICKUP_ROTATE,
                    PickupHash,
                    location.Position.X, location.Position.Y, location.Position.Z,
                    location.Rotation.X, location.Rotation.Y, location.Rotation.Z,
                    4 /* respawns pickup 1 min after collection */, 9999, 2, true, 0
                );
            }
        }

        public override bool Update()
        {
            return false;
        }

        public override void Destroy(bool scriptExit)
        {
            if (scriptExit)
            {
                for (int i = 0; i < MaxRewards; i++)
                {
                    if (_pickups[i] != 0)
                    {
                        Function.Call(Hash.REMOVE_PICKUP, _pickups[i]);
                        _pickups[i] = 0;
                    }
                }
            }
        }
        #endregion
    }
}
