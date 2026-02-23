using System;

public interface IFishingMinigame
{
    event Action OnStart;
    event Action<bool> OnHandle;
    event Action OnFinish;

    void Start();
    bool Handle(bool result); // View gọi vào
}
