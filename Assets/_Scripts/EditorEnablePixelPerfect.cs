using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(PixelPerfectCamera))]
[ExecuteInEditMode]
public class EditorEnablePixelPerfect : MonoBehaviour
{
#if UNITY_EDITOR
    public PixelPerfectCamera PixelPerfectCam;
    private const string _log = "Pixel Perfect Camera Editor mode Activated";

    private void Awake()
    {
        if (Application.isPlaying == false && PixelPerfectCam != null && PixelPerfectCam.runInEditMode == false)
        {
            PixelPerfectCam.runInEditMode = true;
            Debug.Log(_log);
        }
    }
#endif
}