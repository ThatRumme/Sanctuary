using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class EventManager : MonoBehaviour {

    //Game
    public static event Action ResetLevel;
    public static event Action ApplySettings;
    public static event Action<int> CreaturesValueUpdated;
    public static event Action<int> ItemValueUpdated;
    public static event Action PlayerDeath;
    public static event Action<int> PlayerHealthUpdated;


    public static void OnResetLevel()
    {
        ResetLevel?.Invoke();
    }

    public static void OnSettingsApplied()
    {
        ApplySettings?.Invoke();
    }

    public static void OnCreaturesValueUpdated(int value)
    {
        CreaturesValueUpdated?.Invoke(value);
    }

    public static void OnItemValueUpdated(int value)
    {
        ItemValueUpdated?.Invoke(value);
    }

    public static void OnPlayerDied()
    {
        PlayerDeath?.Invoke();
    }
    public static void OnPlayerHealthUpdated(int newHealth)
    {
        PlayerHealthUpdated?.Invoke(newHealth);

    }

}


