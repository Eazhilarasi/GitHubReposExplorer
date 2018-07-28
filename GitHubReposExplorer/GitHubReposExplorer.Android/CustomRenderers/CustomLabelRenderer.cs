using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GitHubReposExplorer.Droid.CustomRenderers;
using GitHubReposExplorer.Views.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly : ExportRenderer(typeof(CustomLabel), typeof(CustomLabelRenderer))]
namespace GitHubReposExplorer.Droid.CustomRenderers
{
    public class CustomLabelRenderer : LabelRenderer
    {
        public CustomLabelRenderer(Context ctx) : base(ctx) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if(Element != null && Control != null)
            {
                SetLineNumber((CustomLabel)Element);
            }

        }

        private void SetLineNumber(CustomLabel label)
        {
            if (label.MaxLines > 0)
            {
                Control.SetSingleLine(false);
                Control.SetMaxLines(label.MaxLines);
            }
        }
    }
}