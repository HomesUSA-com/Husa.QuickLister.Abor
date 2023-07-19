namespace Husa.Quicklister.Abor.Crosscutting.Extensions
{
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class CustomDescriptionAttribute : Attribute
    {
        public CustomDescriptionAttribute()
        {
        }

        public CustomDescriptionAttribute(string description)
        {
            this.Description = description;
        }

        public string Description { get; set; }

        public override string ToString()
        {
            return this.Description;
        }
    }
}
