using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TransformObserver
{

    void OnNotify(string gameEvent, Vector3 position);

}
