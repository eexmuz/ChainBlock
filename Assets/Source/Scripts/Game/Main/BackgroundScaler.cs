using Core;
using UnityEngine;

public class BackgroundScaler : DIBehaviour
{
    [SerializeField]
    private Texture2D _texture;
    
    public void UpdateBackground(Camera camera)
    {
        float aspect = _texture.width / (float) _texture.height;
        Vector3 cameraWorldSize = camera.ViewportToWorldPoint(new Vector3(1f, 1f, transform.position.z - camera.transform.position.z));

        if (camera.aspect > aspect) // width controlling height
        {
            transform.localScale = new Vector3(cameraWorldSize.x * 2f, cameraWorldSize.x * 2f / aspect, 1f);
        }
        else // height controlling width
        {
            transform.localScale = new Vector3(cameraWorldSize.y * aspect * 2f, cameraWorldSize.y * 2f, 1f);
        }
    }
}