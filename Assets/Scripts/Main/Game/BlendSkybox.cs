using System.Collections;
using UnityEngine;

public class BlendSkybox : MonoBehaviour
{
    [SerializeField] Material _skyMat = default;
    [SerializeField, Range(0f, 1f)] float _rate = 0.05f;
    Coroutine _coroutine = default;
    Material _runtimeMaterial = default;


    private void Start()
    {
        FadeSkybox();
    }
    public void FadeSkybox()
    {
        if (_coroutine == null)
        {
            _runtimeMaterial = Instantiate(_skyMat);
            RenderSettings.skybox = _runtimeMaterial;
            _coroutine = StartCoroutine(FadeSkyboxRoutine(_rate));
        }
    }

    IEnumerator FadeSkyboxRoutine(float rate)
    {
        float blend = 0f;

        while (blend < 1)
        {
            _runtimeMaterial.SetFloat("_Blend", blend);
            blend += rate;
            yield return null;
        }

        _runtimeMaterial.SetFloat("_Blend", 1);
        Debug.Log("Fade Finished.");
        _coroutine = null;
    }
}