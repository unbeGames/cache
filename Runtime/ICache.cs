using UnityEngine;

namespace Unbegames.Services {
	public interface ICache {
		bool isPreloaded { get; }
		void Preload(DataStore dataStore);
		void Release();
	}
}
