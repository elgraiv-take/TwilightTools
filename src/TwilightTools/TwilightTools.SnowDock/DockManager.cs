using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elgraiv.TwilightTools.SnowDock.Impl;
using Elgraiv.TwilightTools.SnowDock.Impl.Model;

namespace Elgraiv.TwilightTools.SnowDock;

public class DockManager
{

    private LayoutSystem _layoutSystem = new();
    internal LayoutSystem LayoutSystem => _layoutSystem;

    private Dictionary<string, ContentPanelRecord> _registeredPanel = new();

    private bool _isBuilt = false;

    internal event EventHandler<LayoutUpdatedEventArgs>? LayoutUpdated;

    public void RegisterPanelViewModel(IDockPanelViewModel panel)
    {
        var content = new LayoutContent(panel);
        content.ExpectedPath = panel.PreferedPath;
        _registeredPanel.TryAdd(panel.ContentId, new ContentPanelRecord(content, panel));

        if (_isBuilt)
        {
            _layoutSystem.AddContent(content.ExpectedPath ?? new LayoutPath(), content);
            LayoutUpdated?.Invoke(this, new LayoutUpdatedEventArgs());
        }
    }

    public void BuildLayout()
    {
        _layoutSystem.Reset();

        foreach(var (_,content) in _registeredPanel)
        {
            _layoutSystem.AddContent(content.Content.ExpectedPath ?? new LayoutPath(), content.Content);
        }
        _layoutSystem.OptimizeLayout();
        _isBuilt = true;
        LayoutUpdated?.Invoke(this, new LayoutUpdatedEventArgs());
    }

    internal IDockPanelViewModel? GetViewModel(string contentId)
    {
        if(_registeredPanel.TryGetValue(contentId,out var panel)){
            return panel.ViewModel;
        }
        return null;
    }

    public void SerializeLayout(bool includeInternal, object args)
    {

    }
    public void RestoreLayout(LayoutData layout)
    {
        _layoutSystem.Reset();




        _isBuilt = true;
        LayoutUpdated?.Invoke(this, new LayoutUpdatedEventArgs());
    }
    

}
