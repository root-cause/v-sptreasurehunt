using System;
using GTA.Math;
using TreasureHunt.Classes;

namespace TreasureHunt.Managers
{
    public static class LocationManager
    {
        #region Constants
        public const int MaxLocations = 20;
        #endregion

        #region Methods
        public static Location GetNoteLocation(int index)
        {
            if (index < 0 || index >= MaxLocations)
            {
                return null;
            }

            return _noteLocations[index];
        }

        public static Location GetClueLocation(int index)
        {
            if (index < 0 || index >= _clueLocations.Length)
            {
                return null;
            }

            return _clueLocations[index];
        }

        public static Location GetFinalChestLocation(int index)
        {
            if (index < 0 || index >= MaxLocations)
            {
                return null;
            }

            return _finalChestLocations[index];
        }

        public static Tuple<Location, Location> GetCorpseLocations(int index)
        {
            if (index < 0 || index >= MaxLocations)
            {
                return null;
            }

            return Tuple.Create(_firstCorpseLocations[index], _secondCorpseLocations[index]);
        }

        public static Location GetShovelLocation(int index)
        {
            if (index < 0 || index >= MaxLocations)
            {
                return null;
            }

            return _shovelLocations[index];
        }

        public static Tuple<Location, Location> GetPistolLocations(int index)
        {
            if (index < 0 || index >= MaxLocations)
            {
                return null;
            }

            return Tuple.Create(_firstPistolLocations[index], _secondPistolLocations[index]);
        }

        public static Location GetRewardLocation(int index)
        {
            if (index < 0 || index >= _rewardLocations.Length)
            {
                return null;
            }

            return _rewardLocations[index];
        }
        #endregion

        #region Note locations
        private static readonly Location[] _noteLocations =
        {
            new Location(new Vector3(3204.029f, 4732.324f, 193.519f), new Vector3(81f, 0f, 8.6f), 8.6f),
            new Location(new Vector3(-1577.442f, 2100.604f, 69.268f), new Vector3(86.1001f, -43.0999f, -161.9f), 18.1f),
            new Location(new Vector3(-280.598f, 2846.174f, 54.105f), new Vector3(85.4f, -15.4f, -16.1f), 343.9f),
            new Location(new Vector3(-160.261f, 6583.436f, 5.213f), new Vector3(85.7001f, -150.1f, 6.7f), 6.7f),
            new Location(new Vector3(3927.534f, 4401.725f, 16.681f), new Vector3(88.7704f, -0.0579f, 18.121f), 198.121f),
            new Location(new Vector3(500.93f, 5603.104f, 797.988f), new Vector3(13.7999f, 0.2f, 175.225f), 175.225f),
            new Location(new Vector3(-424.332f, 1595.448f, 356.688f), new Vector3(89.2956f, 1.6f, -33.0419f), 146.9581f),
            new Location(new Vector3(-598.221f, 2099.779f, 130.767f), new Vector3(88.5025f, 93.7551f, 150.676f), 330.676f),
            new Location(new Vector3(1012.713f, 2905.666f, 41.924f), new Vector3(87.9002f, 0.0624f, 97.225f), 277.225f),
            new Location(new Vector3(2615.015f, 3665.585f, 102.223f), new Vector3(12.8f, 0f, 55.824f), 55.824f),
            new Location(new Vector3(-1815.439f, 1897.805f, 138.1239f), new Vector3(-4.1003f, 10.4f, 68.6998f), 68.6998f),
            new Location(new Vector3(-1766.872f, -220.422f, 54.165f), new Vector3(88.5f, -22.9f, -24.042f), 335.958f),
            new Location(new Vector3(2767.702f, -1577.713f, 2.54f), new Vector3(62.3881f, -11.4852f, 150.795f), 330.795f),
            new Location(new Vector3(-671.333f, 4404.611f, 18.628f), new Vector3(88.6471f, 170.8059f, 3.9f), 183.9f),
            new Location(new Vector3(2918.568f, 806.0303f, 3.0282f), new Vector3(86.9f, 2f, -176.1f), 183.9f),
            new Location(new Vector3(1517.439f, 3916.156f, 31.555f), new Vector3(31.8049f, -8.0073f, 3.9f), 183.9f),
            new Location(new Vector3(-751.984f, 4322.46f, 141.926f), new Vector3(21.4238f, 4.7061f, 3.9f), 183.9f),
            new Location(new Vector3(-1642.111f, -1041.605f, 5.436f), new Vector3(89.524f, 0f, 154.4997f), 154.4997f),
            new Location(new Vector3(-1077.839f, 9.078f, 51.375f), new Vector3(89.421f, -4.584f, -98.101f), 81.899f),
            new Location(new Vector3(1899.099f, 3479.637f, 45.849f), new Vector3(67.273f, -10.9299f, -118.301f), 61.699f)
        };
        #endregion

