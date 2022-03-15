namespace wedoIT.CATS.Tools.ReferenceDataGenerator.Model
{
    public class ReferenceDataCurrents
    {
        public ReferenceDataCurrents()
        {
            ValidTo = "null";
        }
        public string ReferenceDataType { get; set; }
        public string Code { get; set; }
        public string CodeName { get; set; }
        public string Code_Description { get; set; }
        public string AppLanguageId { get; set; }
        public string ValidTo { get; set; }
        public string ParameterData { get; set; }
    }
}
