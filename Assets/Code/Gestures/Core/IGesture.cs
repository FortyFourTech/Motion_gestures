using System;
using System.Collections;
using UnityEngine;

public interface IGesture
{
    event Action onFire;
    event Action onStart;
    event Action onBrake;

    // Start this in coroutine
    IEnumerator StartReceiving();
}
