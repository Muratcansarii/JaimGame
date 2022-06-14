// This file stands for arranging ui components for Saving Part of the Jaim Game.

using UnityEngine;

namespace JAIM.Saving  // this namespace holds attributes about Saving
{
    [System.Serializable] // Indicates that a class or a struct can be serialized.
    public class SerializableVector3
    {
        float x, y, z; // defining every axis, x-axis, y-axis and z-axis

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x; // assigning x axis to vector.x
            y = vector.y; // assigning y axis to vector.y
            z = vector.z; // assigning z axis to vector.z
        }

        public Vector3 ToVector()  // Vector3  is a representation of 3D vectors and points.
        {
            return new Vector3(x, y, z);
        }
    }
}
