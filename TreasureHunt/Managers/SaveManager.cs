using System;
using System.IO;
using System.Xml;
using TreasureHunt.Enums;

namespace TreasureHunt.Managers
{
    public static class SaveManager
    {
        #region Constants
        private const int SaveVersion = 1;
        private static readonly string SaveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SPTreasureHunt");
        private static readonly string SaveFile = Path.Combine(SaveDirectory, "save.xml");
        #endregion

        #region Save data
        public static int NoteIndex { get; private set; } = 0;
        public static int ChestIndex { get; private set; } = 0;
        public static TreasureFlags Flags { get; private set; } = TreasureFlags.None;
        #endregion

        #region Private methods
        private static void MakeDefaultSave()
        {
            Random random = new Random();

            NoteIndex = random.Next(0, LocationManager.MaxLocations);
            ChestIndex = random.Next(0, LocationManager.MaxLocations);
            Flags = TreasureFlags.None;

            Save();
        }
        #endregion

        #region Public methods
        public static TreasureStage Load()
        {
            if (!Directory.Exists(SaveDirectory) || !File.Exists(SaveFile))
            {
                MakeDefaultSave();
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(SaveFile);

            int saveVersion = Convert.ToInt32(doc.SelectSingleNode("/TreasureHuntSave/@version")?.Value);
            if (SaveVersion != saveVersion)
            {
                throw new NotImplementedException("Save version handling is not implemented.");
            }

            // Load location data
            XmlNode locations = doc.SelectSingleNode("//LocationData");
            if (locations != null)
            {
                NoteIndex = Convert.ToInt32(locations.Attributes["noteIndex"]?.Value);
                ChestIndex = Convert.ToInt32(locations.Attributes["chestIndex"]?.Value);
            }
            else
            {
                throw new Exception("LocationData not found in save file.");
            }

            // Load flags
            XmlNode flags = doc.SelectSingleNode("//Flags");
            if (Enum.TryParse(flags?.InnerText, out TreasureFlags newFlags))
            {
                Flags = newFlags;
            }
            else
            {
                throw new Exception("Failed to load Flags from save file.");
            }

            // Find current stage
            if (HasFlag(TreasureFlags.FoundNote))
            {
                if (HasFlag(TreasureFlags.FoundFinalChest))
                {
                    return TreasureStage.Found;
                }
                else if (HasFlag(TreasureFlags.FoundCorpse) && HasFlag(TreasureFlags.FoundShovel) && HasFlag(TreasureFlags.FoundEmptyChest))
                {
                    return TreasureStage.SearchingChest;
                }
                else
                {
                    return TreasureStage.SearchingClues;
                }
            }
            else
            {
                return TreasureStage.SearchingNote;
            }
        }

        public static void AddFlag(TreasureFlags flag)
        {
            Flags |= flag;
        }

        public static bool HasFlag(TreasureFlags flag)
        {
            return (Flags & flag) == flag;
        }

        public static void RemoveFlag(TreasureFlags flag)
        {
            Flags &= ~flag;
        }

        public static void Save()
        {
            XmlDocument doc = new XmlDocument();

            XmlElement root = doc.CreateElement("TreasureHuntSave");
            root.SetAttribute("version", SaveVersion.ToString());

            // Locations
            XmlElement locationData = doc.CreateElement("LocationData");
            locationData.SetAttribute("noteIndex", NoteIndex.ToString());
            locationData.SetAttribute("chestIndex", ChestIndex.ToString());
            root.AppendChild(locationData);

            // Flags
            XmlNode flagData = doc.CreateElement("Flags");
            flagData.InnerText = Flags.ToString();

            // Save
            root.AppendChild(flagData);
            doc.AppendChild(root);

            Directory.CreateDirectory(SaveDirectory);
            doc.Save(SaveFile);
        }
        #endregion
    }
}
