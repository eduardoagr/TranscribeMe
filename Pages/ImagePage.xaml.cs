﻿using System.Windows.Controls;

using TranscribeMe.ViewModel;

namespace TranscribeMe.Pages {

    public partial class ImagePage : Page {

        public static ImagePageViewModel Instance { get; } = new ImagePageViewModel();

        public ImagePage() {
            InitializeComponent();
            DataContext = Instance;
        }
    }
}
