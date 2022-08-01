using System;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    public static Action OnFinishLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnFinishLevel?.Invoke();
    }
}