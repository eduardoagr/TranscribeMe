using System.Windows.Controls;

namespace TranscribeMe.CustomControls {

    //We do this, this LisboxItem already has the selectionChange mechanism
    public class NavButton : ListBoxItem {
        static NavButton() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavButton),
                new FrameworkPropertyMetadata(typeof(NavButton)));
        }

        public Uri NavLink {
            get { return (Uri)GetValue(NavLinkProperty); }
            set { SetValue(NavLinkProperty, value); }
        }

        // This property will hold specifics on where to navigate within our appreciation
        public static readonly DependencyProperty NavLinkProperty =
            DependencyProperty.Register(
                "NavLink", typeof(Uri),
                typeof(NavButton),
                new PropertyMetadata(null));


        // This property will hold specifics on the icon
        public string Icon {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon",
                typeof(string),
                typeof(NavButton),
                new PropertyMetadata(null));



    }
}
