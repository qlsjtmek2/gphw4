using UnityEngine;
using DG.Tweening;

public class OpacityController : MonoBehaviour
{
    private Material material;

    private void Start()
    {
        // Material 가져오기
        Renderer renderer = GetComponent<Renderer>();
        material = renderer.material;
    }

    // 불투명도를 변경하는 메서드
    public void FadeTo(float targetOpacity, float duration)
    {
        Color color = material.color;
        material.DOColor(new Color(color.r, color.g, color.b, targetOpacity), duration);
    }
}