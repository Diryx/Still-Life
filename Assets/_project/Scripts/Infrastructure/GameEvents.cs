using System;
using Zenject;

public class GameEvents : IInitializable, IDisposable
{
    public event Action OnUIOpened;
    public event Action OnUIClosed;
    public event Action OnPause;
    public event Action OnResume;

    public void UIOpened() => OnUIOpened?.Invoke();
    public void UIClosed() => OnUIClosed?.Invoke();
    public void Paused() => OnPause?.Invoke();
    public void Resume() => OnResume?.Invoke();

    public void Initialize() { }
    public void Dispose() { }
}