        #region Clue locations
        // Corpse -> shovel -> empty chest
        private static readonly Location[] _clueLocations =
        {
            new Location(new Vector3(-1914.033f, 1388.754f, 218.3793f), new Vector3(16.719f, -1.1001f, 108.913f), 108.913f),
            new Location(new Vector3(1923.548f, 3985.898f, 31.257f), new Vector3(2.062f, -5.1001f, 164.156f), 164.156f),
            new Location(new Vector3(1994.701f, 5079.219f, 41.663f), new Vector3(0.4f, 5f, 5.872f), 1.97f),
        };
        #endregion

        #region Final stage locations
        private static readonly Location[] _finalChestLocations =
        {
            new Location(new Vector3(-1602.755f, 4498.812f, 18.201f), new Vector3(-7.5571f, -7.8f, 24.971f), 24.971f),
            new Location(new Vector3(2762.659f, 4237.131f, 47.487f), new Vector3(0f, 0f, -101.6f), 0f),
            new Location(new Vector3(2994.539f, 6368.674f, 0.97f), new Vector3(-0.4f, -4.2f, -124.002f), 235.998f),
            new Location(new Vector3(2690.052f, 5325.579f, 79.104f), new Vector3(6.4f, 2.8f, 59.556f), 59.556f),
            new Location(new Vector3(-1537.479f, 712.819f, 203.054f), new Vector3(3.076f, 2f, 178.698f), 178.698f),
            new Location(new Vector3(-123.319f, 3098.984f, 22.76f), new Vector3(-3.5f, -1.9f, 28.597f), 28.597f),
            new Location(new Vector3(1760.075f, 4831.197f, 39.395f), new Vector3(-11.8f, -2.6f, -36.2f), 323.8f),
            new Location(new Vector3(-919.149f, 6094.585f, 6.104f), new Vector3(-0.6f, 4.3f, -81.901f), 278.099f),
            new Location(new Vector3(67.883f, 7085.546f, 0.925f), new Vector3(-2.2f, -1.1f, 26.097f), 26.097f),
            new Location(new Vector3(1407.51f, 3272.148f, 38.0382f), new Vector3(-3.143f, 1.2f, 110.192f), 110.192f),
            new Location(new Vector3(310.051f, 4370.523f, 51.035f), new Vector3(8.6f, 6.3f, -51.647f), 308.353f),
            new Location(new Vector3(-972.75f, 872.791f, 171.396f), new Vector3(-0.5f, 0f, 150.952f), 150.952f),
            new Location(new Vector3(1436.604f, -2743.677f, 1.848f), new Vector3(-1.9f, 4.1f, -117.8f), 242.2f),
            new Location(new Vector3(1533.905f, 2336.884f, 69.685f), new Vector3(2.5f, -0.5f, -43f), 317f),
            new Location(new Vector3(-2187.804f, 143.174f, 168.261f), new Vector3(-3.8001f, -1.4f, 84.831f), 84.831f),
            new Location(new Vector3(3049.21f, -281.784f, 9.62f), new Vector3(-4.8f, 1.4f, -107.372f), 252.628f),
            new Location(new Vector3(3021.354f, 3992.088f, 66.265f), new Vector3(-2.4001f, 0.4f, 112.951f), 112.951f),
            new Location(new Vector3(2006.778f, 2021.514f, 62.948f), new Vector3(1.8f, -0.5f, -34.001f), 325.999f),
            new Location(new Vector3(-1799.153f, 2516.403f, 1.1924f), new Vector3(4.8f, 0f, -136.6031f), 223.3969f),
            new Location(new Vector3(1291.624f, -1118.016f, 38.727f), new Vector3(5.5f, 0f, 149.616f), 149.616f),
        };

