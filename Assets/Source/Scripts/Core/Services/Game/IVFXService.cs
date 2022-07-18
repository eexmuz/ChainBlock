using UnityEngine;

namespace Core.Services
{
    public enum VFXId
    {
        IceBreak,
    }
    
    public interface IVFXService : IService
    {
        void Create(VFXId vfxId, Vector3 position, Quaternion rotation, Transform parent);
    }
}