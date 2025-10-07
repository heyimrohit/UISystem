using System;

namespace Aetheriaum.UISystem.Interfaces
{
    public interface IUIAnimation
    {
        bool IsPlaying { get; }
        float Duration { get; }

        void Play();
        void Stop();
        void SetProgress(float progress);

        event Action AnimationCompleted;
    }
}