using GTA;
using GTA.Math;
using GTA.Native;
using AreaLib;
using TreasureHunt.Enums;
using TreasureHunt.Managers;

namespace TreasureHunt.Classes
{
    public class SearchingNoteStage : StageBase
    {
        #region Constants
        private const int BlipFlashTime = 7000;
        #endregion

        // Entities
        private Prop _note = null;
        private Blip _blip = null;
        private Camera _cam = null;
        private int _soundId = -1;

        // Areas
        private Sphere _revealArea = null;
        private Sphere _interactArea = null;

        // Misc.
        private Vector3 _camPos = Vector3.Zero;
        private Vector3 _camRot = Vector3.Zero;
        private float _camFov = 50.0f;

        #region Properties
        public override TreasureStage NextStage => TreasureStage.SearchingClues;
        #endregion

        #region Private methods
        private Blip CreateBlip(Vector3 position)
        {
            Blip blip = World.CreateBlip(position);
            blip.Sprite = BlipSprite.StrangersAndFreaks;
            blip.Color = BlipColor.Yellow2;
            blip.Scale = 0.8f;

            Function.Call(Hash.SET_BLIP_NAME_FROM_TEXT_FILE, blip.Handle, "TREBLIP");
            Function.Call(Hash.SET_BLIP_PRIORITY, blip.Handle, 11);
            return blip;
        }

        private void StopSound()
        {
            if (_soundId != -1)
            {
                Audio.StopSound(_soundId);
                Audio.ReleaseSound(_soundId);
            }

            _soundId = -1;
        }

        private void DestroyAreas()
        {
            if (_revealArea != null)
            {
                AreaLibrary.Untrack(_revealArea);

                _revealArea.PlayerEnter -= EnterRevealArea;
                _revealArea.PlayerLeave -= LeaveRevealArea;
                _revealArea = null;
            }

            if (_interactArea != null)
            {
                AreaLibrary.Untrack(_interactArea);

                _interactArea.PlayerLeave -= LeaveInteractionArea;
                _interactArea = null;
            }
        }
        #endregion

        #region Public methods
        public override void Init(bool scriptStart)
        {
            // Entities
            Location location = LocationManager.GetNoteLocation(SaveManager.NoteIndex);
            CameraData cameraData = CameraManager.GetNoteCamera(SaveManager.NoteIndex);

            _note = World.CreateProp("xm_prop_x17_note_paper_01a", location.Position, false, false);
            _note.Heading = location.Heading;
            _note.Rotation = location.Rotation;

            if (SaveManager.HasFlag(TreasureFlags.RevealedNote))
            {
                _blip = CreateBlip(location.Position);
            }

            _camPos = cameraData.Position;
            _camRot = cameraData.Rotation;
            _camFov = cameraData.FOV;

            // Areas
            _revealArea = new Sphere(location.Position, 75.0f);
            _revealArea.PlayerEnter += EnterRevealArea;
            _revealArea.PlayerLeave += LeaveRevealArea;

            _interactArea = new Sphere(cameraData.Position - new Vector3(0.0f, 0.0f, 1.0f), 1.5f);
            _interactArea.PlayerLeave += LeaveInteractionArea;

            AreaLibrary.Track(_revealArea);
            AreaLibrary.Track(_interactArea);
        }

        public override bool Update()
        {
            if (_interactArea != null && _interactArea.IsPlayerInside)
            {
                if (Game.Player.Character.IsInVehicle())
                {
                    Util.DisplayHelpTextThisFrame("TREA1_HINTB");
                    return false;
                }

                Util.DisplayHelpTextThisFrame(CameraManager.IsActive ? "TREA1_EXIT" : "TREA1_HINT");

                if (Game.IsControlJustPressed(0, Control.Context))
                {
                    if (CameraManager.IsActive)
                    {
                        CameraManager.Disable();

                        SaveManager.AddFlag(TreasureFlags.FoundNote);
                        SaveManager.Save();

                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "clue_complete_shard", "dlc_xm_fm_th_sounds", false);
                        return true;
                    }
                    else
                    {
                        _cam = World.CreateCamera(_camPos, _camRot, _camFov);
                        _cam.Shake(CameraShake.Hand, 0.19f);

                        CameraManager.SetCurrent(_cam);
                        StopSound();

                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "item_found", "dlc_xm_fm_th_sounds", false);
                    }
                }

                if (CameraManager.IsActive)
                {
                    Game.DisableAllControlsThisFrame(0);
                    Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
                }
            }

            return false;
        }

        public override void Destroy(bool scriptExit)
        {
            CameraManager.Disable();

            if (scriptExit)
            {
                if (_note != null)
                {
                    _note.Delete();
                    _note = null;
                }
            }
            else
            {
                _note?.MarkAsNoLongerNeeded();
                DestroyAreas();
            }

            if (_blip != null)
            {
                _blip.Remove();
                _blip = null;
            }

            if (_cam != null)
            {
                _cam.Destroy();
                _cam = null;
            }

            StopSound();
        }
        #endregion

        #region Events
        private void EnterRevealArea(AreaBase area)
        {
            if (_blip == null)
            {
                _blip = CreateBlip(_revealArea.Center);

                Function.Call(Hash.SET_BLIP_FLASHES, _blip.Handle, true);
                Function.Call(Hash.SET_BLIP_FLASH_TIMER, _blip.Handle, BlipFlashTime);
                Function.Call(Hash.FLASH_MINIMAP_DISPLAY);

                SaveManager.AddFlag(TreasureFlags.RevealedNote);
                SaveManager.Save();
            }

            if (_soundId == -1)
            {
                _soundId = Util.PlaySoundFromCoord(_revealArea.Center, "dlc_xm_fm_th_sounds", "item_close_loop");
            }
        }

        private void LeaveRevealArea(AreaBase area)
        {
            StopSound();
        }

        private void LeaveInteractionArea(AreaBase area)
        {
            CameraManager.Disable();
        }
        #endregion
    }
}
