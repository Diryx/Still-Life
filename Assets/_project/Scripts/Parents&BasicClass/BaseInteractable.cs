using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour
{
    [SerializeField] protected string _prompt = "E";
    public virtual string InteractionPrompt => _prompt;
    public virtual bool CanInteract => true;
    public abstract void Interact();
}