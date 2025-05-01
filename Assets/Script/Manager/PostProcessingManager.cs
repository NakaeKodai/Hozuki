using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    public Volume volume;
    private ColorAdjustments colorGradingLayer;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance.MaxHP != 0)
        {
            volume.profile.TryGet(out colorGradingLayer);

            float hpRatio = (float)GameManager.instance.HP / GameManager.instance.MaxHP;
            Debug.Log(hpRatio);
            Debug.Log(1 - hpRatio);

            // HPが減るほどコントラストを上げていく（0〜50）
            colorGradingLayer.contrast.value = Mathf.Lerp(0f, 50f, 1f - hpRatio);

            // HPが減るほど彩度を下げていく（0〜-50）
            colorGradingLayer.saturation.value = Mathf.Lerp(0f, -50f, 1f - hpRatio);
        }
    }

}
