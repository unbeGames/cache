namespace Unbegames.Services {
  public class BaseCache : ICache {
    public virtual bool isPreloaded { get; protected set; }

    public virtual void Preload(DataStore dataStore) {
      isPreloaded = true;
    }

    public virtual void Release() {
      isPreloaded = false;
    }
  }
}
