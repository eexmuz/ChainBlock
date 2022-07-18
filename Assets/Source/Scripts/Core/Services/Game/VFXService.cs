using Core.Attributes;
using Core.Settings;
using UnityEngine;

namespace Core.Services
{
    [InjectionAlias(typeof(IVFXService))]
    public class VFXService : Service, IVFXService
    {
        [Inject]
        private VFXSettings _settings;
        
        public void Create(VFXId vfxId, Vector3 position, Quaternion rotation, Transform parent)
        {
            VisualEffect effect = Instantiate(_settings.GetVFXPrefab(vfxId), position, rotation, parent);
            effect.PlayAndDestroy();
        }
    }
}