﻿using System.Linq;
using Content.Client.GameTicking.Managers;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client._Afterlight.Latejoin;

[GenerateTypedNameReferences]
public sealed partial class VesselListControl : BoxContainer
{
    private ClientGameTicker _gameTicker;

    public Comparison<EntityUid>? Comparison;

    public EntityUid? Selected
    {
        get
        {
            var i = VesselItemList.GetSelected().FirstOrDefault();
            if (i is null)
                return null;

            return (EntityUid) i.Metadata!;
        }
    }

    public VesselListControl()
    {
        _gameTicker = EntitySystem.Get<ClientGameTicker>();
        IoCManager.InjectDependencies(this);
        RobustXamlLoader.Load(this);
        _gameTicker.LobbyJobsAvailableUpdated += UpdateUi;
        UpdateUi(_gameTicker.JobsAvailable);

        Comparison = DefaultComparison;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _gameTicker.LobbyJobsAvailableUpdated -= UpdateUi;
    }

    private int DefaultComparison(EntityUid x, EntityUid y)
    {
        return (int)(_gameTicker.JobsAvailable[x].Values.Sum(a => a ?? 0) - _gameTicker.JobsAvailable[y].Values.Sum(b => b ?? 0));
    }

    public void Sort()
    {
        if(Comparison != null)
            VesselItemList.Sort((a, b) => Comparison((EntityUid) a.Metadata!, (EntityUid) b.Metadata!));
    }

    private void UpdateUi(IReadOnlyDictionary<EntityUid, Dictionary<string, uint?>> obj)
    {
        var itemsToRemove = new List<ItemList.Item>();
        foreach (var (key, item) in VesselItemList.Select(x => ((EntityUid)x.Metadata!, x)))
        {
            if ((!_gameTicker.StationNames.ContainsKey(key)) ||
                (_gameTicker.JobsAvailable[key].Values.Sum(x => x ?? 0) <= 0))
            {
                itemsToRemove.Add(item);
            }
        }

        foreach (var item in itemsToRemove)
        {
            VesselItemList.Remove(item);
        }

        foreach (var (key, name) in _gameTicker.StationNames)
        {
            if (VesselItemList.Any(x => ((EntityUid)x.Metadata!) == key) || _gameTicker.JobsAvailable[key].Values.Sum(x => x ?? 0) <= 0)
                continue;

            var item = new ItemList.Item(VesselItemList)
            {
                Metadata = key,
                Text = name
            };

            if (!string.IsNullOrEmpty(FilterLineEdit.Text) &&
                !name.ToLowerInvariant().Contains(FilterLineEdit.Text.Trim().ToLowerInvariant()))
            {
                continue;
            }

            VesselItemList.Add(item);
        }

        Sort();
    }
}
