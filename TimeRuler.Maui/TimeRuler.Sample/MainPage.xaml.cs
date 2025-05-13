using TimeRulers.Maui.Controls;

namespace TimeRuler.Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            timeRuler.SetOnTimeChangedListener(time =>
            Console.WriteLine($"Time changed: {TimeRulerView.FormatTimeHHmmss(time)}"));
            timeRuler.SetTimePartList(new List<TimeRulerView.TimePart>
                {
                    new TimeRulerView.TimePart { StartTime = 3600, EndTime = 7200 }
                });
            timeRuler.CurrentTime = 3600;
        }
    }

}
