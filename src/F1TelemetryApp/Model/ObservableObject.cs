namespace F1TelemetryApp.Model;

using System.ComponentModel;

public abstract class ObservableObject<T> : INotifyPropertyChanged
{
    public ObservableObject(T value)
    {
        _value = value;
    }

    private T _value;
    public T Value
    {
        get => _value;
        set
        {
            if (!IsEqual(_value, value))
            {
                _value = value;
                NotifyPropertyChanged();
            }
        }
    }

    protected abstract bool IsEqual(T currentValue, T newValue);

    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class ObservableString : ObservableObject<string>
{
    public ObservableString(string value = "") : base(value)
    {
    }

    protected override bool IsEqual(string currentValue, string newValue)
    {
        return currentValue == newValue;
    }
}
