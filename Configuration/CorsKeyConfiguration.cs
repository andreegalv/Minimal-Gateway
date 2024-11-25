namespace SigalNET.Gateway.Configuration
{
    public class CorsKeyConfiguration
    {
        public required string Name { get; set; }

#pragma warning disable CA2227 // Collection properties should be read only
        public required ICollection<string> Origins { get; set; } = [];

        public required ICollection<string> Methods { get; set; } = [];

        public required ICollection<string> Headers { get; set; } = [];

        public ICollection<string>? ExposedHeaders { get; set; } = [];
#pragma warning restore CA2227 // Collection properties should be read only

        public bool AllowAnyOrigin()
        {
            return this.Origins.Count == 1 && string.Equals(this.Origins.First(), "*", StringComparison.OrdinalIgnoreCase);
        }

        public bool AllowAnyMethod()
        {
            return this.Methods.Count == 1 && string.Equals(this.Methods.First(), "*", StringComparison.OrdinalIgnoreCase);
        }

        public bool AllowAnyHeader()
        {
            return this.Headers.Count == 1 && string.Equals(this.Headers.First(), "*", StringComparison.OrdinalIgnoreCase);
        }
    }
}
