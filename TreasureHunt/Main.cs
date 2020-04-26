using System;
using GTA;
using GTA.Native;
using TreasureHunt.Enums;
using TreasureHunt.Classes;
using TreasureHunt.Managers;

namespace TreasureHunt
{
    public class Main : Script
    {
        public TreasureStage CurrentStage = TreasureStage.None;
        public StageBase CurrentStageHandler = null;
        public bool GameReady = false;

        #region Methods
        public void MakeNewHandler(TreasureStage newStage, bool runInit)
        {
            if (CurrentStageHandler != null)
            {
                CurrentStageHandler.Destroy(false);
                CurrentStageHandler = null;
            }

            switch (newStage)
            {
                case TreasureStage.SearchingNote:
                    CurrentStageHandler = new SearchingNoteStage();
                    break;

                case TreasureStage.SearchingClues:
                    CurrentStageHandler = new SearchingCluesStage();
                    break;

                case TreasureStage.SearchingChest:
                    CurrentStageHandler = new SearchingChestStage();
                    break;

                case TreasureStage.Found:
                    CurrentStageHandler = new FoundStage();
                    break;

                default:
                    throw new NotImplementedException("Not implemented stage used with MakeNewHandler.");
            }

            if (runInit)
            {
                CurrentStageHandler?.Init(false);
            }
        }
        #endregion

        #region Constructor
        public Main()
        {
            CurrentStage = SaveManager.Load();
            MakeNewHandler(CurrentStage, false);

            Tick += Main_Tick;
            Aborted += Main_Aborted;
        }
        #endregion

        #region Events
        public void Main_Tick(object sender, EventArgs e)
        {
            if (!GameReady && !Game.IsLoading && Game.Player.CanControlCharacter)
            {
                Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_CHRISTMAS2017/FM_TH", false, -1);

                GameReady = true;
                CurrentStageHandler?.Init(true);
            }

            if (CurrentStageHandler != null && CurrentStageHandler.Update())
            {
                CurrentStage = CurrentStageHandler.NextStage;
                MakeNewHandler(CurrentStage, true);
            }
        }

        public void Main_Aborted(object sender, EventArgs e)
        {
            if (CurrentStageHandler != null)
            {
                CurrentStageHandler.Destroy(true);
                CurrentStageHandler = null;
            }
        }
        #endregion
    }
}
