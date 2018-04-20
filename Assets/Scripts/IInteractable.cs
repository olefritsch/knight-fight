
public interface IInteractable 
{
    bool IsUsed { get; }

    void Interact();

    void StopInteracting();

    void Use();
}
