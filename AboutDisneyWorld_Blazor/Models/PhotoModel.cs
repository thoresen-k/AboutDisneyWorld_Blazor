namespace AboutDisneyWorld_Blazor.Models
{
    public class PhotoModel(string href, string title, string caption, DateTime date)
    {
        public string Href => href;

        public string Title => title;

        public string Caption => caption;

        public DateTime Date => date;
    }
}