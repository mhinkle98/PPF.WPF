   M S B A M L   `   `   `  ???? B  >PPF.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:PPF.WPFPPF.WPF       PPF.WPF.InterferometryView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     'localclr-namespace:PPF.WPF     [ WSciChart.Drawing, Version=6.5.1.26063, Culture=neutral, PublicKeyToken=b55dd9efe817e823X TSciChart.Core, Version=6.5.1.26063, Culture=neutral, PublicKeyToken=b55dd9efe817e823\ XSciChart.Charting, Version=6.5.1.26063, Culture=neutral, PublicKeyToken=b55dd9efe817e823X TSciChart.Data, Version=6.5.1.26063, Culture=neutral, PublicKeyToken=b55dd9efe817e8237s)http://schemas.abtsoftware.co.uk/scichart     .??? $
??Auto??$??Stretch=?  ? RowDefinitions  ?? $??30 ??? $??24 ??? $??* ??? $??30 ? ? ColumnDefinitions ?? $	??150 ??? $??* ??? $??20 ?.+???  ?? Orientation$ 
Horizontal=?$??0??$??1??$??Stretch=?.+??? $??2q?
???????$??5q?$??1 0 0 0q? ? Cursor$
 Hand??$??30??$??New graph from data??
?????? ???? *OpenDataNewGraphCommand+?? $??2q?
???????$??5q?$??	10 0 10 0q?$
 Hand??$??30??$??Add data to current??
?????? ???? *OpenDataCurrentGraphCommand+?? $??2q?
???r???$??5q?$??1 0 0 0q?$
 Hand??$??30??$??Delete chart??
?????? {?. ???? *DeleteChartCommand+?? $??1??$??1??$??Stretch=?.+??? $??Center=?$??15(?
??????????? *Selected.ChartName+?? -    ??Name$ stackPanel2??$ Vertical=?$??2??$??0??$??Stretch=?.+??? $??Charts??$??15(?
???????$??Center=??? $??Center=?
?????? 
?????? ???? *Charts+???? .R??? 
???????$??0 0 0 1q?._??? $
 Hand??$??Stretch=?$	??130??$??25??
???????
?????? 
?????? $??10(?$	??2,5q????? *	ChartName+???? * DataContext.ChangeChartCommand+ ?? RelativeSource ??  ?? AncestorType    PPF.WPF.InterferometryView"  ???? /  )SciChart.Charting.Visuals.SciChartSurface  -     Name$ sciChartsSurface2??$??2??$??1??   RenderableSeries I  CSciChart.Charting.Visuals.RenderableSeries.FastLineRenderableSeries  E  ?SciChart.Charting.Visuals.RenderableSeries.BaseRenderableSeries	   
DataSeries	 ?? *Selected.ChartDataSeries+
   XAxis
 0  *SciChart.Charting.Visuals.Axes.NumericAxis  -  'SciChart.Charting.Visuals.Axes.AxisCore   	AxisTitle ?? *Selected.XLabel+   YAxis   $ Phase Offset??-  'SciChart.Charting.Visuals.Axes.AxisBase   AxisAlignment$
 Left=?   ChartModifier 4  .SciChart.Charting.ChartModifiers.ModifierGroup     ChildModifiers. ?	  9SciChart.Charting.ChartModifiers.RubberBandXyZoomModifier	  8
  2SciChart.Charting.ChartModifiers.ChartModifierBase 
  	ExecuteOn$ MouseLeftButton=? 	  RubberBandFill ?	#33FFFFFF 	  RubberBandStroke ?	#AAFFFFFF  	  RubberBandStrokeDashArray$	 2 2R?6  0SciChart.Charting.ChartModifiers.ZoomPanModifier  $ MouseRightButton=?:  4SciChart.Charting.ChartModifiers.ZoomPanModifierBase   	ClipModeX$
 None=?8  2SciChart.Charting.ChartModifiers.YAxisDragModifier  ;  5SciChart.Charting.ChartModifiers.AxisDragModifierBase   DragMode$ Scale=?8  2SciChart.Charting.ChartModifiers.XAxisDragModifier  $	 Pan=?=  7SciChart.Charting.ChartModifiers.MouseWheelZoomModifier  :  4SciChart.Charting.ChartModifiers.ZoomExtentsModifier  $ MouseDoubleClick=?1  +SciChart.Charting.Visuals.SciChartScrollbar  $??2??$??2??$??1 0 0 0q?   Axis ??  ?? ElementName$ sciChartsSurface2?? ?? Path$ YAxis?  $??3??$??1??$??	15 0 15 0q? ?? $ sciChartsSurface2??$ XAxis?