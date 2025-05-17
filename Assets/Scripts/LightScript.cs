using System.Linq;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    private Light[] dayLights;
    private Light[] nightLights;

    void Start()
    {
        dayLights = GameObject.FindGameObjectsWithTag("Day")
            .SelectMany(go => go.GetComponentsInChildren<Light>())
            .Where(l => l != null)
            .ToArray();

        nightLights = GameObject.FindGameObjectsWithTag("Night")
            .SelectMany(go => go.GetComponentsInChildren<Light>())
            .Where(l => l != null)
            .ToArray();


        GameState.isDay = true;

        GameState.AddListener(OnGameStateChange);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameState.isDay = !GameState.isDay;
            GameState.Notify(nameof(GameState.isDay));
        }
    }

    private void ToggleLights()
    {
        if (GameState.isDay)
        {
            foreach (Light light in dayLights)
            {
                light.intensity = 1.0f;
                light.enabled = true;
            }

            foreach (Light light in nightLights)
            {
                light.intensity = 0.0f;
                light.enabled = false;
            }

            RenderSettings.ambientIntensity = 1.0f;
            RenderSettings.ambientLight = Color.white; 
        }
        else
        {
            foreach (Light light in dayLights)
            {
                light.intensity = 0.0f;
                light.enabled = false;
            }

            foreach (Light light in nightLights)
            {
                if (!GameState.isFpv)
                {
                    light.intensity = 1f;
                    light.enabled = true;
                }
            }

            RenderSettings.ambientIntensity = 0.1f; 
            RenderSettings.ambientLight = Color.gray; 
        }
    }
 

    private void FpvChanged()
    {
        if (!GameState.isDay)
        {
            foreach (Light light in nightLights)
            {
                light.intensity = GameState.isFpv ? 0.0f : 1f;
                light.enabled = !GameState.isFpv;
            }
        }
    }

    private void OnGameStateChange(string fieldName)
    {
        if (fieldName == nameof(GameState.isDay))
        {
            ToggleLights();
        }
        else if (fieldName == nameof(GameState.isFpv))
        {
            FpvChanged();
        }
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChange);
    }
}
