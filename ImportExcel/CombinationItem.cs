namespace ImportExcel
{
    public class CombinationItem
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public string Value { get; set; }
    }

    public class CombinationWithPrice
    {
        public List<CombinationItem> Combinations { get; set; } = new List<CombinationItem>();
        public decimal Price { get; set; }
    }
}