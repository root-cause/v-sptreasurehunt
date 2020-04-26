using GTA;
using GTA.Math;
using GTA.Native;

namespace TreasureHunt
{
    public static class Util
    {
        public static Prop CreatePropNoOffset(Model model, Vector3 position, Vector3 rotation, float heading)
        {
            if (!model.Request(1000))
            {
                return null;
            }

            int handle = Function.Call<int>(Hash.CREATE_OBJECT_NO_OFFSET, model.Hash, position.X, position.Y, position.Z, false, false, false);
            Function.Call(Hash.SET_ENTITY_HEADING, handle, heading);
            Function.Call(Hash.SET_ENTITY_ROTATION, handle, rotation.X, rotation.Y, rotation.Z, 2, 1);

            return new Prop(handle);
        }

        public static int PlaySoundFromCoord(Vector3 position, string audioRef, string audioName)
        {
            int soundId = Function.Call<int>(Hash.GET_SOUND_ID);
            Function.Call(Hash.PLAY_SOUND_FROM_COORD, soundId, audioName, position.X, position.Y, position.Z, audioRef, 0, 0, 0);
            return soundId;
        }

        public static void DisplayHelpTextThisFrame(string gxtEntry)
        {
            Function.Call(Hash._SET_TEXT_COMPONENT_FORMAT, gxtEntry);
            Function.Call(Hash._DISPLAY_HELP_TEXT_FROM_STRING_LABEL, 0, 0, 1, -1);
        }
    }
}
