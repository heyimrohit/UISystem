using System.Collections.Generic;
using UnityEngine;

namespace Aetheriaum.UISystem.Audio
{
    public class UISoundManager
    {
        private readonly Dictionary<string, AudioClip> _soundClips = new();
        private AudioSource _audioSource;

        public void Initialize(AudioSource audioSource)
        {
            _audioSource = audioSource;
            LoadUISounds();
        }

        public void PlaySound(string soundName)
        {
            if (_soundClips.TryGetValue(soundName, out var clip) && _audioSource != null)
            {
                _audioSource.PlayOneShot(clip);
            }
        }

        private void LoadUISounds()
        {
            // Load UI sound effects
            _soundClips["button_click"] = Resources.Load<AudioClip>("Audio/UI/button_click");
            _soundClips["panel_open"] = Resources.Load<AudioClip>("Audio/UI/panel_open");
            _soundClips["panel_close"] = Resources.Load<AudioClip>("Audio/UI/panel_close");
            _soundClips["navigation"] = Resources.Load<AudioClip>("Audio/UI/navigation");
            _soundClips["error"] = Resources.Load<AudioClip>("Audio/UI/error");
            _soundClips["success"] = Resources.Load<AudioClip>("Audio/UI/success");
        }
    }
}