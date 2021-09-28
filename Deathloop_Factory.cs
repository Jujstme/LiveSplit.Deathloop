using LiveSplit.Deathloop;
using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Reflection;

[assembly: ComponentFactory(typeof(Factory))]

namespace LiveSplit.Deathloop
{
    public class Factory : IComponentFactory
    {
        public string ComponentName => "DEATHLOOP Autosplitter";
        public string Description => "Automatic splitting and loadless timing";
        public ComponentCategory Category => ComponentCategory.Control;
        public string UpdateName => this.ComponentName;
        public string UpdateURL => "https://raw.githubusercontent.com/Jujstme/LiveSplit.Deathloop/master/";
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public string XMLURL => this.UpdateURL + "Components/update.LiveSplit.Deathloop.xml";
        public IComponent Create(LiveSplitState state)
        {
            return new Component(state);
        }

    }
}
