using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using AVFoundation;
using CoreGraphics;
using Foundation;
using MatrixLed;
using MatrixLed.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Led), typeof(LedRenderer))]
namespace MatrixLed.iOS
{
    class LedRenderer : BoxRenderer{
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Color"){
                SetNeedsDisplay();      
            }
        }

        public override void Draw(CGRect rect) {
            //base.Draw(rect);

            var led = (Led)Element;         
            using (var context = UIGraphics.GetCurrentContext()){   

                var shadowSize = 3;
                var blur = shadowSize;
                var radius = 20;

                context.SetFillColor(led.Color.ToCGColor());     
                var bounds = Bounds.Inset(2,2); 
                context.AddPath(CGPath.FromRoundedRect(bounds, radius, radius));
                context.SetShadow(new SizeF(shadowSize, shadowSize), blur);
                context.DrawPath(CGPathDrawingMode.Fill);
            }
        }
    }
}