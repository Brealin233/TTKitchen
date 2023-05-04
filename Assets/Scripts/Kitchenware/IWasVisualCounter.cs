using System;

public interface IWasVisualCounter
{
    public event EventHandler<counterVisualEventClass> counterVisualEvent;

    public class counterVisualEventClass : EventArgs
    {
        public float fillAmount;
    }
}
