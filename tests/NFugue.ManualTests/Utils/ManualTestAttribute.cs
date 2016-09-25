using System;

namespace NFugue.ManualTests.Utils
{
    public class ManualTestAttribute : Attribute
    {
        public ManualTestAttribute(string title, string description = "")
        {
            Title = title;
            Description = description;
        }

        public string Title { get; set; }
        public string Description { get; set; }
    }
}
