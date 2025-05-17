using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private GameObject content;
    private bool isMuted = false;
    private Slider effectsSlider;
    private Slider musicSlider;
    private Slider longEffectsSlider;
    private Toggle muteToggle;
    private float startTimeScale;
    private float defaultMusicVolume;
    private float defaultEffectsVolume;
    private float defaultLongEffectsVolume;
    private bool defaultIsMuted;
    void Start()
    {
        GetDefaults();
        content = transform.Find("Content").gameObject;
        effectsSlider = content.transform.Find("Sounds/SliderEffects").GetComponent<Slider>();
        musicSlider = content.transform.Find("Sounds/SliderMusic").GetComponent<Slider>();
        longEffectsSlider = content.transform.Find("Sounds/SliderLongEffects").GetComponent<Slider>();
        muteToggle = content.transform.Find("Sounds/ToggleMute").GetComponent<Toggle>();
        OnMuteChanged(isMuted);
        LoadPrefs();
        startTimeScale = Time.timeScale;
        Hide();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(content.activeInHierarchy)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }
    private void Show() 
    {
        startTimeScale = Time.timeScale;
        content.SetActive(true);
        Time.timeScale = 0;
    }
    private void Hide()
    {
        content.SetActive(false);
        Time.timeScale = startTimeScale;
    }
    public void OnEffectsValueChanged(float volume)
    {
        if (!isMuted) GameState.effectsVolume = volume;
    }
    public void OnLongEffectsChanged(float volume)
    {
        if (!isMuted) GameState.longEffectsVolume = volume;
    }

    public void OnMusicValueChanged(float volume)
    {
        if(!isMuted) GameState.musicVolume = volume;
    }   

    private void GetDefaults()
    {
        defaultMusicVolume = GameState.musicVolume;
        defaultEffectsVolume = GameState.effectsVolume;
        defaultLongEffectsVolume = GameState.longEffectsVolume;
        defaultIsMuted =false;
    }

    public void LoadPrefs()
    {
        if (PlayerPrefs.HasKey("effectsVolume"))
        {
            GameState.effectsVolume = effectsSlider.value = PlayerPrefs.GetFloat("effectsVolume");
        }
        else
        {
            effectsSlider.value = defaultEffectsVolume;

        }
        if (PlayerPrefs.HasKey("longEffectsVolume"))
        {
            GameState.longEffectsVolume = longEffectsSlider.value = PlayerPrefs.GetFloat("longEffectsVolume");
        }
        else
        {
            longEffectsSlider.value = defaultLongEffectsVolume;
        }
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            GameState.musicVolume = musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            musicSlider.value = defaultMusicVolume;
        }
        if (PlayerPrefs.HasKey("isMuted"))
        {
            isMuted = muteToggle.isOn = PlayerPrefs.GetInt("isMuted") == 1;
        }
        else
        {
            isMuted = defaultIsMuted;
        }
    }

    public void OnMuteChanged(bool isMute)
    {
        isMuted = isMute;
        if (isMuted)
        {
            GameState.musicVolume = 0;
            GameState.effectsVolume = 0;
        }
        else
        {
            GameState.musicVolume = musicSlider.value;
            GameState.effectsVolume = effectsSlider.value;
            GameState.longEffectsVolume = longEffectsSlider.value;
        }
    }
    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
        Application.Quit();
#endif
    }
    public void OnDefaultsClick()
    {
        GameState.musicVolume = musicSlider.value = defaultMusicVolume;
        GameState.effectsVolume = effectsSlider.value = defaultEffectsVolume;
        GameState.longEffectsVolume = longEffectsSlider.value = defaultLongEffectsVolume;
        isMuted = muteToggle.isOn =  defaultIsMuted;

    }
    public void OnContinueClick()
    {
        Hide();
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("effectsVolume", effectsSlider.value);
        PlayerPrefs.SetFloat("longEffectsVolume", longEffectsSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
}
