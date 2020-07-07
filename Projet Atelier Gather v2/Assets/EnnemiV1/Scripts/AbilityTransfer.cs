using UnityEngine;

public class AbilityTransfer : MonoBehaviour
{
    [Tooltip("Event that needs to be raised in order to give uses to a power that sits on the player. The player should listen for that event abd take care of playing feedbacks and stuff.")]
    public GameEvent m_AbilityTransferEvent;

    public void Transfer()
    {
        m_AbilityTransferEvent.Raise();
    }
}
