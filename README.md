# RuleView

Time ruler, a time ruler within 24 hours a day, the minimum scale is 1s
## 一、DEMO

> TimeRuleView:

![timeGif](https://github.com/NamCkiku/TimeRuler.Maui/blob/master/time.gif)


## 二、Description


### 1. TimeRuleView
Time ruler, a time ruler within 24 hours a day, the minimum scale is 1s

- Support inertial sliding
- Support zooming time scale
- Support display of multiple time periods

Property Name | Description | Default Value
:------ | :------ | :------
BackgroundColor | BackgroundColor | #EEEEEE
GradationColor | Scale color | Color.GRAY
PartHeight | Height of time period | 20dp
PartColor | The color of time | #F58D24
GradationWidth | Width of scale | 1px
SecondLen | Length of the seconds scale | 3dp
MinuteLen | Length of the sub-divisions | 5dp
HourLen | Length of the hour scale | 10dp
GradationTextColor | Length of the hour scale | Color.GRAY
GradationTextSize | Scale value font size | 12sp
GradationTextGap | The distance between the scale value and the hour scale | 2dp
CurrentTime | Current time | 0 (unit: seconds, range∈[0, 24*3600]）
IndicatorTriangleSideLen | The side length of the triangle at the middle pointer head | 15dp
IndicatorWidth | The side length of the triangle at the middle pointer head | 1dp
IndicatorColor |The color of the middle pointer  | Color.RED
 


## LICENSE
```
MIT License

Copyright (c) 2023 NamCkiku

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE..
```