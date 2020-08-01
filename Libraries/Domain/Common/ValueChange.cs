namespace Helpdesk.Domain.Common
{
    public class ValueChange
    {
        public ValueChange(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public object OldValue { get; }
        public object NewValue { get; }
    }
}