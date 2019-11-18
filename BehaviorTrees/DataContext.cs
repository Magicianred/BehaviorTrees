using System.Collections.Generic;

namespace BehaviorTrees
{
    public class DataContext : Dictionary<string, object>
    {

        public T Get<T>(string key)
        {
            if (!ContainsKey(key))
                return default;

            return (T)this[key];
        }

    }
}
