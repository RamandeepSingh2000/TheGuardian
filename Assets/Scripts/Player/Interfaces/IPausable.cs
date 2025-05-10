using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPausable
{
    void OnGamePaused();
    void OnGameResumed();
    bool TriggerOnGamePaused { get; set; }
}
