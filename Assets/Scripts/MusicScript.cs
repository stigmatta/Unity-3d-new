using UnityEngine;

public class MusicScript : MonoBehaviour
{
    private AudioSource music;

    private static MusicScript instance;

    void Start()
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

        music = GetComponent<AudioSource>();
        GameState.AddListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == nameof(GameState.musicVolume))
        {
            music.volume = GameState.musicVolume;
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            GameState.RemoveListener(OnGameStateChanged);
        }
    }
}