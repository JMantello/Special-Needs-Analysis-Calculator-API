namespace Special_Needs_Analysis_Calculator.Data
{
    public class DocumentAttribute : Attribute
    {
        public int Id { get; set; }
        public string ColumnName { get; set; }
        public DocumentAttribute() { }
    }
}
