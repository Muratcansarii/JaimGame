namespace JAIM.Control // this namespace holds attributes about control
{   // this block of code stands for raycasting to the controller
    public interface IRaycastable
    {
    CursorType GetCursorType();
    bool HandleRaycast(PlayerController callingController);
}
}


