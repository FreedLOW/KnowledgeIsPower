namespace CodeBase.Infrastructure.Services
{
    public class AllServices
    {
        #region Singelton

        private static AllServices instance;
        public static AllServices Container => instance ?? (instance = new AllServices());

        #endregion

        public void RegisterSingle<TService>(TService implementation) where TService : IService => 
            Implementation<TService>.ServiceInstance = implementation;

        public TService Single<TService>() where TService : IService => 
            Implementation<TService>.ServiceInstance;

        public void RemoveSingle<TService>() where TService : class, IService => 
            Implementation<TService>.ServiceInstance = null;

        private static class Implementation<TService> where TService : IService
        {
            public static TService ServiceInstance;
        }
    }
}