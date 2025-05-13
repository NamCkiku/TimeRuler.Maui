using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRulers.Maui.Controls
{
    public class TimeRulerDrawable : IDrawable
    {
        private readonly TimeRulerView view;
        public const int MAX_TIME_VALUE = 24 * 3600;

        public int[] PerTextCounts = {
            60, 60, 2 * 60, 4 * 60,
            5 * 60, 10 * 60,
            20 * 60, 30 * 60,
            3600, 2 * 3600, 3 * 3600, 4 * 3600, 5 * 3600, 6 * 3600
        };

        public TimeRulerDrawable(TimeRulerView view)
        {
            this.view = view;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            // Background
            canvas.FillColor = view.BackgroundColor;
            canvas.FillRectangle(dirtyRect);

            // Rule
            DrawRule(canvas, dirtyRect);

            // Time parts
            DrawTimeParts(canvas, dirtyRect);

            // Indicator
            DrawTimeIndicator(canvas, dirtyRect);

            canvas.RestoreState();
        }

        private void DrawRule(ICanvas canvas, RectF rect)
        {
            canvas.SaveState();
            canvas.Translate(0, view.PartHeight);

            canvas.StrokeColor = view.GradationColor;
            canvas.StrokeSize = view.GradationWidth;

            int start = 0;
            float offset = rect.Width / 2 - view.currentDistance;
            int perTextCount = PerTextCounts[view.perTextCountIndex];

            while (start <= MAX_TIME_VALUE)
            {
                if (start % 3600 == 0)
                    canvas.DrawLine(offset, 0, offset, view.HourLen);
                else if (start % 60 == 0)
                    canvas.DrawLine(offset, 0, offset, view.MinuteLen);
                else
                    canvas.DrawLine(offset, 0, offset, view.SecondLen);

                if (start % perTextCount == 0)
                {
                    canvas.FontColor = view.GradationTextColor;
                    canvas.FontSize = view.GradationTextSize;
                    string text = FormatTimeHHmm(start);
                    canvas.DrawString(text, offset - view.textHalfWidth, view.HourLen + view.GradationTextGap, HorizontalAlignment.Left);
                }

                start += view.unitSecond;
                offset += view.unitGap;
            }

            canvas.RestoreState();
        }

        private void DrawTimeIndicator(ICanvas canvas, RectF rect)
        {
            canvas.StrokeColor = view.IndicatorColor;
            canvas.StrokeSize = view.IndicatorWidth;
            canvas.DrawLine(rect.Width / 2, 0, rect.Width / 2, rect.Height);

            float halfSideLen = view.IndicatorTriangleSideLen / 2f;
            var path = new PathF();
            path.MoveTo(rect.Width / 2 - halfSideLen, 0);
            path.LineTo(rect.Width / 2 + halfSideLen, 0);
            path.LineTo(rect.Width / 2, (float)(Math.Sin(Math.PI / 3) * halfSideLen));
            path.Close();

            canvas.FillColor = view.IndicatorColor;
            canvas.FillPath(path);
        }

        private void DrawTimeParts(ICanvas canvas, RectF rect)
        {
            if (view.timePartList == null) return;

            canvas.StrokeSize = view.PartHeight;
            canvas.StrokeColor = view.PartColor;
            float halfPartHeight = view.PartHeight / 2f;
            float secondGap = view.unitGap / view.unitSecond;

            foreach (var part in view.timePartList)
            {
                float start = rect.Width / 2 - view.currentDistance + part.StartTime * secondGap;
                float end = rect.Width / 2 - view.currentDistance + part.EndTime * secondGap;
                canvas.DrawLine(start, halfPartHeight, end, halfPartHeight);
            }
        }

        private string FormatTimeHHmm(int timeValue)
        {
            if (timeValue < 0) timeValue = 0;
            int hour = timeValue / 3600;
            int minute = timeValue % 3600 / 60;
            return $"{hour:D2}:{minute:D2}";
        }

    }
}