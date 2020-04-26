using GTA;
using GTA.Math;
using TreasureHunt.Classes;

namespace TreasureHunt.Managers
{
    public static class CameraManager
    {
        #region Constants
        public const int MaxCameraLocations = 20;
        #endregion

        #region Properties
        public static bool IsActive { get; private set; } = false;
        public static Camera Current { get; private set; } = null;
        #endregion

        #region Methods
        public static CameraData GetNoteCamera(int index)
        {
            if (index < 0 || index >= MaxCameraLocations)
            {
                return null;
            }

            return _noteCameraLocations[index];
        }

        public static CameraData GetClueCamera(int index)
        {
            if (index < 0 || index >= _clueCameraLocations.Length)
            {
                return null;
            }

            return _clueCameraLocations[index];
        }

        public static void SetCurrent(Camera camera)
        {
            if (camera == null || !camera.Exists())
            {
                return;
            }

            IsActive = true;
            Current = camera;

            World.RenderingCamera = camera;
        }

        public static void Disable()
        {
            if (IsActive && Current != null && World.RenderingCamera == Current)
            {
                World.RenderingCamera = null;
            }

            IsActive = false;
            Current = null;
        }
        #endregion

        #region Note camera data
        private static readonly CameraData[] _noteCameraLocations =
        {
            new CameraData(new Vector3(3204.026f, 4731.787f, 193.5683f), new Vector3(0.2032f, 0f, 4.0436f), 50.0f),
            new CameraData(new Vector3(-1577.196f, 2100.982f, 69.2545f), new Vector3(2.1669f, 0f, 149.6662f), 50.0f),
            new CameraData(new Vector3(-280.9694f, 2845.835f, 54.1275f), new Vector3(-2.2991f, 0f, -46.0688f), 50.0f),
            new CameraData(new Vector3(-160.4209f, 6583.866f, 5.164f), new Vector3(9.0567f, 0f, -160.2472f), 50.0f),
            new CameraData(new Vector3(3927.559f, 4401.271f, 16.643f), new Vector3(5.6859f, 0f, 1.301f), 50.0f),
            new CameraData(new Vector3(501.0201f, 5603.358f, 798.2953f), new Vector3(-53.319f, 0f, 159.1515f), 50.0f),
            new CameraData(new Vector3(-424.6067f, 1595.158f, 356.6844f), new Vector3(2.0886f, 0f, -42.2077f), 50.0f),
            new CameraData(new Vector3(-598.7962f, 2100.106f, 130.6525f), new Vector3(7.0512f, 0f, -115.8027f), 50.0f),
            new CameraData(new Vector3(1013.162f, 2905.668f, 41.8756f), new Vector3(7.4812f, 0f, 85.9177f), 50.0f),
            new CameraData(new Vector3(2615.228f, 3665.456f, 102.5955f), new Vector3(-57.09f, 0f, 56.433f), 50.0f),
            new CameraData(new Vector3(-1815.325f, 1897.805f, 138.6159f), new Vector3(-76.2137f, 0f, 67.1257f), 50.0f),
            new CameraData(new Vector3(-1767.167f, -220.697f, 54.169f), new Vector3(-0.4374f, 0f, -45.5744f), 50.0f),
            new CameraData(new Vector3(2767.94f, -1577.425f, 2.7301f), new Vector3(-24.9484f, 0f, 138.8994f), 50.0f),
            new CameraData(new Vector3(-671.2609f, 4405.041f, 18.6267f), new Vector3(-2.3779f, 0f, 168.0288f), 50.0f),
            new CameraData(new Vector3(2918.614f, 806.4957f, 2.9927f), new Vector3(5.8804f, 0f, 174.5372f), 50.0f),
            new CameraData(new Vector3(1517.409f, 3915.866f, 31.845f), new Vector3(-46.4038f, 0f, -4.9927f), 50.0f),
            new CameraData(new Vector3(-751.9604f, 4322.178f, 142.2352f), new Vector3(-46.9575f, 0f, 0.6407f), 50.0f),
            new CameraData(new Vector3(-1641.882f, -1041.092f, 5.4026f), new Vector3(1.2561f, 0f, 155.7362f), 50.0f),
            new CameraData(new Vector3(-1078.354f, 9.1891f, 51.3932f), new Vector3(-2.6799f, 0f, -100.954f), 50.0f),
            new CameraData(new Vector3(1898.71f, 3479.844f, 45.9619f), new Vector3(-14.1365f, 0f, -119.5328f), 50.0f)
        };
        #endregion

        #region Clue camera data
        // Corpse -> shovel -> empty chest
        private static readonly CameraData[] _clueCameraLocations =
        {
            new CameraData(new Vector3(-1913.469f, 1388.91f, 219.8596f), new Vector3(-54.9011f, -0.4062f, 110.553f), 50.354f),
            new CameraData(new Vector3(1924.187f, 3986.594f, 32.1668f), new Vector3(-49.7593f, 0f, 119.1415f), 54.354f),
            new CameraData(new Vector3(1994.829f, 5079.042f, 42.0735f), new Vector3(-49.1641f, 0f, 28.3932f), 50.354f)
        };
        #endregion
    }
}
