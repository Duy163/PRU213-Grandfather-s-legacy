using System;

public static class StoryEvent
{
    public static Action<NPCData, StoryDatabase> OnStartDialogue;
    public static Action OnBeginDialogue;
}