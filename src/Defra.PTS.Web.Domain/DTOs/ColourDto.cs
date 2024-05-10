namespace Defra.PTS.Web.Domain.DTOs
{
    public class ColourDto
    {
        public string Id { get; set; }

        public string Code
        {
            get
            {
                return ToCode(Name);
            }
        }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }


        private static string ToCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            value = value.Trim();
            value = value.Replace(@" ", string.Empty);
            value = value.Replace(@")", string.Empty);
            value = value.Replace(@"(", "_");
            value = value.Replace(@"[", "_");
            value = value.Replace(@"]", "_");
            value = value.Replace(@",", "_");
            value = value.Replace(@".", string.Empty);
            value = value.Replace(@"/", "_");
            value = value.Replace(@"\", "_");
            value = value.Replace(@"#", "_");
            value = value.Replace(@"?", "_");
            value = value.Replace(@"\t", string.Empty);
            value = value.Replace(@"\n", string.Empty);
            value = value.Replace(@"\r", string.Empty);

            return value;
        }
    }

}
