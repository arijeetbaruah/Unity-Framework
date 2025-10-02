using UnityEngine;

namespace Baruah.Service
{
    public interface IService
    {
        void Initialize();
        void Update();
        void OnDestroy();
    }
}
