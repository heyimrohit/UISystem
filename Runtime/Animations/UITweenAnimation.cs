using Aetheriaum.UISystem.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aetheriaum.UISystem.Animations
{
    public class UITweenAnimation : IUIAnimation
    {
        public bool IsPlaying { get; private set; }
        public float Duration { get; private set; }

        private readonly VisualElement _target;
        private readonly AnimationCurve _curve;
        private readonly Action<float> _updateCallback;
        private float _currentTime;

        public event Action AnimationCompleted;

        public UITweenAnimation(VisualElement target, float duration, AnimationCurve curve, Action<float> updateCallback)
        {
            _target = target;
            Duration = duration;
            _curve = curve ?? AnimationCurve.EaseInOut(0, 0, 1, 1);
            _updateCallback = updateCallback;
        }

        public void Play()
        {
            if (IsPlaying) return;

            IsPlaying = true;
            _currentTime = 0f;

            // Start coroutine or animation system integration
            StartAnimation();
        }

        public void Stop()
        {
            IsPlaying = false;
            _currentTime = 0f;
        }

        public void SetProgress(float progress)
        {
            progress = Mathf.Clamp01(progress);
            _currentTime = progress * Duration;

            var curveValue = _curve.Evaluate(progress);
            _updateCallback?.Invoke(curveValue);
        }

        private void StartAnimation()
        {
            // This would integrate with Unity's animation system or your custom animation framework
            // For now, this is a conceptual implementation
        }
    }
}