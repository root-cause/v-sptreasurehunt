using GTA.Math;

namespace TreasureHunt.Classes
{
    public class Location
    {
        public Vector3 Position { get; } = Vector3.Zero;
        public Vector3 Rotation { get; } = Vector3.Zero;
        public float Heading { get; } = 0.0f;

        public Location(Vector3 position, Vector3 rotation, float heading)
        {
            Position = position;
            Rotation = rotation;
            Heading = heading;
        }
    }
}
