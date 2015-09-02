using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfTemplateLib
{
    public class TaSlider : Slider
    {
        public TaSlider()
        {
            
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //var track = (Track)GetTemplateChild("PART_Track");
            //if (track != null)
            //{
            //    //var tt = track.Thumb.ToolTip as ToolTip;
            //    track.Thumb.LayoutUpdated += tt_SizeChanged;
            //}
        }

        void tt_SizeChanged(object sender, EventArgs e)
        {
            var border = (Border)GetTemplateChild("TrackBackground");

            if (null != border)
            {
                var binding = border.GetBindingExpression(WidthProperty);
                if (null != binding)
                {
                    binding.UpdateTarget();
                }
            }
        }

    }
}