// CameraFacing.cs file stands for handling the facing feature of camera in JaimGame

// Adding namespaces that we keep in safe 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JAIM.Core // this namespace holds attributes about Core
{
    public class CameraFacing : MonoBehaviour // Monobehaviour is the base class for all created component in unity
    {
        void LateUpdate() // lateupdate start to work after all other update functions has been called.
        {
            transform.forward = Camera.main.transform.forward; // arranging camera's transform
        }
    }
} 