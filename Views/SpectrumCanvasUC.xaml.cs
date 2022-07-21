using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CustomSpectrumAnalyzer
{
    /// <summary>
    /// SpectrumCanvasUC.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SpectrumCanvasUC : UserControl
    {
        public bool IsInit = false;

        List<Point> pointList = new List<Point>();

        #region Width/Height Offsets
        double heightTextOffset = 5;

        const int X_MarkerTextOffset = 8;
        const int Y_MarkerTextOffset = 30;

        int polygonOffsetA = 5; // polygon offset x 
        int polygonOffsetB = 10; // polygon offset y
        #endregion

        #region Drag & Drop
        bool isDragged = false;
        double MovedX = 0.1;
        int dragNum = 0;
        #endregion

        readonly int XaxisMax = 3;

        int selectedMarkerIndex = -1;

        float[] data = null;

        #region object name
        readonly string MarkerPolygonName = "MarkerTri";
        readonly string MarkerTextName = "MarkerText";

        readonly string XaxisTextName = "XaxisText";
        readonly string YaxisTextName = "YaxisText";

        readonly string XunitTextName = "XunitText";
        readonly string YunitTextName = "YunitText";
        #endregion

        public SpectrumCanvasUC()
        {
            InitializeComponent();
            this.DataContext = App.Current.Services.GetService(typeof(SpectrumViewModel));
            IsInit = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Set Window Properties
            SpectrumViewModel.CanvasWidth = MainCanvas.ActualWidth;
            SpectrumViewModel.CanvasHeight = MainCanvas.ActualHeight;

            SetDefaultShapeProperties(SpectrumPolyline);

            SettingParameter defaultParam = new SettingParameter(ESettingCommandType.None);
            this.Init(defaultParam);
            this.GeneratePoints(defaultParam);
        }

        // SampleData Generate
        public void GeneratePoints(SettingParameter param)
        {
            this.pointList.Clear();

            // 최대 1000개의 데이터 발생시키기
            int MaxLength = 1000;

            #region Random하게 발생 (주석)
            //// -80~-20 정도로 발생시키기
            //float randomMin = -80;
            //float ranStep = 60;
            //float hold = 20;

            //// legacy
            //// float ranStep = 300;
            //// float randomMin = 60;

            //Random random = new Random();
            //for (int i = 0; i < MaxLength; i++)
            //{
            //    float data = randomMin + (float)random.NextDouble() * ranStep; // 0~1 * step

            //    // hold 값 기준으로 데이터 편차가 클 경우에 다시 한번 랜덤 연산 적용할 것
            //    // 좀 더 중간으로 고르게 분포시키기 위해서
            //    if (data <= randomMin + hold || data >= randomMin + ranStep - hold)
            //    {
            //        data = randomMin + (float)random.NextDouble() * ranStep;
            //    }

            //    double scaledX = (double)i * (canvasWidth - widthOffset) / (double)LengthOfData;
            //    double scaledY = GetScaledY(data);

            //    // scaledY의 크기를 그래프 안쪽으로 맞추기 위한 조정
            //    if (scaledY < heightOffset)
            //    {
            //        scaledY = heightOffset;
            //    }

            //    else if (scaledY > canvasHeight - heightOffset * 2)
            //    {
            //        scaledY = canvasHeight - heightOffset * 2;
            //    }

            //    // Points.Add(new Point(scaledX, scaledY));
            //    pointList.Add(new Point(scaledX, scaledY));
            //}
            #endregion

            // -100 ~ 100 사이 발생
            float randomMin = -100;
            float ranStep = 100;

            data = new float[MaxLength];
            for (int i = 0; i < MaxLength; i++)
            {
                float y = randomMin + i * 0.001f * ranStep; // 0~1 * step
                data[i] = y;

                double scaledX = SpectrumViewModel.GetScaledX(i);
                double scaledY = SpectrumViewModel.GetScaledY(y, param.ViewerRefLv);

                pointList.Add(new Point(scaledX, scaledY));
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                SpectrumPolyline.Points = new PointCollection(pointList);
                SpectrumPolyline.Stroke = new SolidColorBrush() { Color = Color.FromRgb(160, 128, 0) };
                SpectrumPolyline.Stroke.Freeze();
            }));
        }

        public void Init(SettingParameter defaultParam)
        {
            #region Draw OutLine
            // Draw Vertical Lines
            for (int i = 0; i <= SpectrumViewModel.CrossLineSize; i++)
            {
                double x = GetXpos(i);

                Line verticalLine = new Line();
                SetDefaultShapeProperties(verticalLine);
                verticalLine.X1 = x;
                verticalLine.Y1 = 0;

                verticalLine.X2 = x;
                verticalLine.Y2 = SpectrumViewModel.CanvasHeight - SpectrumViewModel.HeightOffset;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    MainCanvas.Children.Add(verticalLine);
                }));
            } // end for (int i = 0; i <= CrossLineSize; i++)

            // Draw X Axis Text
            for (int i = 0; i < XaxisMax; i++)
            {
                DrawXaxisText(GetXpos(i * (SpectrumViewModel.CrossLineSize / (XaxisMax - 1))), i, defaultParam.CenterFreq, defaultParam.Span);
            }

            // Draw X Unit Text
            DrawXunitText();

            // Draw Horizontal Lines (In Draw Y Axis Text)
            for (int i = 0; i <= SpectrumViewModel.CrossLineSize; i++)
            {
                double y = GetYpos(i);

                Line horizontalLine = new Line();
                SetDefaultShapeProperties(horizontalLine);
                horizontalLine.X1 = 0;
                horizontalLine.Y1 = y;

                horizontalLine.X2 = SpectrumViewModel.CanvasWidth - SpectrumViewModel.WidthOffset;
                horizontalLine.Y2 = y;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    MainCanvas.Children.Add(horizontalLine);
                }));

                this.DrawYaxisText(y, i, defaultParam.ViewerRefLv);
            } // end for (int i = 0; i <= CrossLineSize; i++)

            // Draw Y Unit Text
            DrawYunitText();
            #endregion
        }

        private double GetXpos(int iter)
        {
            return (double)iter / (double)SpectrumViewModel.CrossLineSize * (SpectrumViewModel.CanvasWidth - SpectrumViewModel.WidthOffset);
        }

        private double GetYpos(int iter)
        {
            return (double)iter / (double)SpectrumViewModel.CrossLineSize * (SpectrumViewModel.CanvasHeight - SpectrumViewModel.HeightOffset);
        }

        // X축 Text 구성 및 생성/도시
        private void DrawXaxisText(double x, int iter, double centerFreq, double span)
        {
            int xOffset = 25;
            TextBlock textBlockX = new TextBlock();
            textBlockX.Name = XaxisTextName + iter.ToString();

            // iter의 경우의 수 :: 0(왼쪽 끝), 1(중앙), 2(우측 끝)
            if (iter == 0)
            {
                // 가장 왼쪽 x
                x = 0;
            }

            else if (iter == 1)
            {
                // center x
                x = (SpectrumViewModel.CanvasWidth - 0) / 2 - xOffset;
            }

            else if (iter == 2)
            {
                // 가장 오른쪽 x
                x = SpectrumViewModel.CanvasWidth - 0 - xOffset - xOffset;
            }

            // Center Frequency와 Span 조정에 따른 Text 값, 배치 수정
            // Revision To Do 
            textBlockX.Text = (centerFreq - (span / 2) + (iter * span / (XaxisMax - 1))).ToString("F2");
            Canvas.SetTop(textBlockX, SpectrumViewModel.CanvasHeight - heightTextOffset);
            Canvas.SetLeft(textBlockX, x);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                MainCanvas.Children.Add(textBlockX);
            }));
        }

        // Y축 Text 구성 및 생성/도시
        private void DrawYaxisText(double y, int iter, double viewerRefLv)
        {
            double widthTextOffset = -18;

            // Y Legend Text
            TextBlock textBlockY = new TextBlock();
            textBlockY.Name = YaxisTextName + iter.ToString(); // 0 ~ 10

            // Ref Lv에 따른 Text 값 수정
            // Revision To Do 
            var yValue = viewerRefLv - SpectrumViewModel.YInterval * iter;
            textBlockY.Text = yValue.ToString();

            if (yValue < 0)
            {
                widthTextOffset -= 7;
            }

            // Viewer Ref Lv에 따른 Y축 Text 배치 수정
            Canvas.SetTop(textBlockY, y - 2);
            Canvas.SetLeft(textBlockY, widthTextOffset);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                MainCanvas.Children.Add(textBlockY);
            }));
        }
        #region 고정 요소 도시
        // X축 단위 이름 표시
        private void DrawXunitText()
        {
            double xUnitOffset = 5;
            TextBlock textBlockXunit = new TextBlock();
            textBlockXunit.Name = XunitTextName;
            textBlockXunit.Text = "[MHZ]";
            Canvas.SetTop(textBlockXunit, SpectrumViewModel.CanvasHeight - heightTextOffset);
            Canvas.SetLeft(textBlockXunit, SpectrumViewModel.CanvasWidth - xUnitOffset);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                MainCanvas.Children.Add(textBlockXunit);
            }));
        }

        // Y축 단위 이름 표시
        private void DrawYunitText()
        {
            double yUnitOffset = 17;
            TextBlock textBlockYunit = new TextBlock();
            textBlockYunit.Name = YunitTextName;
            textBlockYunit.Text = "[dBm]";
            Canvas.SetTop(textBlockYunit, -yUnitOffset);
            Canvas.SetLeft(textBlockYunit, -yUnitOffset);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                MainCanvas.Children.Add(textBlockYunit);
            }));
        }
        #endregion

        public void ResetAllMarker()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                // 모든 Marker를 순회하면서 해당 Marker의 Control들을 삭제함
                for (int i = 1; i <= SpectrumViewModel.MarkerNum; i++)
                {
                    var markerPolyObj = (Polygon)this.FindChildFromName(MarkerPolygonName + i.ToString());
                    var markerTextObj = (TextBlock)this.FindChildFromName(MarkerTextName + i.ToString());

                    if (markerPolyObj != null && markerTextObj != null)
                    {
                        MainCanvas.Children.Remove(markerPolyObj);
                        MainCanvas.Children.Remove(markerTextObj);
                    }
                }

                // MarkerLine.Visibility = Visibility.Hidden;
                MarkerThumb.Visibility = Visibility.Hidden;
            }));

            // Marker Number 초기화
            SpectrumViewModel.MarkerNum = 1;
        }

        // 설정 적용
        public void SetOptions(SettingParameter param)
        {
            // 바뀐 설정에 따른 축 데이터 적용

            // Find X axis Text (1~max)
            // And Apply X Setting
            for (int i = 0; i < XaxisMax; i++)
            {
                var element = FindChildFromName(XaxisTextName + i);

                // 해당 Element 제거 후 값 수정하여 재등록
                Dispatcher.Invoke(new Action(() =>
                {
                    MainCanvas.Children.Remove(element);
                }));

                DrawXaxisText(GetXpos(i), i, param.CenterFreq, param.Span);
            }

            // Find Y axis Text (0~SizeOfLine)
            // And Apply Y Setting
            for (int i = 0; i <= SpectrumViewModel.CrossLineSize; i++)
            {
                var element = FindChildFromName(YaxisTextName + i);

                // 해당 Element 제거 후 값 수정하여 재등록
                Dispatcher.Invoke(new Action(() =>
                {
                    MainCanvas.Children.Remove(element);
                }));

                DrawYaxisText(GetYpos(i), i, param.ViewerRefLv);
            }
        }

        private void SetDefaultShapeProperties(Shape shape, double thickness = 1, System.Windows.Media.Brush brush = null)
        {
            if (shape != null)
            {
                shape.Visibility = Visibility.Visible;
                shape.StrokeThickness = thickness;
                shape.Stroke = System.Windows.Media.Brushes.Gray;
                if (brush != null)
                {
                    shape.Stroke = brush;
                }
            }
        }

        #region Marker 관련
        private Point GetPolygonCenter(PointCollection ptCol)
        {
            double x = 0.0;
            double y = 0.0;

            foreach (var pt in ptCol)
            {
                x += pt.X;
                y += pt.Y;
            }

            double centerX = x / ptCol.Count;
            double centerY = y / ptCol.Count;

            return new Point(centerX, centerY);
        }

        // 선택된 위치에 marker가 존재하는 지, 어떤 것인 지 판별
        // Hit 실패 시(Marker 선택되지 않은 경우) -1 반환, 성공 시 해당 Marker Index 반환
        // 추가로 선택한 marker의 중앙 위치를 반환, 선택된 marker가 없을 경우는 빈 객체 변환함
        private int HitTestMarker(Point clickedPosition, ref Point markerCenterPosition)
        {
            int markerIndex = -1;

            // 클릭한 지점 주변 거리
            double xClickedBound = 10.0;
            double yClickedBound = 10.0;

            foreach (var child in MainCanvas.Children)
            {
                // Marker를 표현하는 도형 Polygon으로
                if (child is Polygon)
                {
                    Polygon element = child as Polygon;
                    Point center = GetPolygonCenter(element.Points);
                    markerCenterPosition = center;

                    // Boundary 안에 들어가 있는 Marker인 경우
                    if (clickedPosition.X <= center.X + xClickedBound &&
                         clickedPosition.X >= center.X - xClickedBound &&
                         clickedPosition.Y <= center.Y + yClickedBound &&
                         clickedPosition.Y >= center.Y - yClickedBound)
                    {
                        string name = element.Name;
                        name = name.Replace(MarkerPolygonName, "");
                        markerIndex = int.Parse(name);
                        break;
                    }
                }
            } // end foreach (var child in MainCanvas.Children)

            return markerIndex;
        }

        // Line에 대한 HitTest
        private bool HitTestLine(Point clickedPosition)
        {
            bool bHit = false;

            // 클릭한 지점 주변 거리
            double xClickedBound = 7.0;
            double yClickedBound = 7.0;

            //// MarkerLine Always X1==X2
            //if (clickedPosition.X > MarkerThumb. - xClickedBound &&
            //    clickedPosition.X < MarkerLine.X2 + xClickedBound &&
            //    clickedPosition.Y > MarkerLine.Y1 - yClickedBound &&
            //    clickedPosition.Y < MarkerLine.Y2 + yClickedBound)
            //{
            //    bHit = true;
            //}

            return bHit;
        }

        // Marker 추가
        public void AddMarker(System.Windows.Input.MouseButtonEventArgs e)
        {
            var curPos = e.GetPosition(this.MainCanvas);
            Point markerCenterPos = new Point();

            SpectrumViewModel.ClickedPoint = curPos;

            // Mouse Down 위치에 Marker 유무 확인
            selectedMarkerIndex = HitTestMarker(curPos, ref markerCenterPos);

            // 성공
            if (selectedMarkerIndex != -1)
            {
                SpectrumViewModel.MarkerNum = selectedMarkerIndex;
            } 

            // 실패
            else
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    // Marker 생성
                    // Mouse Down 위치에 삼각형 도형을 생성하도록 
                    // 1) Get Mouse Down Position : (x, y)
                    // 2) 세 꼭지점 Polygon 생성
                    // -> (x, y) - (x-a, y-b) - (x+a, y-b)
                    // 3) TextBlock 생성 :: 위치 (y-b-b), Content = "M1"

                    // 삼각형 생성(Marker Triangle)
                    Polygon polygon = new Polygon();
                    polygon.Name = MarkerPolygonName + SpectrumViewModel.MarkerNum.ToString(); // To Do :: Marker polygon Num에 따른 Name 변경할 것
                    Point p1 = curPos;
                    Point p2 = new Point(curPos.X, curPos.Y);
                    Point p3 = new Point(curPos.X, curPos.Y);

                    polygon.Points.Add(curPos);
                    polygon.Points.Add(new Point(curPos.X - polygonOffsetA, curPos.Y - polygonOffsetB));
                    polygon.Points.Add(new Point(curPos.X + polygonOffsetA, curPos.Y - polygonOffsetB));
                    polygon.Fill = new SolidColorBrush() { Color = Color.FromArgb(100, 255, 0, 0) };
                    polygon.MouseDown += Polygon_MouseDown;

                    // 텍스트 블록 생성(Marker TextBlock)
                    TextBlock textBlockMarker = new TextBlock();
                    textBlockMarker.Name = MarkerTextName + SpectrumViewModel.MarkerNum.ToString(); // To Do :: Marker Num에 따른 Marker Name 변경
                    textBlockMarker.Text = "M" + SpectrumViewModel.MarkerNum.ToString();

                    Canvas.SetLeft(textBlockMarker, curPos.X - X_MarkerTextOffset);
                    Canvas.SetTop(textBlockMarker, curPos.Y - Y_MarkerTextOffset);

                    SetDefaultShapeProperties(polygon, 1.5, System.Windows.Media.Brushes.Red);

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        SpectrumViewModel.MarkerNum++;

                        MainCanvas.Children.Add(polygon);
                        MainCanvas.Children.Add(textBlockMarker);
                    }));
                } // end if (e.LeftButton == MouseButtonState.Pressed)
            }
        }
        /// Polygon MouseDown Event -> Behavior 쪽으로 넘김
        //private void PolyLine_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    var curPos = e.GetPosition(this.MainCanvas);
        //    AddMarker(e.GetPosition(this.MainCanvas));
        //}

        private void Polygon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var polygon = sender as Polygon;
            string strName = polygon.Name;

            var curPos = e.GetPosition(this.MainCanvas);
            Point markerCenterPos = new Point();

            SpectrumViewModel.ClickedPoint = curPos;
            // Mouse Down 위치에 Marker 유무 확인
            selectedMarkerIndex = HitTestMarker(SpectrumViewModel.ClickedPoint, ref markerCenterPos);

            // 성공
            if (selectedMarkerIndex != -1)
            {
                // Hit 위치에 더블클릭 시 해당 x 위치에 y0 ~ yHeight의 Line 생성
                if (e.ClickCount == 2)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        MarkerThumb.Visibility = Visibility.Visible;
                        Canvas.SetLeft(MarkerThumb, markerCenterPos.X);
                    }));

                    isDragged = false;
                }
            } // end if (selectedMarkerIndex != -1)
        }

        public void MarkerDragged(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;
            Point markerCenterPos = new Point();

            if (MovedX < 0 || MovedX > MainCanvas.ActualWidth)
            {
                // return;
            }

            if (isDragged)
            {
                // x + 범위 넘어가는 경우
                double XRightMargin = 12;
                double XLeftMargin = 2;

                // 아예 바깥으로 못 넘어가게끔 예외처리 필요
                if ((e.HorizontalChange > 0 && MovedX > MainCanvas.ActualWidth - XRightMargin)
                    || (e.HorizontalChange < 0 && MovedX < XLeftMargin))
                {
                    return;
                }

                Canvas.SetLeft(thumb, MovedX + e.HorizontalChange);
                MovedX += e.HorizontalChange;

                if (dragNum++ % 10 <= 10)
                {
                    // Moved X 위치에 해당 Marker Num을 배치하기
                    var xIndex = SpectrumViewModel.GetXIndexAtScreen(MovedX);
                    // data 배열 Index에 대한 예외처리
                    if (xIndex < 0 || xIndex >= data.Length)
                    {
                        return;
                    }
                    var movedY = SpectrumViewModel.GetScaledY(data[xIndex], 0);

                    // Moved X에 대한 Frequency 값
                    // 선택한 Marker 객체를 찾아 위치 이동
                    var markerPolyObj = (Polygon)this.FindChildFromName(MarkerPolygonName + selectedMarkerIndex.ToString());
                    var markerTextObj = (TextBlock)this.FindChildFromName(MarkerTextName + selectedMarkerIndex.ToString());

                    if (markerPolyObj != null && markerTextObj != null)
                    {
                        // 현재 마우스 위치로 삼각형 위치 변경
                        markerPolyObj.Points.Clear();

                        markerPolyObj.Points.Add(new Point(MovedX, movedY));
                        markerPolyObj.Points.Add(new Point(MovedX - polygonOffsetA, movedY - polygonOffsetB));
                        markerPolyObj.Points.Add(new Point(MovedX + polygonOffsetA, movedY - polygonOffsetB));

                        SpectrumViewModel.DraggedPoint = new Point(MovedX, movedY);

                        // 텍스트 블록 위치 변경
                        Canvas.SetLeft(markerTextObj, MovedX - X_MarkerTextOffset);
                        Canvas.SetTop(markerTextObj, movedY - Y_MarkerTextOffset);
                    }
                }

            } // end if IsDragged

            else
            {
                MovedX = SpectrumViewModel.ClickedPoint.X + e.HorizontalChange;
                isDragged = true;

                // Mouse Down 위치에 Marker 유무 확인
                selectedMarkerIndex = HitTestMarker(SpectrumViewModel.ClickedPoint, ref markerCenterPos);
            }
        }

        /// Marker Thumb Drag-Drop Event -> Behavior 쪽으로 넘김
        //private void MarkerThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        //{
        //    // MarkerDragged(sender, e);
        //}

        /// Canvas에서 Name을 통해 Child Element를 Finding 하기
        private UIElement FindChildFromName(string strName)
        {
            UIElement element = null;

            // 뒤에서 부터 찾아나가기
            int countOfCanvasChild = this.MainCanvas.Children.Count;
            for (int i = countOfCanvasChild - 1; i >= 0; i--)
            {
                var child = this.MainCanvas.Children[i];

                if (child is TextBlock)
                {
                    TextBlock tbChild = (TextBlock)child;
                    if (tbChild.Name == strName)
                    {
                        return tbChild;
                    }
                }

                else if (child is Polygon)
                {
                    Polygon polygonChild = (Polygon)child;
                    if (polygonChild.Name == strName)
                    {
                        return polygonChild;
                    }
                }
            }
            return element;
        }
        #endregion
    }
}
