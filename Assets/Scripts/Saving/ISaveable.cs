// This file has Isaveable feature, we are using this in every part of Saving part of our game

namespace JAIM.Saving // this namespace holds attributes about Saving
{
    public interface ISaveable // defining ISaveable  and its capturestate and restorestate behaviours.
    {
        object CaptureState(); // captures the current state
        void RestoreState(object state); // Restoring the current state
    }
}