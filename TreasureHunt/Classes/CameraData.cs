using GTA.Math;

namespace TreasureHunt.Classes
{
    public class CameraData
    {
        public Vector3 Position { get; } = Vector3.Zero;
        public Vector3 Rotation { get; } = Vector3.Zero;
        public float FOV { get; } = 50.0f;

        public CameraData(Vector3 position, Vector3 rotation, float fov)
        {
            Position = position;
            Rotation = rotation;
            FOV = fov;
        }
    }
}
