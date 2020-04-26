using GTA;
using GTA.Math;
using GTA.Native;
using AreaLib;
using TreasureHunt.Enums;
using TreasureHunt.Managers;

namespace TreasureHunt.Classes
{
    public class SearchingCluesStage : StageBase
    {
        #region Constants
        private const int MaxClues = 3;
        private const float AreaDistance = 10.0f;
        private const float AreaRange = 50.0f;
        private const int HelpTextTime = 7000;
        private const int BlipFlashTime = 7000;

        private readonly string[] ClueModels = { "xm_prop_x17_corpse_03", "xm_prop_x17_shovel_01b", "xm_prop_x17_chest_open" };
        private readonly TreasureFlags[] ClueFlags = { TreasureFlags.FoundCorpse, TreasureFlags.FoundShovel, TreasureFlags.FoundEmptyChest };
        #endregion

        // Entities
        private readonly Prop[] _props = new Prop[MaxClues];
        private readonly Blip[] _areaBlips = new Blip[MaxClues];
        private readonly Blip[] _areaClueBlips = new Blip[MaxClues];
        private Camera _cam = null;
        private int _soundId = -1;

        // Areas
        private readonly Sphere[] _audioAreas = new Sphere[MaxClues];
        private readonly Sphere[] _clueAreas = new Sphere[MaxClues];
        private readonly Sphere[] _interactionAreas = new Sphere[MaxClues];

        // Misc.
        private int _helpTextHideAt = 0;
        private bool _helpTextIsClue = false;
        private TreasureFlags _interactionFlag = TreasureFlags.None;

        #region Properties
        public override TreasureStage NextStage => TreasureStage.SearchingChest;
        #endregion

        #region Private methods
        private int FlagToIndex(TreasureFlags flag)
        {
            switch (flag)
            {
                case TreasureFlags.FoundCorpse:
                    return 0;

                case TreasureFlags.FoundShovel:
                    return 1;

                case TreasureFlags.FoundEmptyChest:
                    return 2;

                default:
                    return -1;
            }
        }

        private int GetNumCluesFound()
        {
            int num = 0;

            if (SaveManager.HasFlag(TreasureFlags.FoundCorpse)) num++;
            if (SaveManager.HasFlag(TreasureFlags.FoundShovel)) num++;
            if (SaveManager.HasFlag(TreasureFlags.FoundEmptyChest)) num++;

            return num;
        }

        private Blip CreateAreaBlip(Vector3 center, float radius)
        {
            Blip blip = World.CreateBlip(center, radius);
            blip.Color = BlipColor.Yellow3;
            blip.Alpha = 80;

            Function.Call(Hash.SET_BLIP_DISPLAY, blip.Handle, 4);
            return blip;
        }

        private Blip CreateClueBlip(Vector3 position)
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

        private void DestroyAreas(int index)
        {
            if (_audioAreas[index] != null)
            {
                AreaLibrary.Untrack(_audioAreas[index]);

                _audioAreas[index].PlayerEnter -= EnterAudioClueArea;
                _audioAreas[index].PlayerLeave -= LeaveAudioClueArea;
                _audioAreas[index] = null;
            }

            if (_clueAreas[index] != null)
            {
                AreaLibrary.Untrack(_clueAreas[index]);

                _clueAreas[index].PlayerEnter -= EnterVisualClueArea;
                _clueAreas[index].PlayerLeave -= LeaveVisualClueArea;
                _clueAreas[index] = null;
            }

            if (_interactionAreas[index] != null)
            {
                AreaLibrary.Untrack(_interactionAreas[index]);

                _interactionAreas[index].PlayerEnter -= EnterInteractionArea;
                _interactionAreas[index].PlayerLeave -= LeaveInteractionArea;
                _interactionAreas[index] = null;
            }
        }

        private void DestroyBlips(int index)
        {
            if (_areaBlips[index] != null)
            {
                _areaBlips[index].Remove();
                _areaBlips[index] = null;
            }

            if (_areaClueBlips[index] != null)
            {
                _areaClueBlips[index].Remove();
                _areaClueBlips[index] = null;
            }
        }

        private void DestroyClueImmersive(TreasureFlags flag)
        {
            int index = FlagToIndex(flag);

            _props[index]?.MarkAsNoLongerNeeded();
            DestroyAreas(index);
            DestroyBlips(index);
        }
        #endregion

