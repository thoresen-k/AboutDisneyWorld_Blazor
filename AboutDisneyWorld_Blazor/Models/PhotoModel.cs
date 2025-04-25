namespace AboutDisneyWorld_Blazor.Models
{
    record PhotoModel
    {
        public required string Href { get; init; }

        public required string  Title { get; init; }

        public required string Caption { get; init; }

        public DateTime Date { get; init; }
    }
}