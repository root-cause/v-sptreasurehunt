using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using AreaLib;
using TreasureHunt.Enums;
using TreasureHunt.Managers;

namespace TreasureHunt.Classes
{
    public class SearchingChestStage : StageBase
    {
        #region Constants
        private const int HelpTextTime = 7000;
        private const int BlipFlashTime = 7000;

        private const string AnimDict = "anim@TreasureHunt@DoubleAction@Action";

        private readonly Control[] ControlsToDisable =
        {
            Control.CharacterWheel,
            Control.SelectCharacterMichael,
            Control.SelectCharacterFranklin,
            Control.SelectCharacterTrevor,
            Control.SelectCharacterMultiplayer
        };
        #endregion

        // Entities
        private Prop _chest = null;
        private Blip _blip = null;
        private Camera _cam = null;
        private List<Prop> _props = new List<Prop>();
        private int _soundId = -1;
        private int _sceneId = -1;

        // Areas
        private Sphere _audioArea = null;
        private Sphere _interactArea = null;

        // Misc.
        private int _helpTextHideAt = 0;
        private bool _receivedWeapon = false;

        #region Properties
        public override TreasureStage NextStage => TreasureStage.Found;
        #endregion

        #region Private methods
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
            if (_audioArea != null)
            {
                AreaLibrary.Untrack(_audioArea);

                _audioArea.PlayerEnter -= EnterAudioArea;
                _audioArea.PlayerLeave -= LeaveAudioArea;
                _audioArea = null;
            }

            if (_interactArea != null)
            {
                AreaLibrary.Untrack(_interactArea);

                _interactArea.PlayerLeave -= LeaveInteractionArea;
                _interactArea = null;
            }
        }

        private void DestroyScene()
        {
            if (_sceneId != -1)
            {
                Function.Call((Hash)0xCD9CC7E200A52A6F, _sceneId); // _DISPOSE_SYNCHRONIZED_SCENE
                _sceneId = -1;
            }
        }
        #endregion

        #region Public methods
        public override void Init(bool scriptStart)
        {
            Function.Call(Hash.REQUEST_ANIM_DICT, AnimDict);

            // Closed chest
            Location chestLocation = LocationManager.GetFinalChestLocation(SaveManager.ChestIndex);

            _chest = World.CreateProp("xm_prop_x17_chest_closed", chestLocation.Position, false, false);
            _chest.Heading = chestLocation.Heading;
            _chest.Rotation = chestLocation.Rotation;

            _blip = World.CreateBlip(chestLocation.Position);
            _blip.Sprite = BlipSprite.Treasure;
            _blip.Color = BlipColor.Yellow2;

            Function.Call(Hash.SET_BLIP_NAME_FROM_TEXT_FILE, _blip.Handle, "TREA1B_BLIP");
            Function.Call(Hash.SET_BLIP_PRIORITY, _blip.Handle, 11);

            // Corpses
            var corpseLocations = LocationManager.GetCorpseLocations(SaveManager.ChestIndex);
            _props.Add(Util.CreatePropNoOffset("xm_prop_x17_corpse_01", corpseLocations.Item1.Position, corpseLocations.Item1.Rotation, corpseLocations.Item1.Heading));
            _props.Add(Util.CreatePropNoOffset("xm_prop_x17_corpse_02", corpseLocations.Item2.Position, corpseLocations.Item2.Rotation, corpseLocations.Item2.Heading));

            // Shovel
            Location shovelLocation = LocationManager.GetShovelLocation(SaveManager.ChestIndex);

            Prop shovel = World.CreateProp("xm_prop_x17_shovel_01a", shovelLocation.Position, false, false);
            shovel.Heading = shovelLocation.Heading;
            shovel.Rotation = shovelLocation.Rotation;
            _props.Add(shovel);

            // Pistols
            var pistolLocations = LocationManager.GetPistolLocations(SaveManager.ChestIndex);
            _props.Add(Util.CreatePropNoOffset("w_pi_pistol", pistolLocations.Item1.Position, pistolLocations.Item1.Rotation, pistolLocations.Item1.Heading));
            _props.Add(Util.CreatePropNoOffset("w_pi_pistol", pistolLocations.Item2.Position, pistolLocations.Item2.Rotation, pistolLocations.Item2.Heading));

            // Areas
            _audioArea = new Sphere(chestLocation.Position, 75.0f);
            _audioArea.PlayerEnter += EnterAudioArea;
            _audioArea.PlayerLeave += LeaveAudioArea;

            _interactArea = new Sphere(_chest.GetOffsetInWorldCoords(new Vector3(0.0f, -0.85f, 0.0f)), 1.5f);
            _interactArea.PlayerLeave += LeaveInteractionArea;

            AreaLibrary.Track(_audioArea);
            AreaLibrary.Track(_interactArea);

            // Display help text & flash blip
            if (!scriptStart)
            {
                _helpTextHideAt = Game.GameTime + HelpTextTime;

                Function.Call(Hash.SET_BLIP_FLASHES, _blip.Handle, true);
                Function.Call(Hash.SET_BLIP_FLASH_TIMER, _blip.Handle, BlipFlashTime);
            }
        }

