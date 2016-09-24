using Xunit;

namespace NFugue.ManualTests.Utils
{
    public class ManualTestAttribute : FactAttribute
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
