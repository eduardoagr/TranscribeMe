using System.Windows.Controls;

using TranscribeMe.ViewModel;

namespace TranscribeMe.CustomControls {
    /// <summary>
    /// Interaction logic for SideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : UserControl {
        public SideMenuControl() {
            InitializeComponent();
            DataContext = new SideMenuControlViewModel();
        }

        public int SelectedIndex {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(SideMenuControl));


        //Since we want to navigate through different pages in listbox item selection change event we are declaring  an event in our usercontrol for that...
        public event EventHandler SelectionChanged;
        private void MyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            SelectionChanged?.Invoke(sender, e);
        }
    }
}
