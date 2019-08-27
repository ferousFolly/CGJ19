using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.Events;

public class OnPlayerLand : MonoBehaviour {
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private UnityEvent onLanding;

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log($"collision with {other} on layer: {other.gameObject.layer}");
        if ((groundLayer & 1 << other.gameObject.layer) == 1 << other.gameObject.layer) {
            Debug.Log("landing event");
            UnityEvent temp = onLanding;
            if (temp != null)
                temp.Invoke();
        }
    }
}