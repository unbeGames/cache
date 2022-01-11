using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Unbegames.Services {
	public static class AddressableHelper {
		public async static Task<IList<T>> LoadAssetsAsync<T>(string name) where T : Object {
			var async = Addressables.LoadAssetsAsync<T>(name, null);
			await async.Task;
			return async.Result;
		}

		public static AsyncOperationHandle LoadAssets<T>(string name, System.Action<IList<T>> callback) where T : Object {
			var async = Addressables.LoadAssetsAsync<T>(name, null);
			async.Completed += (AsyncOperationHandle<IList<T>> op) => callback.Invoke(op.Result);
			return async;
		}		
		
		public static AsyncOperationHandle LoadAssets<T>(IEnumerable<object> keys, System.Action<IList<T>> callback) where T : Object {
			var async = Addressables.LoadAssetsAsync<T>(keys, null, Addressables.MergeMode.Intersection);  
			async.Completed += (AsyncOperationHandle<IList<T>> op) => callback.Invoke(op.Result);
			return async;
		}

		public async static Task<T> InstantiateAsync<T>(string name, Transform parent = null) where T : MonoBehaviour {
			var handle = Addressables.InstantiateAsync(name, parent, false);

			await handle.Task;

			GameObject result = null;

			if (handle.Status == AsyncOperationStatus.Succeeded) {
				result = handle.Result;
			} else {
				Helpers.Error($"Can not instantiate asset with name {name}, status {handle.Status}");
			} 

			return result?.GetComponent<T>();
		}

		public static async Task<T> LoadAssetAsync<T>(string name) where T : Object {
			AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(name);

			await handle.Task;

			T result = null;

			if (handle.Status == AsyncOperationStatus.Succeeded) {
				result = handle.Result;
			} else {
				Helpers.Error($"Can not load asset with name {name}, status {handle.Status}");
			}

			return result;
		}

		public static void Release<T>(T obj) where T : Object {
			Addressables.Release<T>(obj);
		}

		public static void Release(AsyncOperationHandle handle) {
			Addressables.Release(handle);
		}

		public static void ReleaseInstance(GameObject gameObject) {
			Addressables.ReleaseInstance(gameObject);
		}

		public async static Task LoadContentCatalog(string path) {
			var handle = Addressables.LoadContentCatalogAsync(path);
			var locator = await handle.Task;
			Addressables.AddResourceLocator(locator);
			Addressables.Release(handle);
		}
	}
}
