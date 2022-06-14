// FollowCamera.cs file stands for handling camer's behavior of following targeted gameobject in JaimGame

// Adding namespaces that we keep in safe 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JAIM.Core // this namespace holds attributes about Core
{
    public class FollowCamera : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
    [SerializeField] Transform target; // SerializedField allow us to make a copy of our created variables in unity engine

    void LateUpdate() // lateupdate start to work after all other update functions has been called.
        {
        transform.position = target.position; // makes it equal to transform positon to target's position
    }
}

}