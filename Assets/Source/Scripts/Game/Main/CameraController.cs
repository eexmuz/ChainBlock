using Core;
using Core.Attributes;
using Core.Settings;
using UnityEngine;

public class CameraController : DIBehaviour
{
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private float _borderWidth;
    
    [Inject]
    private GameSettings _gameSettings;
    
    public void SetupCamera(LevelData levelData)
    {
        float boardWidth = levelData.Dimensions.x * _gameSettings.CellSize.x + _borderWidth;
        float tanFOV = Mathf.Tan(Camera.VerticalToHorizontalFieldOfView(_camera.fieldOfView, _camera.aspect) * Mathf.Deg2Rad);

        var position = _camera.transform.position;
        position.z = -boardWidth / tanFOV;
        _camera.transform.position = position;
    }
}