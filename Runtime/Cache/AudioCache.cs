using UnityEngine;

namespace Unbegames.Services {
	public class AudioCache : AreaCache<AudioClip> {
    protected override string preloadLabel => "preload_sounds"; 

    public AudioCache(params string[] areas) : base(areas) {
			
    }
	}
}
