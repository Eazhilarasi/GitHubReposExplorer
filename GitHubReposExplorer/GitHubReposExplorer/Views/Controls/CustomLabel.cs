using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GitHubReposExplorer.Views.Controls
{
    public class CustomLabel: Label
    {

        public int MaxLines
        {
            get { return (int)GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }

        public static readonly BindableProperty MaxLinesProperty =
            BindableProperty.Create("MaxLines", typeof(int), typeof(CustomLabel), default(int));

    }
}
