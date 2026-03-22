using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PowerManager : MonoBehaviour
{
    [SerializeField]
    private Slider _powerSlider;

    private Action<float> OnUpdatePower;
    private Tween powerTween;
    private float _maxPower = 20f;
    private float currentPower;
    private bool IsCharging;

    public void StartPowerMode(Action<float> onUpdatePower)
    {
        powerTween?.Kill();
        OnUpdatePower = onUpdatePower;
        powerTween = DOTween.To(
            () => 5f,
            x => currentPower = x,
            20f,
            2f
        )
        .SetEase(Ease.InOutSine)
        .SetLoops(-1, LoopType.Yoyo);
        IsCharging = true;
    }

    private void Update()
    {
        if (IsCharging == false)
        {
            return;
        }
        Debug.Log("currentPower :" + currentPower);
        _powerSlider.value = currentPower;
        OnUpdatePower?.Invoke(currentPower);
    }

    public void OnStop()
    {
        IsCharging = false;
    }
}
