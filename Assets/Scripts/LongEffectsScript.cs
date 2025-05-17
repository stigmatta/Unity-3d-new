using UnityEngine;

public class LongEffectsScript : MonoBehaviour
{
    private AudioSource openingSound1;
    private AudioSource openingSound2;

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length > 0)
        {
            openingSound1 = audioSources[0];
            if(audioSources.Length > 1)
            {
                openingSound2 = audioSources[1];
            }
        }
        GameState.AddListener(OnGameStateChanged);
    }
    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == nameof(GameState.longEffectsVolume))
        {
            if (openingSound1 != null)
            {
                openingSound1.volume = GameState.longEffectsVolume;
            }
            if (openingSound2 != null)
            {
                openingSound2.volume = GameState.longEffectsVolume;
            }
        }
    }
    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChanged);
    }
}
