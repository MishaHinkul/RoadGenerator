using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsModel
{
  public SettingsModel()
  {
    CarSpawnTime = 1;
    StopGassStationTime = 1;
    Scale = 30;
    SpeedVisualizeAlgorithm = 0.015f;
  }

  public float CarSpawnTime { get; set; }
  public float StopGassStationTime { get; set; }
  public float Scale { get; set; }
  public bool VisualizeAlgorithm { get; set; }

  public float SpeedVisualizeAlgorithm { get; set; }
}