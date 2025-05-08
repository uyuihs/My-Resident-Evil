using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MagicNumberSetting {
    public float lowerEps = 0.05f;//小于该值为0
    public float upperEps = 0.5f;//小于该值为0.5,大于该值为1
    public float zero = (float)1e-22;//表示0
}
