using System.Windows.Forms;
using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;

namespace UniqueLootHelper
{
    public class Settings : ISettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(false);
        public ButtonNode RefreshUniquesFile { get; set; } = new ButtonNode();
        public ColorNode Color { get; set; } = new ColorNode(SharpDX.Color.Purple);
        public RangeNode<int> FrameThickness { get; set; } = new RangeNode<int>(2, 1, 5);
        public RangeNode<uint> CacheIntervall { get; set; } = new RangeNode<uint>(2, 1, 5);
    }
}
