using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//HPバーなどを追従させるスクリプト
public class UIFollowTarget : MonoBehaviour
{
    RectTransform rectTransform = null;
    [SerializeField] Transform target = null;

    public float minX = 0;
    public float minY = 0;
    public float maxX = 0;
    public float maxY = 0;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()//recttransformを毎フレームプレイヤーに追従させる
    {
        
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);
        rectTransform.localPosition = new Vector3(Mathf.Clamp(gameObject.transform.localPosition.x, minX, maxX),
            Mathf.Clamp(gameObject.transform.localPosition.y, minY, maxY), 0f);
    }
}