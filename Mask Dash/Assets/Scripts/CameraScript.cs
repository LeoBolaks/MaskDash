using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Shader fishEyeShader;
    public RenderTexture output;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.current != null)
        {
            Camera.current.targetTexture = output;

            Camera.current.RenderWithShader(fishEyeShader, "RenderType");

            Camera.current.targetTexture = null;
        }
    }
}
