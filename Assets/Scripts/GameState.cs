using System.Collections.Generic;
using System;
using UnityEngine;

public class GameState 
{
    #region float effectsVolume
    private static float _effectsVolume = 0.2f;
    public static float effectsVolume
    {
        get => _effectsVolume;
        set
        {
            if (_effectsVolume != value)
            {
                _effectsVolume = value;
                Notify(nameof(effectsVolume));
            }

        }
    }
    #endregion

    #region float longEffectsVolume
    private static float _longEffectsVolume = 0.1f;
    public static float longEffectsVolume
    {
        get => _longEffectsVolume;
        set
        {
            if (_longEffectsVolume != value)
            {
                _longEffectsVolume = value;
                Notify(nameof(longEffectsVolume));
            }

        }
    }
    #endregion

    #region float musicVolume
    private static float _musicVolume = 0.119f;
    public static float musicVolume
    {
        get => _musicVolume;
        set
        {
            if (_musicVolume != value)
            {
                _musicVolume = value;
                Notify(nameof(musicVolume));
            }

        }
    }
    #endregion
    #region bool isDay
    private static bool _isDay = true;
    public static bool isDay
    {
        get => _isDay;
        set
        {
            if (_isDay != value)
            {
                _isDay = value;
                Notify(nameof(isFpv));
            }

        }
    }
    #endregion
    #region bool isFpv
    private static bool _isFpv = true;
    public static bool isFpv
    {
        get => _isFpv;
        set
        {
            if (_isFpv != value)
            {
                _isFpv = value;
                Notify(nameof(isFpv));
            }

        }
    }
    #endregion

    #region Change Notifier
    private static List<Action<string>> listeners = new List<Action<string>>();
    public static void AddListener(Action<string> listener)
    {
        listeners.Add(listener);
        listener(null);
    }

    public static void RemoveListener(Action<string> listener)
    {
        if (listener != null && listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }

    public static void Notify(string fieldName)
    {
        foreach (Action<string> listener in listeners)
        {
            listener.Invoke(fieldName);
        }
    }
    #endregion

    public static void SetProperty(string name,object value)
    {
        var prop = typeof(GameState).GetProperty(name, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        if(prop == null)
        {
            Debug.LogError($"Property {name} not found in GameState");
            return;
        }
        else
            prop.SetValue(null, value);
    }
}
