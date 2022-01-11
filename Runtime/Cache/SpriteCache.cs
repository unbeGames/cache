using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unbegames.Services {
	public class SpriteCache : ICache {
		public bool isPreloaded => sprites.Count >= preloadNames.Length;

		private readonly Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
		private readonly Dictionary<string, Mesh> meshSprites = new Dictionary<string, Mesh>();

		public void Preload(DataStore _) {
			foreach (var name in preloadNames) {
				Load(name);
				if (iconToMeshMapping.ContainsKey(name)) {
					LoadMesh(name);
				}
			}			
		}

		public Sprite Get(string name) {
			Sprite sprite = null;
			if (sprites.ContainsKey(name)) {
				sprite = sprites[name];
			}
			return sprite;
		}
		
		public Mesh GetMeshSprite(string name) {
			if (!meshSprites.ContainsKey(name)) {
				var sprite = Get(name);
				Mesh mesh = new Mesh();
				mesh.SetVertices(Array.ConvertAll(sprite.vertices, i => (Vector3)i).ToList());
				mesh.SetUVs(0, sprite.uv);
				mesh.SetTriangles(sprite.triangles, 0);
				meshSprites.Add(name, mesh);
			}
			return meshSprites[name];
		}
			

		public void Release() {
			sprites.Clear();
			meshSprites.Clear();
		}

		public void Get(string name, Action<Sprite> cb){
			if (sprites.ContainsKey(name)){
				cb(sprites[name]);
			}	else {
				Load(name, cb);
			}
		}		

		private async void Load(string name, Action<Sprite> cb = null) {
			var sprite = await AddressableHelper.LoadAssetAsync<Sprite>(name);
			if (!sprites.ContainsKey(name)) {				
				sprites.Add(name, sprite);
			}
			cb?.Invoke(sprite);
		}

		private async void LoadMesh(string name) {
			var path = iconToMeshMapping[name];
			var mesh = await AddressableHelper.LoadAssetAsync<Mesh>(path);
			if (!meshSprites.ContainsKey(name)) {
				meshSprites.Add(name, mesh);
			}
		}

		private Dictionary<string, string> iconToMeshMapping = new Dictionary<string, string> {
		};

		private string[] preloadNames = new string[] { 
		};
	};	
}