        public override bool Update()
        {
            if (_helpTextHideAt > 0 && Game.GameTime < _helpTextHideAt)
            {
                Util.DisplayHelpTextThisFrame("TREASURE_COLLEZ");
            }

            if (_interactArea != null && _interactArea.IsPlayerInside)
            {
                // Helptext is no longer needed
                if (_helpTextHideAt > 0)
                {
                    _helpTextHideAt = 0;
                }

                if (!Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, _sceneId))
                {
                    if (Game.Player.Character.IsInVehicle())
                    {
                        Util.DisplayHelpTextThisFrame("TREA2_HINTB");
                        return false;
                    }

                    Util.DisplayHelpTextThisFrame("TREA2_HINT");

                    if (Game.IsControlJustPressed(0, Control.Context))
                    {
                        Vector3 scenePos = _chest.Position + new Vector3(0.0f, 0.0f, 0.0077f);
                        Vector3 sceneRot = Function.Call<Vector3>(
                            Hash.GET_ANIM_INITIAL_OFFSET_ROTATION,
                            AnimDict, "Double_Action_PickUP",
                            _chest.Position.X, _chest.Position.Y, _chest.Position.Z,
                            _chest.Rotation.X, _chest.Rotation.Y, _chest.Rotation.Z,
                            0f, 2
                        );

                        _cam = Function.Call<Camera>(Hash.CREATE_CAM, "DEFAULT_ANIMATED_CAMERA", true);
                        CameraManager.SetCurrent(_cam);

                        _sceneId = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, scenePos.X, scenePos.Y, scenePos.Z, sceneRot.X, sceneRot.Y, sceneRot.Z, 2);

                        Function.Call(
                            Hash.TASK_SYNCHRONIZED_SCENE,
                            Game.Player.Character.Handle, _sceneId,
                            AnimDict, "Double_Action_PickUP",
                            1.5f, -1.5f, 262, 0
                        );

                        Function.Call(
                            Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM,
                            _chest.Handle, _sceneId,
                            "Double_Action_PickUP_CHEST", AnimDict,
                            1.5f, -1.5f, 4
                        );

                        Function.Call(
                            Hash.PLAY_SYNCHRONIZED_CAM_ANIM,
                            _cam.Handle, _sceneId,
                            "Double_Action_PickUp_CAM", AnimDict
                        );

                        _receivedWeapon = false;

                        Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
                        StopSound();

                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "item_found", "dlc_xm_fm_th_sounds", false);
                    }
                }
                else
                {
                    for (int i = 0; i < ControlsToDisable.Length; i++)
                    {
                        Game.DisableControlThisFrame(0, ControlsToDisable[i]);
                    }

                    Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);

                    // Scene phase stuff
                    float phase = Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, _sceneId);

                    // A hack to make the weapon appear in the treasure chest, couldn't get it to work as a prop
                    if (!_receivedWeapon && phase >= 0.3f)
                    {
                        _receivedWeapon = true;
                        Game.Player.Character.Weapons.Give(WeaponHash.DoubleActionRevolver, 9999, true, true);
                    }

                    if (_receivedWeapon && phase >= 1.0f)
                    {
                        Game.Player.Character.Task.ClearAll();

                        GameplayCamera.RelativeHeading = 0.0f;
                        GameplayCamera.RelativePitch = 0.0f;
                        CameraManager.Disable();

                        SaveManager.AddFlag(TreasureFlags.FoundFinalChest);
                        SaveManager.Save();

                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "clue_complete_shard", "dlc_xm_fm_th_sounds", false);

                        Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "DAR_UNLOCK");
                        Function.Call(Hash._0xC8F3AAF93D0600BF, "WEAPON_UNLOCK", 2, "DAR_UNLOCK", 1);
                        return true;
                    }
                }
            }

            return false;
        }

        public override void Destroy(bool scriptExit)
        {
            CameraManager.Disable();

            if (scriptExit)
            {
                if (_chest != null)
                {
                    _chest.Delete();
                    _chest = null;
                }
            }
            else
            {
                _chest?.MarkAsNoLongerNeeded();
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

            foreach (Prop prop in _props)
            {
                if (scriptExit)
                {
                    prop?.Delete();
                }
                else
                {
                    prop?.MarkAsNoLongerNeeded();
                }
            }

            _props.Clear();
            _props = null;

            DestroyScene();
            StopSound();

            Function.Call(Hash.REMOVE_ANIM_DICT, AnimDict);
        }
        #endregion

        #region Events
        private void EnterAudioArea(AreaBase area)
        {
            if (_soundId == -1)
            {
                _soundId = Util.PlaySoundFromCoord(_audioArea.Center, "dlc_xm_fm_th_sounds", "item_close_loop");
            }
        }

        private void LeaveAudioArea(AreaBase area)
        {
            StopSound();
        }

        private void LeaveInteractionArea(AreaBase area)
        {
            CameraManager.Disable();

            if (Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, _sceneId))
            {
                DestroyScene();
            }
        }
        #endregion
    }
}
