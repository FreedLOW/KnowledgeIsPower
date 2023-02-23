using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> completedCache = new Dictionary<string, AsyncOperationHandle>();
        private readonly Dictionary<string, List<AsyncOperationHandle>> handles = new Dictionary<string, List<AsyncOperationHandle>>();

        public void Initialize()
        {
            Addressables.InitializeAsync();
        }
        
        public async Task<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWithCacheOnComplete(
                Addressables.LoadAssetAsync<T>(assetReference), 
                cacheKey: assetReference.AssetGUID);
        }

        public async Task<T> Load<T>(string address) where T : class
        {
            if (completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await RunWithCacheOnComplete(
                Addressables.LoadAssetAsync<T>(address), 
                cacheKey: address);
        }

        public async Task<GameObject> Instantiate(string address) => 
            await Addressables.InstantiateAsync(address).Task;

        public async Task<GameObject> Instantiate(string address, Vector3 at) => 
            await Addressables.InstantiateAsync(address, at, Quaternion.identity).Task;

        public void CleanUp()
        {
            foreach (List<AsyncOperationHandle> resourceHandles in handles.Values)
            {
                foreach (AsyncOperationHandle handle in resourceHandles)
                {
                    Addressables.Release(handle);
                }
            }
            
            completedCache.Clear();
            handles.Clear();
        }

        private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += completeHandle =>
            {
                completedCache[cacheKey] = completeHandle;
            };

            AddHandle(key: cacheKey, handle);

            return await handle.Task;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                handles[key] = resourceHandles;
            }

            resourceHandles.Add(handle);
        }
    }
}