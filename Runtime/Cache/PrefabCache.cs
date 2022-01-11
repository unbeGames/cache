using UnityEngine;

namespace Unbegames.Services {
	public class PrefabCache : AreaCache<GameObject> {

		private const string skipSymbol = "_";
		protected override string preloadLabel => "preload_prefabs";

		public override void Preload(DataStore _) {
			var resourcesPrefabs = Resources.LoadAll<GameObject>("prefabs");
			foreach (var prefab in resourcesPrefabs) {
				if (!prefab.name.StartsWith(skipSymbol)) {
					assets.Add(prefab.name, prefab);
				}
			}
			base.Preload(_);
		}
		
		public GameObject Instantiate(string name, Transform parent = null) {
			return GameObject.Instantiate(Get(name), parent);
		}		
		
		public T Instantiate<T>(string name, Transform parent = null) where T : MonoBehaviour {
			return GameObject.Instantiate(Get(name), parent).GetComponent<T>();
		}
	}
}
