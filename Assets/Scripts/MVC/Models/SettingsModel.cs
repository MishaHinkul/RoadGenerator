using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Все доступные настройки для игры
/// </summary>
public class SettingsModel
{
    /// <summary>
    /// Периодичность с которой будут появлятся машины 
    /// </summary>
    public float carSpawnTime;

    /// <summary>
    /// Время остановки на заправке
    /// </summary>
    public float stopGassStationTime;

    public float scale;

    public SettingsModel()
    {
        carSpawnTime = 3;
        stopGassStationTime = 3;
        scale = 25;
    }

}
