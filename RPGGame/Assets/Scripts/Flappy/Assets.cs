using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assets : MonoBehaviour {
    private static Assets instance;

    public static Assets GetInstance() {
        return instance;
    }

    void Awake() {
        instance = this;
    }

    public Sprite pipeHead;
    public Transform transPipeHead;
    public Transform transPipeBody;
}