        private static readonly Location[] _firstCorpseLocations =
        {
            new Location(new Vector3(-1603.881f, 4495.683f, 18.218f), new Vector3(-9.7f, 0f, 118.172f), 118.172f),
            new Location(new Vector3(2762.452f, 4239.723f, 47.412f), new Vector3(-2.73f, -2.136f, -28.402f), 331.598f),
            new Location(new Vector3(2994.587f, 6370.531f, 0.834f), new Vector3(-3.888f, 0f, -46.502f), 313.498f),
            new Location(new Vector3(2691.818f, 5322.524f, 78.783f), new Vector3(0f, -10.026f, -169.602f), 190.398f),
            new Location(new Vector3(-1536.876f, 715.981f, 202.816f), new Vector3(-7.4f, 0f, -53.403f), 306.597f),
            new Location(new Vector3(-121.388f, 3098.395f, 22.757f), new Vector3(0f, 0f, -48.604f), 311.396f),
            new Location(new Vector3(1758.229f, 4832.102f, 39.275f), new Vector3(-9.9f, 8.5f, 33.199f), 33.199f),
            new Location(new Vector3(-921.3657f, 6091.964f, 5.946f), new Vector3(-4.2f, 0f, 164.6351f), 164.6351f),
            new Location(new Vector3(70.556f, 7085.731f, 0.939f), new Vector3(0f, 0f, -67.904f), 292.096f),
            new Location(new Vector3(1407.289f, 3274.18f, 37.984f), new Vector3(-3.109f, -4.3071f, 43.71f), 43.71f),
            new Location(new Vector3(307.353f, 4370.244f, 50.701f), new Vector3(-1.022f, -9.2981f, 45.352f), 45.352f),
            new Location(new Vector3(-970.564f, 872.705f, 171.756f), new Vector3(12.8f, 9.886f, -149.4f), 210.6f),
            new Location(new Vector3(1437.094f, -2741.581f, 1.981f), new Vector3(5f, 2.18f, -38.2f), 321.8f),
            new Location(new Vector3(1533.107f, 2333.873f, 69.487f), new Vector3(0f, 5.674f, -167.402f), 192.598f),
            new Location(new Vector3(-2187.76f, 141.008f, 168.179f), new Vector3(-5f, 1.7f, 138.628f), 138.628f),
            new Location(new Vector3(3049.227f, -279.45f, 9.742f), new Vector3(-3.1f, 7.1f, -41.172f), 318.828f),
            new Location(new Vector3(3023.637f, 3990.868f, 66.2941f), new Vector3(0f, 0f, -155.6018f), 0f),
            new Location(new Vector3(2007.148f, 2018.956f, 63.042f), new Vector3(10f, 0f, -139.601f), 220.399f),
            new Location(new Vector3(-1798.481f, 2518.949f, 1.0165f), new Vector3(0f, -5.9002f, -46.8038f), 313.1962f),
            new Location(new Vector3(1293.918f, -1115.553f, 37.812f), new Vector3(-8.878f, -12.914f, -82.602f), 277.398f),
        };

