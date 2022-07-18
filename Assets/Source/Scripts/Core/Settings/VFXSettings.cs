using System.Collections.Generic;
using Core.Services;
using UnityEngine;


namespace Core.Settings
{
    [CreateAssetMenu(menuName = "Settings/VFX Settings")]
    public class VFXSettings : ScriptableObject, ISettings
    {
        [System.Serializable]
        private struct Effect
        {
            public VFXId Key;
            public VisualEffect Prefab;
        }

        [SerializeField]
        private Effect[] _effects;

        private Dictionary<VFXId, VisualEffect> _map;

        public VisualEffect GetVFXPrefab(VFXId key)
        {
            _map ??= CreateMap();
            return _map[key];
        }

        private Dictionary<VFXId, VisualEffect> CreateMap()
        {
            _map = new Dictionary<VFXId, VisualEffect>();
            foreach (var effect in _effects)
            {
                _map.Add(effect.Key, effect.Prefab);
            }

            return _map;
        }
    }
}