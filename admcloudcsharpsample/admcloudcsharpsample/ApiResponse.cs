namespace csharpsample
{
    internal class ApiResponse
    {
        public bool success { get; set; }

        public string message { get; set; } = "";

        public object? data { get; set; }
    }
}