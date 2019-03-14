using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Все доступные настройки для игры
/// </summary>
public class SettingsModel
{
  public SettingsModel()
  {
    CarSpawnTime = 1;
    StopGassStationTime = 1;
    Scale = 30;
  }

  /// <summary>
  /// Периодичность с которой будут появлятся машины 
  /// </summary>
  public float CarSpawnTime { get; set; }

  /// <summary>
  /// Время остановки на заправке
  /// </summary>
  public float StopGassStationTime { get; set; }

  public float Scale { get; set; }
}