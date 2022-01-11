using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Unbegames.Services {
  public class AreaCache<T> : BaseCache where T : UnityEngine.Object {
    protected static readonly List<string> areaLabels = new List<string>();

    protected virtual string preloadLabel { get; }

    public bool areaLoaded { get; private set; }

    // preloaded assets
    protected readonly Dictionary<string, T> assets = new Dictionary<string, T>();
    // assets specific to area in the game
    protected readonly Dictionary<string, T> areaAssets = new Dictionary<string, T>();

    private AsyncOperationHandle handle;

    public AreaCache(params string[] areas) {
      foreach (var area in areas) {
        AddAreaLabel(area);
      }
    }

    public override void Preload(DataStore _) {
      AddressableHelper.LoadAssets<T>(preloadLabel, Prealoading);
    }

    public void AddAreaLabel(string area) {
      areaLabels.Add(area);
    }

    public void LoadArea(int area) {      
      if (area >= 0 && area < areaLabels.Count) {
        var label = areaLabels[area];
        handle = AddressableHelper.LoadAssets<T>(label, LoadingArea);
      }
    }

    public void UnloadArea() {
      AddressableHelper.Release(handle);
      areaAssets.Clear();
      areaLoaded = false;
    }

    public override void Release() {
      assets.Clear();
      areaAssets.Clear();
      base.Release();
    }

    public T Get(string name) {
      T asset = null;
      if (assets.ContainsKey(name)) {
        asset = assets[name];
      } else if (areaAssets.ContainsKey(name)) {
        asset = areaAssets[name];
      } else {
        Helpers.Warn($"Asset with name {name} not found");
      }
      return asset;
    }

    private void LoadingArea(IList<T> list) {
      foreach (var clip in list) {
        areaAssets.Add(clip.name, clip);
      }
      areaLoaded = true;
    }

    private void Prealoading(IList<T> list) {
      foreach (var asset in list) {
        assets.Add(asset.name, asset);
      }
      isPreloaded = true;
    }
  }
}