        private static readonly Location[] _secondCorpseLocations =
        {
            new Location(new Vector3(-1600.034f, 4497.527f, 18.803f), new Vector3(6f, -4.2001f, -65.306f), 294.694f),
            new Location(new Vector3(2760.595f, 4234.673f, 47.482f), new Vector3(0.7f, 1.63f, 148.1f), 148.1f),
            new Location(new Vector3(2991.698f, 6368.075f, 1.107f), new Vector3(4.401f, 4.3f, 130.398f), 130.398f),
            new Location(new Vector3(2691.153f, 5327.774f, 79.0767f), new Vector3(-0.2f, 21.85f, 6.81f), 6.81f),
            new Location(new Vector3(-1539.629f, 712.7659f, 202.943f), new Vector3(-0.6f, 3.129f, 160.997f), 160.997f),
            new Location(new Vector3(-124.9162f, 3095.986f, 22.652f), new Vector3(-6.425f, 3.335f, 105.896f), 105.896f),
            new Location(new Vector3(1761.359f, 4828.671f, 39.8742f), new Vector3(0f, 3.307f, -128.3481f), 231.6519f),
            new Location(new Vector3(-920.2819f, 6096.609f, 6.267f), new Vector3(3.2f, 6.2f, -18.2939f), 341.7061f),
            new Location(new Vector3(65.728f, 7084.014f, 0.911f), new Vector3(0.4f, 4.3f, 99.224f), 99.224f),
            new Location(new Vector3(1411.464f, 3272.167f, 38.195f), new Vector3(1.055f, 6.6f, -98.943f), 261.057f),
            new Location(new Vector3(310.9685f, 4367.286f, 50.769f), new Vector3(1.355f, 8.366f, -123.013f), 236.987f),
            new Location(new Vector3(-973.6434f, 875.7452f, 172.1553f), new Vector3(20.3f, -13.7961f, 59.4f), 59.4f),
            new Location(new Vector3(1435.357f, -2745.606f, 1.865f), new Vector3(7.117f, 4.7f, -178.3f), 181.7f),
            new Location(new Vector3(1531.791f, 2337.719f, 69.603f), new Vector3(2f, 0f, 23.999f), 23.999f),
            new Location(new Vector3(-2184.945f, 144.7612f, 168.674f), new Vector3(8.594f, 2.068f, -47.806f), 312.194f),
            new Location(new Vector3(3047.516f, -284.488f, 9.718f), new Vector3(7.737f, -16.24f, 172.827f), 172.827f),
            new Location(new Vector3(3020.615f, 3994.894f, 66.188f), new Vector3(0.9f, 4.173f, 42.084f), 42.084f),
            new Location(new Vector3(2003.224f, 2020.946f, 62.691f), new Vector3(-2.5421f, -2.2131f, 63.347f), 63.347f),
            new Location(new Vector3(-1801.99f, 2515.435f, 1.138f), new Vector3(4.963f, 9.3f, 137.6939f), 137.6939f),
            new Location(new Vector3(1290.756f, -1114.798f, 38.2307f), new Vector3(5.492f, 29.756f, 57.717f), 57.717f),
        };

        private static readonly Location[] _shovelLocations =
        {
            new Location(new Vector3(-1603.627f, 4498.334f, 18.031f), new Vector3(0.1f, 0.032f, 67.461f), 67.461f),
            new Location(new Vector3(2762.823f, 4237.938f, 47.326f), new Vector3(8.029f, 0.331f, -65.954f), 294.046f),
            new Location(new Vector3(2993.927f, 6367.937f, 0.886f), new Vector3(0f, 0f, -174.402f), 185.598f),
            new Location(new Vector3(2690.38f, 5326.472f, 78.994f), new Vector3(-0.001f, 4.886f, 18.619f), 198.619f),
            new Location(new Vector3(-1536.766f, 712.3657f, 202.809f), new Vector3(0f, 0f, -143.902f), 0f),
            new Location(new Vector3(-124.586f, 3099.214f, 22.477f), new Vector3(0f, 0f, 74.797f), 74.797f),
            new Location(new Vector3(1761.386f, 4831.27f, 39.235f), new Vector3(0f, 0f, -75.8f), 284.2f),
            new Location(new Vector3(-918.815f, 6093.755f, 5.916f), new Vector3(0f, 0f, -105.901f), 254.099f),
            new Location(new Vector3(67.151f, 7085.35f, 0.745f), new Vector3(0f, 0f, 52.497f), 52.497f),
            new Location(new Vector3(1406.92f, 3272.709f, 37.8519f), new Vector3(0f, 0f, 79.898f), 0f),
            new Location(new Vector3(309.516f, 4371.15f, 50.965f), new Vector3(0f, 0f, -17.447f), 342.553f),
            new Location(new Vector3(-973.386f, 873.212f, 171.229f), new Vector3(0f, 0f, 111.2f), 111.2f),
            new Location(new Vector3(1437.111f, -2743.1f, 1.746f), new Vector3(0f, 0f, -75.8f), 284.2f),
            new Location(new Vector3(1534.695f, 2336.525f, 69.544f), new Vector3(0f, 0f, -64.801f), 295.199f),
            new Location(new Vector3(-2188.05f, 142.464f, 168.067f), new Vector3(0f, 0f, 106.629f), 106.629f),
            new Location(new Vector3(3049.147f, -282.568f, 9.409f), new Vector3(0f, 0f, -133.372f), 226.628f),
            new Location(new Vector3(3020.398f, 3993.052f, 66.012f), new Vector3(0f, 0f, 70.798f), 0f),
            new Location(new Vector3(2007.649f, 2021.071f, 62.809f), new Vector3(0f, 0f, -53.801f), 306.199f),
            new Location(new Vector3(-1798.297f, 2516.755f, 1.0686f), new Vector3(0f, 0f, -101.2033f), 258.7968f),
            new Location(new Vector3(1293.041f, -1118.008f, 38.5067f), new Vector3(0f, 0f, -129.1842f), 230.8158f),
        };

