﻿namespace ApplicationParser
{
    public class ArtifactDef
    {
        private string _name;
        public string Name
        {
            get { return EncodeName(_name?.Replace(" ", string.Empty) ?? string.Empty); }
            set { _name = value; }
        }
        public string Guid { get; set; }

        //move to utils class?
        public static string EncodeName(string text)
        {
            return text
                .Replace("%", "Percent")
                .Replace("::", "_");
        }
    }
}
