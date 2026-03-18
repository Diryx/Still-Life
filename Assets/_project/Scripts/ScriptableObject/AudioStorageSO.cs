using UnityEngine;

[CreateAssetMenu(fileName = "AudioStorage", menuName = "Game/Audio Storage")]
public class AudioStorageSO : ScriptableObject
{
    [Header("Music")]
    [SerializeField] private AudioClip[] _music;

    [Header("UI Sounds")]
    [SerializeField] private AudioClip[] _uiSounds;

    [Header("Game Sounds")]
    [SerializeField] private AudioClip[] _gameSounds;

    public AudioClip GetMusicClip(int index) => GetClip(_music, index);
    public AudioClip GetUISound(int index) => GetClip(_uiSounds, index);
    public AudioClip GetGameSound(int index) => GetClip(_gameSounds, index);

    private AudioClip GetClip(AudioClip[] array, int index)
    {
        if (array != null && index >= 0 && index < array.Length)
            return array[index];
        return null;
    }
}