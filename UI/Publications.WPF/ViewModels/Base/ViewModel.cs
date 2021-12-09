using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Publications.WPF.ViewModels.Base;

public abstract class ViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null!) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));

    //protected virtual void OnPropertyChanged<T, TProperty>(Expression<Func<T, TProperty>> expr)
    //    where T : ViewModel
    //{
    //    if (expr.Body is not MemberExpression member)
    //        throw new ArgumentException("В выражении можно исопльзовать только оператор доступа к свойствам");

    //    var property_name = member.Member.Name;
    //    OnPropertyChanged(property_name);
    //}

    protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null!)
    {
        if (Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(PropertyName);
        return true;
    }
}