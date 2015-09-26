using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using PuzzleGenerator;

namespace DesktopTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource", typeof(ImageSource), typeof(MainWindow), new PropertyMetadata(default(ImageSource)));

        public static readonly DependencyProperty XBlocksCountProperty = DependencyProperty.Register(
            "XBlocksCount", typeof(int), typeof(MainWindow), new PropertyMetadata(4));
        
        public static readonly DependencyProperty YBlocksCountProperty = DependencyProperty.Register(
            "YBlocksCount", typeof(int), typeof(MainWindow), new PropertyMetadata(4));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public int XBlocksCount
        {
            get { return (int)GetValue(XBlocksCountProperty); }
            set { SetValue(XBlocksCountProperty, value); }
        }

        public int YBlocksCount
        {
            get { return (int)GetValue(YBlocksCountProperty); }
            set { SetValue(YBlocksCountProperty, value); }
        }

        private string _filename;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();

            //fileDialog.DefaultExt = ".png";
            fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            Nullable<bool> result = fileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                // Open document 
                _filename = fileDialog.FileName;
                ImageSource = new BitmapImage(new Uri(_filename));
            }
            else
            {
                MessageBox.Show("Image does not selected!");
            }
        }

        

        private void CreatePuzzleButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_filename))
            {
                MessageBox.Show("Image does not selected!");
                return;
            }

            var filePath = _filename;
            var x = XBlocksCount;
            var y = YBlocksCount;
            var generator = new PuzzleCreator(filePath);
            var images = generator.GetMixedPartsOfImage(x, y);
            Bitmap bitmap = generator.GenerateBitmap(images, x, y);


            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "puzzle"; 
            saveFileDialog.DefaultExt = ".jpg";
            saveFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            Nullable<bool> result = saveFileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                bitmap.Save(saveFileDialog.FileName);
            }
            else
            {
                MessageBox.Show("Puzzle does not saved!");
            }
        }
    }
}