        private static readonly Location[] _firstPistolLocations =
        {
            new Location(new Vector3(-1604.105f, 4496.371f, 18.2583f), new Vector3(-83.1f, 178.9999f, -87.6999f), 92.3001f),
            new Location(new Vector3(2763.044f, 4239.264f, 47.4964f), new Vector3(-89.9031f, -180f, 91.2999f), 271.2999f),
            new Location(new Vector3(2992.352f, 6367.888f, 1.1114f), new Vector3(89.193f, -180f, -156.386f), 23.614f),
            new Location(new Vector3(2690.929f, 5322.497f, 78.9753f), new Vector3(-78.5711f, -1.092f, 141.258f), 141.258f),
            new Location(new Vector3(-1536.488f, 715.2114f, 202.8942f), new Vector3(-89.5369f, -0.727f, -108.3411f), 251.6589f),
            new Location(new Vector3(-120.9035f, 3097.57f, 22.8124f), new Vector3(-89.323f, 0f, -91.376f), 268.624f),
            new Location(new Vector3(1761.343f, 4829.36f, 39.866f), new Vector3(81.425f, 130.49f, -0.171f), 179.829f),
            new Location(new Vector3(-922.1017f, 6092.313f, 6.0183f), new Vector3(-89.6942f, 0f, 105.97f), 105.97f),
            new Location(new Vector3(66.1749f, 7083.509f, 0.9451f), new Vector3(89.8781f, -180f, 161.2889f), 341.2889f),
            new Location(new Vector3(1407.987f, 3274.676f, 38.1074f), new Vector3(-84.8051f, 0f, -3.858f), 356.1421f),
            new Location(new Vector3(310.8712f, 4367.925f, 50.8256f), new Vector3(81.438f, 0f, 136.63f), 136.63f),
            new Location(new Vector3(-973.7573f, 874.9136f, 171.8822f), new Vector3(81.26f, 0f, -34.0359f), 325.9641f),
            new Location(new Vector3(1435.967f, -2744.787f, 1.7869f), new Vector3(87.3642f, -180f, -93.6649f), 86.3351f),
            new Location(new Vector3(1531.553f, 2337.099f, 69.5891f), new Vector3(84.8201f, -180f, 107.1904f), 287.1904f),
            new Location(new Vector3(-2188.216f, 141.5933f, 168.2223f), new Vector3(-89.7154f, 0f, 79.5793f), 79.5793f),
            new Location(new Vector3(3048.266f, -283.8977f, 9.581f), new Vector3(89.9604f, 92.0993f, 0f), 92.0993f),
            new Location(new Vector3(3022.801f, 3990.652f, 66.3215f), new Vector3(-86.3589f, 180f, -26.86f), 153.14f),
            new Location(new Vector3(2006.444f, 2018.607f, 63.048f), new Vector3(-84.7038f, -180f, -33.017f), 146.983f),
            new Location(new Vector3(-1801.288f, 2515.463f, 1.1814f), new Vector3(86.8761f, 0f, 31.394f), 31.394f),
            new Location(new Vector3(1293.979f, -1115.963f, 37.9089f), new Vector3(-79.0579f, 0f, -166.846f), 193.154f)
        };

