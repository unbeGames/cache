using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Unbegames.Services {
	public class MusicCache : BaseCache {
		public Dictionary<string, List<string>> allPlaylists;

		public event EventHandler OnLoaded;
		private readonly Dictionary<string, AudioClip> loadedClips = new Dictionary<string, AudioClip>();

		public override async void Preload(DataStore _) {
			var result = await AddressableHelper.LoadAssetAsync<TextAsset>("playlist");
			PreloadPlaylist(result);
		}

		private void PreloadPlaylist(TextAsset asset) {
			allPlaylists = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(asset.text);
			AddressableHelper.Release(asset);
			isPreloaded = true;
			OnLoaded?.Invoke();
		}

		public override void Release() {
			allPlaylists = null;
			loadedClips.Clear();
			base.Release();
		}

		public bool HasClip(string name) {
			return loadedClips.ContainsKey(name);
		}

		public AudioClip GetClip(string name) {
			var clip = loadedClips[name];
			if (loadedClips.Count > 1) {
				foreach (var pair in loadedClips) {
					if (pair.Key != name) {
						AddressableHelper.Release(pair.Value);
					}
				}
				loadedClips.Clear();
				loadedClips.Add(name, clip);
			}
			return clip;
		}

		public async void RequestClip(string name){
			if (!loadedClips.ContainsKey(name)) {
				var clip = await AddressableHelper.LoadAssetAsync<AudioClip>(name);
				loadedClips.Add(name, clip);
			}
		}
	}
}
