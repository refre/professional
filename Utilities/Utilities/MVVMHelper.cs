using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ista.Utilities.MVVM
{
    public static class MVVMHelper
    {
        public static void Raise<T, P>(this PropertyChangedEventHandler pc, T source, Expression<Func<T, P>> pe)
        {
            if (pc != null)
                pc.Invoke(source, new PropertyChangedEventArgs(((MemberExpression)pe.Body).Member.Name));
        }
    }
}
