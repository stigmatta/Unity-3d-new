using UnityEngine;

public class EffectsScript : MonoBehaviour
{
    private AudioSource keyCollectedInTimeSound;
    private AudioSource keyCollectedOutOfTimeSound;
    private AudioSource batteryCollectedSound;

    private static EffectsScript instance; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
            return;
        }

        InitializeAudioSources();
    }

    void Start()
    {
        GameEventSystem.Subscribe(OnGameEvent);
        GameState.AddListener(OnGameStateChanged);
        OnGameStateChanged(nameof(GameState.effectsVolume));
    }

    private void InitializeAudioSources()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length >= 3)
        {
            keyCollectedInTimeSound = audioSources[0];
            batteryCollectedSound = audioSources[1];
            keyCollectedOutOfTimeSound = audioSources[2];
        }
        else
        {
            Debug.LogError("EffectsScript: Not enough AudioSources attached!");
        }
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
            float volume = GameState.effectsVolume;
            keyCollectedInTimeSound.volume = volume;
            batteryCollectedSound.volume = volume;
            keyCollectedOutOfTimeSound.volume = volume;
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            GameEventSystem.Unsubscribe(OnGameEvent);
            GameState.RemoveListener(OnGameStateChanged);
        }
    }
}