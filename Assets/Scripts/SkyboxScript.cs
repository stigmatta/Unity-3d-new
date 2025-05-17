using UnityEngine;

public class SkyboxScript : MonoBehaviour
{
    [SerializeField] private Material daySkybox;
    [SerializeField] private Material nightSkybox;
    void Start()
    {
        GameState.AddListener(OnGameStateChange);
        RenderSettings.skybox = GameState.isDay ? daySkybox : nightSkybox;
    }

    private void OnGameStateChange(string fieldName)
    {
        if (fieldName == nameof(GameState.isDay))
        {
            RenderSettings.skybox = GameState.isDay ? daySkybox : nightSkybox;
        }

    }

    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChange);
    }
}
