using ExileCore;
using ExileCore.PoEMemory.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ExileCore.Shared.Cache;
using ExileCore.PoEMemory.Elements;

namespace UniqueLootHelper
{
    public class UniqueLootHelperCore : BaseSettingsPlugin<Settings>
    {
        private const string UNIQUESARTWORK_FILE = "UniquesArtworks.txt";
        private HashSet<string> UniquesHashSet;
        
        public List<SharpDX.RectangleF> drawingList = new List<SharpDX.RectangleF>();

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
                UniquesHashSet = File.ReadAllLines(path).Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("#")).ToList().Select(x => x + ".dds").ToHashSet();
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
            foreach (var frame in drawingList)
            {
                Graphics.DrawFrame(frame, Settings.Color, Settings.FrameThickness);
            }
        }

        public override Job Tick()
        {
            drawingList.Clear();
            if (GameController.Area.CurrentArea.IsHideout ||
                GameController.Area.CurrentArea.IsTown)
            {
                return null;
            }
            foreach (var label in GameController.IngameState.IngameUi.ItemsOnGroundLabelsVisible)
            {
                var worlditem = label.ItemOnGround.GetComponent<WorldItem>();
                if (worlditem == null) continue;
                if (worlditem.ItemEntity.Type != ExileCore.Shared.Enums.EntityType.Item) continue;
                var renderitem = worlditem.ItemEntity.GetComponent<RenderItem>();
                if (renderitem == null) continue;
                var modelPath = renderitem.ResourcePath;
                if (modelPath == null) continue;
                if (UniquesHashSet.Contains(modelPath))
                {
                    drawingList.Add(label.Label.GetClientRectCache);
                }
            }
            return null;
        }
    }
}
