using UnityEngine;

[System.Serializable]
public class GameData
{
    public float MasterVolume = 0.5f;
    public float MusicVolume = 1.0f;
    public float EffectsVolume = 1.0f;
    public float Sensitivity = 1.0f;
    public int FOV = 70;
    public int Weight = 1920;
    public int Height = 1080;
    public bool Windowed = false;
    public bool UseMouseWheelForSlots = true;
    public KeyCode ForwardKey = KeyCode.W;
    public KeyCode BackKey = KeyCode.S;
    public KeyCode LeftKey = KeyCode.A;
    public KeyCode RightKey = KeyCode.D;
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode SprintKey = KeyCode.LeftShift;
    public KeyCode Fire_1 = KeyCode.Mouse1;
    public KeyCode Fire_2 = KeyCode.Mouse2;
    public KeyCode InventoryKey = KeyCode.Tab;
    public KeyCode InteractionKey = KeyCode.E;
    public KeyCode DropItemKey = KeyCode.Q;
    public KeyCode PauseKey = KeyCode.Escape;
    public KeyCode Slot1Key = KeyCode.Alpha1;
    public KeyCode Slot2Key = KeyCode.Alpha2;
    public KeyCode Slot3Key = KeyCode.Alpha3;
}