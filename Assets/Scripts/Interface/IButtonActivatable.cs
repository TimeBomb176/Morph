using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IButtonActivatable
{
    public void ActivateWithButton();

    public Transform GetTransform();
}
