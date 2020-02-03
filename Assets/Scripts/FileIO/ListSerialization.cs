using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListSerialization<T>
{
    [SerializeField] List<T> _target;
    public List<T> ToList() { return _target; }

    public ListSerialization(List<T> target)
    {
        this._target = target;
    }
}
