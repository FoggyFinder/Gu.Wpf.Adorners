namespace Gu.Wpf.Adorners
{
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// A drag adorner that renders a rectangle with the visual brush of the adorned element.
    /// </summary>
    public class VisualBrushDragAdorner : DragAdorner<Rectangle>
    {
        static VisualBrushDragAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VisualBrushDragAdorner), new FrameworkPropertyMetadata(typeof(VisualBrushDragAdorner)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualBrushDragAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The drag source.</param>
        public VisualBrushDragAdorner(UIElement adornedElement)
            : base(adornedElement, new Rectangle())
        {
            this.Child.Bind(WidthProperty)
                .OneWayTo(adornedElement, ActualWidthProperty)
                .IgnoreReturnValue();
            this.Child.Bind(HeightProperty)
                .OneWayTo(adornedElement, ActualHeightProperty)
                .IgnoreReturnValue();
            this.Child.Fill = new VisualBrush(adornedElement);
        }
    }
}
