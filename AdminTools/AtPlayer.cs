using PluginAPI.Core;
using PluginAPI.Core.Interfaces;

namespace AdminTools
{
    public sealed class AtPlayer : Player
    {
        public AtPlayer(IGameComponent component) : base(component) { }
        
        public bool PryGateEnabled { get; set; }
        
        public bool BreakDoorsEnabled { get; set; }
        
        public bool InstantKillEnabled { get; set; }
    }
}
