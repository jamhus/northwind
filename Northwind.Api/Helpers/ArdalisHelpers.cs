using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Northwind.Helpers
{
    public class ArdalisHelpers
    {
    }
    public sealed class FromMultiSourceAttribute : Attribute, IBindingSourceMetadata
    {
        public BindingSource BindingSource { get; } = CompositeBindingSource.Create(
            [BindingSource.Path, BindingSource.Query],
            nameof(FromMultiSourceAttribute));
    }
}
