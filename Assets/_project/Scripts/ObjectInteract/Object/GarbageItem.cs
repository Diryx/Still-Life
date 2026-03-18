using UnityEngine;

public class GarbageItem : BaseInteractable
{
    [SerializeField] private GameObject _pickupEffect;
    [SerializeField] private AudioClip _clip;

    public override void Interact()
    {
        if (_pickupEffect != null && _clip != null)
        {
            Instantiate(_pickupEffect, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_clip, transform.position);
        }
            
        Destroy(gameObject);
    }
}