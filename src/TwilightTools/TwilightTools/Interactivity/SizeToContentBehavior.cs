using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace Elgraiv.TwilightTools.Interactivity
{
    public class SizeToContentBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += WindowLoaded;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            //WindowChromeを使っていると初期化時点では表示がずれるためここで設定する
            AssociatedObject.SizeToContent = SizeToContent.WidthAndHeight;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= WindowLoaded;
            base.OnDetaching();
        }
    }
}
