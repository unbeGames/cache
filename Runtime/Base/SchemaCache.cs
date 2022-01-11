using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Unbegames.Services {
  public interface IDefindex {
    int defindex { get; }
  }

  public class SchemaCache<T> : BaseCache where T : IDefindex {

    protected AsyncOperationHandle handle;

    protected Dictionary<int, T> schemaItems = new Dictionary<int, T>();
    protected string tagName;

    public SchemaCache(string tagName) {
      this.tagName = tagName;
    }

    public override void Preload(DataStore _) {
      handle = AddressableHelper.LoadAssets<TextAsset>(tagName, Prealoading);
    }

    private void Prealoading(IList<TextAsset> assets) {
      foreach (var resource in assets) {
        var schemaItem = JsonConvert.DeserializeObject<T>(resource.text);
        Postprocess(schemaItem);
        schemaItems.Add(schemaItem.defindex, schemaItem);
      }
      AddressableHelper.Release(handle);
      isPreloaded = true;
    }

    public T Get(int defindex) {
      if (!schemaItems.ContainsKey(defindex)) {
        Helpers.Error($"Can not find schema item with {defindex} of type {typeof(T).Name}");
      }
      return schemaItems[defindex];
    }

    public override void Release() {
      schemaItems.Clear();
      base.Release();
    }

    public IEnumerable<T> All() {
      return schemaItems.Values;
    }

    protected virtual void Postprocess(T item) {

    }
  }
}
