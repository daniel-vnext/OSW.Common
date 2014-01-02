namespace OSW.Common.Utilities
{
    using System;

    public class FluentWrapper<T>
    {
        private readonly T value;

        private bool invert;

        public FluentWrapper(T value)
        {
            this.value = value;
            this.invert = false;
        }

        public FluentWrapper<T> Not
        {
            get
            {
                this.invert = !this.invert;
                return this;
            }
        }

        public bool Evaluate(Func<T, bool> predicate)
        {
            return this.invert ? !predicate(this.value) : predicate(this.value);
        }
    }
}