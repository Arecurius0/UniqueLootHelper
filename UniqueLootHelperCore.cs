using ExileCore;
using ExileCore.PoEMemory.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace UniqueLootHelper
{
    public class UniqueLootHelperCore : BaseSettingsPlugin<Settings>
    {
        private const string UNIQUESARTWORK_FILE = "UniquesArtworks.txt";
        private HashSet<String> UniquesHashSet;

        public override bool Initialise()
        {
            Name = "UniqueLootHelper";
            Settings.RefreshUniquesFile.OnPressed += () => { ReadUniquesArtworkFile(); };
            ReadUniquesArtworkFile();
            return base.Initialise();
        }
        private void ReadUniquesArtworkFile()
        {
            var path = $"{DirectoryFullName}\\{UNIQUESARTWORK_FILE}";
            if (File.Exists(path))
            {
                UniquesHashSet = File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("#")).ToHashSet();
            }
            else
                CreateUniquesArtworkFile();
        }

        private void CreateUniquesArtworkFile()
        {
            var path = $"{DirectoryFullName}\\{UNIQUESARTWORK_FILE}";
            if (File.Exists(path)) return;
            using (var streamWriter = new StreamWriter(path, true))
            {
                streamWriter.Write("");
                streamWriter.Close();
            }
        }
        public override void Render()
        {
            if(GameController.Area.CurrentArea.IsHideout ||
                GameController.Area.CurrentArea.IsTown)
            {
                return;
            }
            
            foreach(var label in GameController.IngameState.IngameUi.ItemsOnGroundLabelsVisible)
            {
                var modelPath = label.ItemOnGround.GetComponent<WorldItem>()?.ItemEntity.GetComponent<RenderItem>()?.ResourcePath;
                if (modelPath == null) continue;
                var trimmedPath = modelPath.Substring(0, modelPath.IndexOf("."));
                if (UniquesHashSet.Contains(trimmedPath))
                {
                    Graphics.DrawFrame(label.Label.GetClientRectCache, Settings.Color, Settings.FrameThickness);
                }
            }
            
            base.Render();
        }

        public override Job Tick()
        {

            return base.Tick();
        }
    }
}
