﻿using System;

namespace XRMS.Libraries.MVVM
{
    public class CommandBase<T> : Cinch.SimpleCommand<T,T>
    {
        public CommandBase(Action<T> executeMethod, Func<T, bool> canExecuteMethod) : base(canExecuteMethod, executeMethod)
        {

        }
        public CommandBase(Action<T> executeMethod) : base(executeMethod)
        {

        }
    }
}
