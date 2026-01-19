using System.Collections.Generic;

namespace Utilites.Pooling {
    public static class PoolProvider {
        static Dictionary<string, IPoolLoader> _loaders = new Dictionary<string, IPoolLoader>();

        public static void RegisterLoader(IPoolLoader loader) {
            if (_loaders.ContainsKey(loader.LoaderName))
                _loaders.Remove(loader.LoaderName);
            _loaders.Add(loader.LoaderName, loader);
        }

        public static void RemoveLoader(IPoolLoader loader) {
            if (_loaders.ContainsKey(loader.LoaderName))
                _loaders.Remove(loader.LoaderName);
        }

        public static IPoolLoader GetLoader(string loaderName) {
            return _loaders.TryGetValue(loaderName, out var loader) ? loader : null;
        }
    }
}