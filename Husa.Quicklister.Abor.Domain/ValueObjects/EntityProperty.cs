namespace Husa.Quicklister.Abor.Domain.ValueObjects
{
    public record EntityProperty
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
    }
}
