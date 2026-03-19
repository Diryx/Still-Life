using UnityEngine;

namespace Infrastructure.SO
{
    [CreateAssetMenu(fileName = "ImageStorage", menuName = "Game/Image Storage")]
    public class ImageStorageSO : ScriptableObject
    {
        [SerializeField] private Sprite[] _noteSprites;

        public Sprite GetNoteSprite(int index)
        {
            if (index >= 0 && index < _noteSprites.Length)
                return _noteSprites[index];
            return null;
        }
    }
}