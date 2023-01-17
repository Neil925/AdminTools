using PluginAPI.Core.Factories;
using PluginAPI.Core.Interfaces;

namespace AdminTools
{
    public sealed class AdminToolsPlayerFactory : PlayerFactory
    {
        public override IPlayer Create(IGameComponent component) => new AtPlayer(component);
    }
}
