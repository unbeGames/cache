using System.Collections.Generic;
using System.Linq;

namespace Unbegames.Services {
	public class CacheStore {
		public static bool isPreloaded { get; private set; }

		public static AudioCache audio => staticCaches[CacheType.audio] as AudioCache;
		public static PrefabCache prefabs => staticCaches[CacheType.prefabs] as PrefabCache;
		public static MusicCache music => staticCaches[CacheType.music] as MusicCache;
		public static SpriteCache sprites => staticCaches[CacheType.sprites] as SpriteCache;
		public static VFXCache vfx => staticCaches[CacheType.effects] as VFXCache;

		public enum CacheType {
			audio, prefabs, materials, music, achievements, sprites, effects
		}

		private static Dictionary<CacheType, ICache> staticCaches = new Dictionary<CacheType, ICache>(){
			{ CacheType.audio, new AudioCache() },
			{ CacheType.prefabs, new PrefabCache() },
			{ CacheType.music, new MusicCache() },
			{ CacheType.sprites, new SpriteCache() },
			{ CacheType.effects, new VFXCache() },
		};

		private static Dictionary<System.Type, ICache> dynamicCaches = new Dictionary<System.Type, ICache>();

		public static void AddCache(ICache cache){
			dynamicCaches.Add(cache.GetType(), cache); 
		}

		public static T Get<T>() where T : ICache {
			return (T) dynamicCaches[typeof(T)];
		}

		public static void Release() {
			UnityEngine.Debug.Log("Cleaning CacheStore");
			foreach (var pair in staticCaches) {
				pair.Value.Release();
			}
			foreach (var pair in dynamicCaches) {
				pair.Value.Release();
			}
			dynamicCaches.Clear();
			isPreloaded = false;
		}

		public static void Preload(DataStore dataStore){			
			foreach(var pair in staticCaches){
				pair.Value.Preload(dataStore);
			}
			foreach(var pair in dynamicCaches){
				pair.Value.Preload(dataStore);
			}
		}

		public static void CheckPreload() {
			bool preloaded = true;
			preloaded = preloaded && staticCaches.Values.All(c => c.isPreloaded);
			preloaded = preloaded && dynamicCaches.Values.All(c => c.isPreloaded);
			isPreloaded = preloaded;
		}
	}
}
