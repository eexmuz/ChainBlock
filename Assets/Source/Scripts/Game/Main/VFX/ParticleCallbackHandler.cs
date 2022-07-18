using UnityEngine;

public class ParticleCallbackHandler : MonoBehaviour
{
    public event System.Action OnSystemStopped;
    
    private void OnParticleSystemStopped()
    {
        OnSystemStopped?.Invoke();
    }
}
