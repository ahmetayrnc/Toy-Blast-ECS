using System;
using UnityEngine;


[CreateAssetMenu]
public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
{
    public int initialValue;
    [NonSerialized] private int _runTimeValue;

    public void OnBeforeSerialize()
    {
    }

    public int RunTimeValue
    {
        get => _runTimeValue;
        set => _runTimeValue = value;
    }

    public void OnAfterDeserialize()
    {
        _runTimeValue = initialValue;
    }
    
    public static implicit operator int(IntVariable variable)
    {
        return variable._runTimeValue;
    }
}