using System;

namespace ProjectX.Core.ArgumentRule
{
    public static class ArgumentRule
    {
        public static void NotNull<T>(T argument, string argumentName) where T : class
        {
            if (argument is null)
                throw new ArgumentNullException(argumentName);
        }

        public static void Requires(bool conditionResult, string message)
        {
            if (conditionResult == false)
                throw new ArgumentException(message);
        }
    }
}