        private static readonly Location[] _secondPistolLocations =
        {
            new Location(new Vector3(-1600.413f, 4498.021f, 18.7202f), new Vector3(77.9f, 69.3f, 93.8001f), 93.8001f),
            new Location(new Vector3(2761.466f, 4234.847f, 47.4949f), new Vector3(84.9001f, 15.2f, 1.1f), 1.1f),
            new Location(new Vector3(2995.265f, 6369.949f, 0.8826f), new Vector3(-86.5114f, 3.53f, -86.8701f), 93.1299f),
            new Location(new Vector3(2690.717f, 5327.306f, 79.19f), new Vector3(79.825f, 1.819f, -104.302f), 255.698f),
            new Location(new Vector3(-1538.966f, 713.006f, 202.9678f), new Vector3(89.7753f, -180f, -123.4861f), 56.5139f),
            new Location(new Vector3(-124.3272f, 3095.602f, 22.7146f), new Vector3(89.4486f, 180f, 168.294f), 348.294f),
            new Location(new Vector3(1759.065f, 4832.398f, 39.2519f), new Vector3(-81.6559f, -169.692f, 179.829f), 359.829f),
            new Location(new Vector3(-920.8973f, 6096.31f, 6.2749f), new Vector3(90f, -120.961f, 0f), 239.039f),
            new Location(new Vector3(70.711f, 7084.906f, 0.9933f), new Vector3(-89.9802f, 131.75f, 0f), 228.25f),
            new Location(new Vector3(1411.12f, 3272.707f, 38.2368f), new Vector3(87.2011f, 0f, 161.2889f), 161.2889f),
            new Location(new Vector3(308.1051f, 4370.81f, 50.875f), new Vector3(-69.4829f, 0f, -3.858f), 356.1421f),
            new Location(new Vector3(-971.7089f, 872.5903f, 171.4207f), new Vector3(-86.804f, 180f, -34.0359f), 145.9641f),
            new Location(new Vector3(1437.587f, -2742.167f, 2.0156f), new Vector3(-62.3724f, 0f, -126.7849f), 233.2151f),
            new Location(new Vector3(1532.133f, 2333.83f, 69.4566f), new Vector3(-87.4966f, 180f, -36.7786f), 143.2214f),
            new Location(new Vector3(-2185.42f, 144.9704f, 168.6582f), new Vector3(67.2471f, -180f, 44.6933f), 224.6933f),
            new Location(new Vector3(3049.718f, -280.5405f, 9.6845f), new Vector3(-81.1849f, -180f, 92.0993f), 92.0993f),
            new Location(new Vector3(3020.531f, 3994.178f, 66.2253f), new Vector3(88.8816f, -180f, 109.317f), 289.317f),
            new Location(new Vector3(2003.407f, 2020.228f, 62.6822f), new Vector3(83.763f, -180f, 132.273f), 312.273f),
            new Location(new Vector3(-1798.077f, 2518.217f, 1.1636f), new Vector3(-80.6252f, 0f, -108.8031f), 251.1969f),
            new Location(new Vector3(1290.987f, -1115.424f, 38.4153f), new Vector3(70.156f, 0f, -44.28f), 315.72f)
        };
        #endregion

        #region Reward locations
        private static readonly Location[] _rewardLocations =
        {
            new Location(new Vector3(-813.3670f, 182.4114f, 76.3207f), new Vector3(-90.0f, -22.0f, 0.0f), 0.0f),
            new Location(new Vector3(-9.2935f, -1434.048f, 30.8685f), new Vector3(-90.0f, 51.0f, 0.0f), 0.0f),
            new Location(new Vector3(1970.2178f, 3814.4692f, 34.0090f), new Vector3(-90.0f, 138.0f, 0.0f), 0.0f),
            new Location(new Vector3(96.5158f, -1294.379f, 29.0887f), new Vector3(-90.0f, 60.0f, 0.0f), 0.0f),
            new Location(new Vector3(0.4309f, 527.4254f, 170.5271f), new Vector3(-90.0f, -24.0f, 0.0f), 0.0f)
        };
        #endregion
    }
}
