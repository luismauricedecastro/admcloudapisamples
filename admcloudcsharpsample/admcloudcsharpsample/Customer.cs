namespace csharpsample
{
    internal class Customer
    {
        public Guid ID { get; set; }

        public string Name { get; set; } = "";

        public bool IsCustomer { get; set; }

        public Guid? PriceLevelID { get; set; }

        public string CurrencyID { get; set; } = "";
    }
}