        #region Public methods
        public override void Init(bool scriptStart)
        {
            Vector3 areaOffset = new Vector3(0.0f, 0.0f, 0.5f);

            for (int i = 0; i < MaxClues; i++)
            {
                if (SaveManager.HasFlag(ClueFlags[i]))
                {
                    continue;
                }

                // Prop
                Location location = LocationManager.GetClueLocation(i);
                _props[i] = World.CreateProp(ClueModels[i], location.Position, false, false);
                _props[i].Heading = location.Heading;
                _props[i].Rotation = location.Rotation;

                // Blips
                Vector3 visualClueCenter = location.Position.Around(AreaDistance);
                _areaBlips[i] = CreateAreaBlip(visualClueCenter, AreaRange);
                _areaClueBlips[i] = CreateClueBlip(visualClueCenter);

                // Areas
                _audioAreas[i] = new Sphere(location.Position, 75.0f);
                _audioAreas[i].PlayerEnter += EnterAudioClueArea;
                _audioAreas[i].PlayerLeave += LeaveAudioClueArea;

                _clueAreas[i] = new Sphere(visualClueCenter, AreaRange);
                _clueAreas[i].SetData("thMod_BlipIndex", i);
                _clueAreas[i].PlayerEnter += EnterVisualClueArea;
                _clueAreas[i].PlayerLeave += LeaveVisualClueArea;

                _interactionAreas[i] = new Sphere(CameraManager.GetClueCamera(i).Position - areaOffset, 1.5f);
                _interactionAreas[i].SetData("thMod_AddsFlag", ClueFlags[i]);
                _interactionAreas[i].PlayerEnter += EnterInteractionArea;
                _interactionAreas[i].PlayerLeave += LeaveInteractionArea;

                AreaLibrary.Track(_audioAreas[i]);
                AreaLibrary.Track(_clueAreas[i]);
                AreaLibrary.Track(_interactionAreas[i]);
            }

            // Display help text
            if (!scriptStart)
            {
                _helpTextHideAt = Game.GameTime + HelpTextTime;

                for (int i = 0; i < MaxClues; i++)
                {
                    if (_areaClueBlips[i] != null)
                    {
                        Function.Call(Hash.SET_BLIP_FLASHES, _areaClueBlips[i].Handle, true);
                        Function.Call(Hash.SET_BLIP_FLASH_TIMER, _areaClueBlips[i].Handle, BlipFlashTime);
                    }
                }
            }
        }

        public override bool Update()
        {
            if (_helpTextHideAt > 0 && _interactionFlag == TreasureFlags.None && Game.GameTime < _helpTextHideAt)
            {
                Util.DisplayHelpTextThisFrame(_helpTextIsClue ? "TREA1_HELP" : "TREA2_HELP");
            }

            if (_interactionFlag != TreasureFlags.None)
            {
                // Disable other helptext, no longer needed
                if (_helpTextHideAt > 0)
                {
                    _helpTextHideAt = 0;
                    _helpTextIsClue = false;
                }

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
                        DestroyClueImmersive(_interactionFlag);
                        
                        SaveManager.AddFlag(_interactionFlag);
                        SaveManager.Save();

                        Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "clue_complete_shard", "dlc_xm_fm_th_sounds", false);

                        int numFound = GetNumCluesFound();
                        Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "TREASURE_COLLECT");
                        Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, numFound);
                        Function.Call(Hash._DRAW_NOTIFICATION, false, true);

                        if (numFound == MaxClues)
                        {
                            return true;
                        }

                        _interactionFlag = TreasureFlags.None;
                    }
                    else
                    {
                        CameraData cameraData = CameraManager.GetClueCamera(FlagToIndex(_interactionFlag));

                        _cam = World.CreateCamera(cameraData.Position, cameraData.Rotation, cameraData.FOV);
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

            for (int i = 0; i < MaxClues; i++)
            {
                if (scriptExit)
                {
                    if (_props[i] != null)
                    {
                        _props[i].Delete();
                        _props[i] = null;
                    }
                }
                else
                {
                    _props[i]?.MarkAsNoLongerNeeded();
                    DestroyAreas(i);
                }

                DestroyBlips(i);
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
        private void EnterAudioClueArea(AreaBase area)
        {
            if (_soundId == -1)
            {
                _soundId = Util.PlaySoundFromCoord((area as Sphere).Center, "dlc_xm_fm_th_sounds", "item_close_loop");
            }
        }

        private void LeaveAudioClueArea(AreaBase area)
        {
            StopSound();
        }

        private void EnterVisualClueArea(AreaBase area)
        {
            if (area.GetData("thMod_BlipIndex", out int blipIdx) && _areaClueBlips[blipIdx] != null)
            {
                _areaClueBlips[blipIdx].Alpha = 0;
            }

            _helpTextIsClue = true;
            _helpTextHideAt = Game.GameTime + HelpTextTime;
        }

        private void LeaveVisualClueArea(AreaBase area)
        {
            if (area.GetData("thMod_BlipIndex", out int blipIdx) && _areaClueBlips[blipIdx] != null)
            {
                _areaClueBlips[blipIdx].Alpha = 255;
            }

            if (_helpTextHideAt > 0 && _helpTextIsClue)
            {
                _helpTextHideAt = 0;
            }
        }

        private void EnterInteractionArea(AreaBase area)
        {
            if (area.GetData("thMod_AddsFlag", out TreasureFlags flag))
            {
                _interactionFlag = flag;
            }
        }

        private void LeaveInteractionArea(AreaBase area)
        {
            CameraManager.Disable();
            _interactionFlag = TreasureFlags.None;
        }
        #endregion
    }
}
