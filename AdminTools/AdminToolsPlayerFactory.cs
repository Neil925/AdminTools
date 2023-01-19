using PluginAPI.Core.Factories;
using PluginAPI.Core.Interfaces;
using System;

namespace AdminTools
{
    public sealed class AdminToolsPlayerFactory : PlayerFactory
    {
        public override IPlayer Create(IGameComponent component) => new AtPlayer(component);

        public override Type BaseType => typeof(AtPlayer);
    }
}
