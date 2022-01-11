using UnityEngine;
using UnityEngine.VFX;

namespace Unbegames.Services {
	public class VFXCache : AreaCache<GameObject> {
    protected override string preloadLabel => "preload_effects";			

		public VisualEffect GetEffect(string name) {
			VisualEffect visualEffect = null;
			var go = Get(name);
			if(go != null) {
				visualEffect = go.GetComponent<VisualEffect>();
      }
			return visualEffect;
		}

		public VisualEffect GetInstance(string name, Transform parent) {
			VisualEffect vfx = null;
			var original = Get(name);
			if(original != null) {
				var go = GameObject.Instantiate(original, parent);
				go.name = name;
				vfx = go.GetComponent<VisualEffect>();
			}
			return vfx;
		}
	}
}
