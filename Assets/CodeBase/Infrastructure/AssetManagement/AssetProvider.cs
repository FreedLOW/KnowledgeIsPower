using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public void Initialize()
        {
            Addressables.InitializeAsync();
        }
        
        public async Task<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (_completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWithCacheOnComplete(
                handle: Addressables.LoadAssetAsync<T>(assetReference), 
                cacheKey: assetReference.AssetGUID);
        }

        public async Task<T> Load<T>(string address) where T : class
        {
            if (_completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWithCacheOnComplete(
                handle: Addressables.LoadAssetAsync<T>(address),
                cacheKey: address);
        }

        public void Cleanup()
        {
            foreach (List<AsyncOperationHandle> handles in _handles.Values)
            {
                foreach (AsyncOperationHandle handle in handles)
                {
                    Addressables.Release(handle);
                }
            }
            
            _completedCache.Clear();
            _handles.Clear();
        }

        public Task<GameObject> Instantiate(string path, Vector3 at) => 
            Addressables.InstantiateAsync(path, at, Quaternion.identity).Task;

        public Task<GameObject> Instantiate(string path) => 
            Addressables.InstantiateAsync(path).Task;

        private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += completeHandle =>
            {
                _completedCache[cacheKey] = completeHandle;
            };

            AddHandles(cacheKey, handle);

            return await handle.Task;
        }

        private void AddHandles<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourcesHandles))
            {
                resourcesHandles = new List<AsyncOperationHandle>();
                _handles[key] = resourcesHandles;
            }

            resourcesHandles.Add(handle);
        }
    }
}