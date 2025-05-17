using UnityEngine;

public class EffectsScript : MonoBehaviour
{
    private AudioSource keyCollectedInTimeSound;
    private AudioSource keyCollectedOutOfTimeSound;
    private AudioSource batteryCollectedSound;
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        keyCollectedInTimeSound = audioSources[0];
        batteryCollectedSound = audioSources[1];
        keyCollectedOutOfTimeSound = audioSources[2];
        GameEventSystem.Subscribe(OnGameEvent);
        GameState.AddListener(OnGameStateChanged);
        OnGameStateChanged(nameof(GameState.effectsVolume));
    }

    private void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent.sound != null)
        {
            switch (gameEvent.sound)
            {
                case EffectsSounds.KeyCollectedInTime:
                    keyCollectedInTimeSound.Play();
                    break;
                case EffectsSounds.BatteryCollected:
                    batteryCollectedSound.Play();
                    break;
                case EffectsSounds.KeyCollectedOutOfTime:
                    keyCollectedOutOfTimeSound.Play();
                    break;
                default:
                    Debug.LogWarning($"Unknown sound effect: {gameEvent.sound}");
                    break;
            }
        }
    }

    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == null || fieldName == nameof(GameState.effectsVolume))
        {
            keyCollectedInTimeSound.volume = GameState.effectsVolume;
            batteryCollectedSound.volume = GameState.effectsVolume;
            keyCollectedOutOfTimeSound.volume = GameState.effectsVolume;
        }
    }

    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
        GameState.RemoveListener(OnGameStateChanged);

    }
}
