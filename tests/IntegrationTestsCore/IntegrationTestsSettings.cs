namespace IntegrationTestsCore
{
    public class IntegrationTestsSettings
    {
        public string DefaultUserPassword { get; set; } = "Aw23_ffdD3df_ddw!efefdewww";

        public string DefaultUserName { get; set; } = "a@bmail.com";

        public string Version { get; set; } = "v1";

        public string Environment { get; set; } = "Development";

        public string BaseAddress { get; set; } = @"http://localhost:5000";

        public DumpSettings Dump { get; set; }
    }
}