using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using GitHubReposExplorer.iOS.CustomRenderers;
using GitHubReposExplorer.Views.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRenderer))]
namespace GitHubReposExplorer.iOS.CustomRenderers
{
    public class CustomLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Element != null && Control != null)
            {
                SetLineNumber((CustomLabel)Element);
            }
        }

        private void SetLineNumber(CustomLabel label)
        {
            if (label.MaxLines > 0)
            {
                Control.Lines = label.MaxLines;
            }
        }
    }
}