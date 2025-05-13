using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRulers.Maui.Controls
{
    public class TimeRulerView : ContentView
    {
        private const bool LOG_ENABLE = false;
        public const int MAX_TIME_VALUE = 24 * 3600;

        public static readonly BindableProperty GradationColorProperty =
            BindableProperty.Create(nameof(GradationColor), typeof(Color), typeof(TimeRulerView), Colors.Gray);

        public static readonly BindableProperty PartHeightProperty =
            BindableProperty.Create(nameof(PartHeight), typeof(float), typeof(TimeRulerView), 20f);

        public static readonly BindableProperty PartColorProperty =
            BindableProperty.Create(nameof(PartColor), typeof(Color), typeof(TimeRulerView), Color.FromArgb("#F58D24"));

        public static readonly BindableProperty GradationWidthProperty =
            BindableProperty.Create(nameof(GradationWidth), typeof(float), typeof(TimeRulerView), 1f);

        public static readonly BindableProperty SecondLenProperty =
            BindableProperty.Create(nameof(SecondLen), typeof(float), typeof(TimeRulerView), 3f);

        public static readonly BindableProperty MinuteLenProperty =
            BindableProperty.Create(nameof(MinuteLen), typeof(float), typeof(TimeRulerView), 5f);

        public static readonly BindableProperty HourLenProperty =
            BindableProperty.Create(nameof(HourLen), typeof(float), typeof(TimeRulerView), 10f);

        public static readonly BindableProperty GradationTextColorProperty =
            BindableProperty.Create(nameof(GradationTextColor), typeof(Color), typeof(TimeRulerView), Colors.Gray);

        public static readonly BindableProperty GradationTextSizeProperty =
            BindableProperty.Create(nameof(GradationTextSize), typeof(float), typeof(TimeRulerView), 12f);

        public static readonly BindableProperty GradationTextGapProperty =
            BindableProperty.Create(nameof(GradationTextGap), typeof(float), typeof(TimeRulerView), 20f);

        public static readonly BindableProperty CurrentTimeProperty =
            BindableProperty.Create(nameof(CurrentTime), typeof(int), typeof(TimeRulerView), 0, propertyChanged: OnCurrentTimeChanged);

        public static readonly BindableProperty IndicatorColorProperty =
            BindableProperty.Create(nameof(IndicatorColor), typeof(Color), typeof(TimeRulerView), Colors.Red);

        public static readonly BindableProperty IndicatorTriangleSideLenProperty =
            BindableProperty.Create(nameof(IndicatorTriangleSideLen), typeof(float), typeof(TimeRulerView), 15f);

        public static readonly BindableProperty IndicatorWidthProperty =
            BindableProperty.Create(nameof(IndicatorWidth), typeof(float), typeof(TimeRulerView), 1f);

        public Color GradationColor
        {
            get => (Color)GetValue(GradationColorProperty);
            set => SetValue(GradationColorProperty, value);
        }

        public float PartHeight
        {
            get => (float)GetValue(PartHeightProperty);
            set => SetValue(PartHeightProperty, value);
        }

        public Color PartColor
        {
            get => (Color)GetValue(PartColorProperty);
            set => SetValue(PartColorProperty, value);
        }

        public float GradationWidth
        {
            get => (float)GetValue(GradationWidthProperty);
            set => SetValue(GradationWidthProperty, value);
        }

        public float SecondLen
        {
            get => (float)GetValue(SecondLenProperty);
            set => SetValue(SecondLenProperty, value);
        }

        public float MinuteLen
        {
            get => (float)GetValue(MinuteLenProperty);
            set => SetValue(MinuteLenProperty, value);
        }

        public float HourLen
        {
            get => (float)GetValue(HourLenProperty);
            set => SetValue(HourLenProperty, value);
        }

        public Color GradationTextColor
        {
            get => (Color)GetValue(GradationTextColorProperty);
            set => SetValue(GradationTextColorProperty, value);
        }

        public float GradationTextSize
        {
            get => (float)GetValue(GradationTextSizeProperty);
            set => SetValue(GradationTextSizeProperty, value);
        }

        public float GradationTextGap
        {
            get => (float)GetValue(GradationTextGapProperty);
            set => SetValue(GradationTextGapProperty, value);
        }

        public int CurrentTime
        {
            get => (int)GetValue(CurrentTimeProperty);
            set => SetValue(CurrentTimeProperty, value);
        }

        public Color IndicatorColor
        {
            get => (Color)GetValue(IndicatorColorProperty);
            set => SetValue(IndicatorColorProperty, value);
        }

        public float IndicatorTriangleSideLen
        {
            get => (float)GetValue(IndicatorTriangleSideLenProperty);
            set => SetValue(IndicatorTriangleSideLenProperty, value);
        }

        public float IndicatorWidth
        {
            get => (float)GetValue(IndicatorWidthProperty);
            set => SetValue(IndicatorWidthProperty, value);
        }

        public static int[] UnitSeconds = {
            10, 10, 10, 10,
            60, 60,
            5 * 60, 5 * 60,
            15 * 60, 15 * 60, 15 * 60, 15 * 60, 15 * 60, 15 * 60
        };

        public static float[] PerCountScaleThresholds = {
            6f, 3.6f, 1.8f, 1.5f,
            0.8f, 0.4f,
            0.25f, 0.125f,
            0.07f, 0.04f, 0.03f, 0.025f, 0.02f, 0.015f
        };

        private float scale = 1f;
        private float oneSecondGap = 12f / 60f;
        public float unitGap = 12f;
        public int perTextCountIndex = 4;
        public int unitSecond = UnitSeconds[4];
        public float textHalfWidth;

        public float currentDistance;
        private int scrollSlop = 10;
        private int minVelocity = 100;

        private float initialX;
        private float lastX;
        private bool isMoving;
        private bool isScaling;
        private Animation flingAnimation;

        public List<TimePart> timePartList;
        private Action<int> onTimeChanged;

        public class TimePart
        {
            public int StartTime { get; set; }
            public int EndTime { get; set; }
        }

        private GraphicsView graphicsView;

        public TimeRulerView()
        {
            graphicsView = new GraphicsView
            {
                BackgroundColor = Colors.Transparent,
                InputTransparent = true,
            };
            Content = graphicsView;
            // Enable gestures
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(panGesture);

            var pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += OnPinchUpdated;
            GestureRecognizers.Add(pinchGesture);

            // Create drawable
            graphicsView.Drawable = new TimeRulerDrawable(this);

            // Calculate text width
            textHalfWidth = MeasureTextWidth("00:00") / 2f;
            CalculateValues();
        }

        private static void OnCurrentTimeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (TimeRulerView)bindable;
            view.CalculateValues();
            view.graphicsView.Invalidate();
        }

        private void CalculateValues()
        {
            currentDistance = CurrentTime / (float)unitSecond * unitGap;
        }

        private float MeasureTextWidth(string text)
        {
            // Approximate text width in MAUI
            return text.Length * GradationTextSize * 0.6f; // Rough estimation
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    isMoving = false;
                    initialX = (float)e.TotalX;
                    lastX = initialX;
                    if (flingAnimation != null)
                    {
                        AnimationExtensions.AbortAnimation(this, "Fling");
                    }
                    break;

                case GestureStatus.Running:
                    if (isScaling) break;
                    float dx = (float)e.TotalX - lastX;
                    if (!isMoving)
                    {
                        if (Math.Abs((float)e.TotalX - initialX) <= scrollSlop) break;
                        isMoving = true;
                    }
                    currentDistance -= dx;
                    ComputeTime();
                    break;

                case GestureStatus.Completed:
                    if (isScaling || !isMoving) break;
                    // Simulate fling
                    float velocity = ((float)e.TotalX - initialX) / 0.1f;
                    if (Math.Abs(velocity) >= minVelocity)
                    {
                        float flingVelocity = -velocity;
                        flingAnimation = new Animation(v =>
                        {
                            currentDistance += (float)v;
                            ComputeTime();
                        }, 0, flingVelocity, Easing.Linear);

                        flingAnimation.Commit(this, "Fling", 16, 1000, Easing.Linear, (v, c) =>
                        {
                            flingAnimation = null;
                        });
                    }
                    break;
            }
            lastX = (float)e.TotalX;
        }

        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            switch (e.Status)
            {
                case GestureStatus.Started:
                    isScaling = true;
                    break;

                case GestureStatus.Running:
                    float scaleFactor = (float)e.Scale;
                    float maxScale = PerCountScaleThresholds[0];
                    float minScale = PerCountScaleThresholds[PerCountScaleThresholds.Length - 1];

                    if ((scaleFactor > 1 && scale >= maxScale) || (scaleFactor < 1 && scale <= minScale))
                        break;

                    scale *= scaleFactor;
                    scale = Math.Max(minScale, Math.Min(maxScale, scale));
                    perTextCountIndex = FindScaleIndex(scale);

                    unitSecond = UnitSeconds[perTextCountIndex];
                    unitGap = scale * oneSecondGap * unitSecond;

                    currentDistance = CurrentTime / (float)unitSecond * unitGap;
                    graphicsView.Invalidate();
                    break;

                case GestureStatus.Completed:
                    isScaling = false;
                    break;
            }
        }

        private int FindScaleIndex(float scale)
        {
            int min = 0;
            int max = PerCountScaleThresholds.Length - 1;
            int mid = (min + max) >> 1;

            while (!(scale >= PerCountScaleThresholds[mid] && scale < PerCountScaleThresholds[mid - 1]))
            {
                if (scale >= PerCountScaleThresholds[mid - 1])
                    max = mid;
                else
                    min = mid + 1;

                mid = (min + max) >> 1;
                if (min >= max || mid == 0) break;
            }

            return mid;
        }

        private void ComputeTime()
        {
            float maxDistance = MAX_TIME_VALUE / (float)unitSecond * unitGap;
            currentDistance = Math.Min(maxDistance, Math.Max(0, currentDistance));
            CurrentTime = (int)(currentDistance / unitGap * unitSecond);
            onTimeChanged?.Invoke(CurrentTime);
            graphicsView.Invalidate();
        }

        public static string FormatTimeHHmmss(int timeValue)
        {
            int hour = timeValue / 3600;
            int minute = timeValue % 3600 / 60;
            int second = timeValue % 3600 % 60;
            return $"{hour:D2}:{minute:D2}:{second:D2}";
        }

        public void SetOnTimeChangedListener(Action<int> listener)
        {
            onTimeChanged = listener;
        }

        public void SetTimePartList(List<TimePart> timeParts)
        {
            timePartList = timeParts;
            graphicsView.Invalidate();
        }
    }
}