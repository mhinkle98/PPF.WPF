   M S B A M L   `   `   `  ????B  >PPF.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:PPF.WPFPPF.WPF  5           PPF.WPF.InterferometryView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      'localclr-namespace:PPF.WPF     5      [ WSciChart.Drawing, Version=6.5.1.26063, Culture=neutral, PublicKeyToken=b55dd9efe817e823X TSciChart.Core, Version=6.5.1.26063, Culture=neutral, PublicKeyToken=b55dd9efe817e823\ XSciChart.Charting, Version=6.5.1.26063, Culture=neutral, PublicKeyToken=b55dd9efe817e823X TSciChart.Data, Version=6.5.1.26063, Culture=neutral, PublicKeyToken=b55dd9efe817e8237s)http://schemas.abtsoftware.co.uk/scichart     5      .??5
      ? $
??Auto??6   $??Stretch=?6     ? RowDefinitions  5   
   ?? 5      $??35 ?6   ?? 5      $??30 ?6   ?? 5      $??* ?6   ?? 5      $??25 ?6    ? ColumnDefinitions 5   
   ?? 5      $	??150 ?6   ?? 5      $??* ?6   ?? 5      $??20 ?6   .+?5   
   ??  ?? Orientation$ 
Horizontal=?6   $??0??6.   $??1??6;   $??Stretch=?6K   $??0 0 0 2q?6i   .+?5      ?? $??2q?6   
???????$??5q?6<   $??1 0 0 0q?6H    ? Cursor$
 Hand??6Y   $??30??6g   $??New graph from data??6s   
?????? ??6?   ?? *OpenDataNewGraphCommand+6?   ?? 5      $??2q?6   
???????$??5q?6<   $??	10 0 10 0q?6H   $
 Hand??6[   $??30??6i   $??Add data to current??6u   
?????? ??6?   ?? *OpenDataCurrentGraphCommand+6?   ?? 5      $??2q?6   
???r???$??5q?6=   $??1 0 0 0q?6I   $
 Hand??6Z   $??30??6h   $??Delete chart??6t   
?????? {?. ??6?   ?? *DeleteChartCommand+6?   5      ?? 5   
   $??1??6   $??1??6"   $??Center=?62   $	??Top=?6O   $ 
Horizontal=?6g   $??0 0 0 5q?6?   .+?5      ?? $??Center=?6   $??20(?6Y   
???????$??0 0 3 0q?6z   ??65   ?? *Selected.ChartName+6z   ?? 5      $
??Auto??6   $??Bottom=?6#   $
 Hand??6>   
?????? 
?????? 
???????$??12(?6?   $??Italic$?6?   $??rename??6?   ??6?   ?? *RenameChartCommand+5       ?? 5"   
   -   5         ??Name$ stackPanel2??5"      $ Vertical=?6*   $??2??6A   $??0??6N   $??Stretch=?6^   .+?5#      ?? $??Charts??6   $??15(?6&   
???????$??Center=?6G   ?? 5$      $??Center=?6   
?????? 
?????? ??6;   ?? *Charts+??5%      ?? 5&      .R?5'      ?? 
???????$??0 0 0 1q?65   ._?5(      ?? $
 Hand??5)   %   $??Stretch=?5*   %   $	??130??5+   %   $??25??5,   %   
???????
?????? 
?????? $??10(?50   %   $	??2,5q?54   %   ??5(   %   ?? *	ChartName+??51   %   ?? * DataContext.ChangeChartCommand+ ?? RelativeSource ??  ?? AncestorType    PPF.WPF.InterferometryView"  ??53   %   ?? 55      56      57      59      5:      /  )SciChart.Charting.Visuals.SciChartSurface  5<   
   -   5          Name$ sciChartsSurface2??5<   9   $??2??6   $??1??6)      RenderableSeries 5>      I  CSciChart.Charting.Visuals.RenderableSeries.FastLineRenderableSeries  E  ?SciChart.Charting.Visuals.RenderableSeries.BaseRenderableSeries	   
DataSeries	 6-   ?? *Selected.ChartDataSeries+
   XAxis
 5@      0  *SciChart.Charting.Visuals.Axes.NumericAxis  5A      -  'SciChart.Charting.Visuals.Axes.AxisCore   	AxisTitle 6    ?? *Selected.XLabel+   YAxis 5C        5D      $ Phase Offset??6    -  'SciChart.Charting.Visuals.Axes.AxisBase   AxisAlignment$
 Left=?69      ChartModifier 5F      4  .SciChart.Charting.ChartModifiers.ModifierGroup  5G         ChildModifiers. 5H      ?	  9SciChart.Charting.ChartModifiers.RubberBandXyZoomModifier	  8
  2SciChart.Charting.ChartModifiers.ChartModifierBase 
  	ExecuteOn$ MouseLeftButton=?5I       	  RubberBandFill ?	#33FFFFFF 	  RubberBandStroke ?	#AAFFFFFF  	  RubberBandStrokeDashArray$	 2 2R?5K      6  0SciChart.Charting.ChartModifiers.ZoomPanModifier  5L      $ MouseRightButton=?6(   :  4SciChart.Charting.ChartModifiers.ZoomPanModifierBase   	ClipModeX$
 None=?6E   8  2SciChart.Charting.ChartModifiers.YAxisDragModifier  5M      ;  5SciChart.Charting.ChartModifiers.AxisDragModifierBase   DragMode$ Scale=?6*   8  2SciChart.Charting.ChartModifiers.XAxisDragModifier  5N      $	 Pan=?6*   =  7SciChart.Charting.ChartModifiers.MouseWheelZoomModifier  5O      :  4SciChart.Charting.ChartModifiers.ZoomExtentsModifier  5P      $ MouseDoubleClick=?6,   5Q      5S      1  +SciChart.Charting.Visuals.SciChartScrollbar  5T   
   $??2??6   $??2??6+   $??1 0 0 0q?6v      Axis 6;   ??  ?? ElementName$ sciChartsSurface2?? ?? Path$ YAxis?6v     5U   
   $??3??6   $??1??6+   $??	15 0 15 0q?6v    6;   ?? $ sciChartsSurface2??$ XAxis?6v   5V      5W      