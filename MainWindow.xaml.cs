using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Crossword_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static private List<int> fromSixToFifteen = new List<int>();
        static private List<int> fromOneToTen = new List<int>();
        public MainWindow()
        {
            InitializeComponent();

            for (int i = 6; i <= 15; i++)
            {
                fromSixToFifteen.Add(i);
            }
            for (int i = 1; i <= 10; i++)
            {
                fromOneToTen.Add(i);
            }

            RowSelecor.ItemsSource = fromSixToFifteen;
            RowSelecor.SelectedItem = 15;

            ColSelecor.ItemsSource = fromSixToFifteen;
            ColSelecor.SelectedItem = 15;

            fileCounter.ItemsSource = fromOneToTen;
            fileCounter.SelectedItem = 3;
        }

        private void createCrossword()
        {
            CrosswordField.Children.Clear();
            CrosswordField.ColumnDefinitions.Clear();
            CrosswordField.RowDefinitions.Clear();

            int row = fromSixToFifteen[RowSelecor.SelectedIndex];
            int col = fromSixToFifteen[ColSelecor.SelectedIndex];

            for (int i = 0; i <  row; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                CrosswordField.RowDefinitions.Add(rowDefinition);
            }
            for (int i = 0; i < col;  i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                CrosswordField.ColumnDefinitions.Add(columnDefinition);
            }

            for(int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    TextBox newTextBox = new TextBox()
                    {
                        FontSize = 16,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextAlignment = TextAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Width = 25,
                        Height = 25,
                        Text = "-",
                        BorderBrush = Brushes.LightBlue,
                        BorderThickness = new Thickness(1),
                        MaxLength = 1
                    };

                    newTextBox.MouseDoubleClick += (s, e) =>
                    {   
                        if(newTextBox.Text == "-")
                        {
                            newTextBox.Text = "#";
                        }
                        else
                        {
                            newTextBox.Text = "-";
                        }
                        
                    };
                    Grid.SetRow(newTextBox, i);
                    Grid.SetColumn(newTextBox, j);
                    CrosswordField.Children.Add(newTextBox);
                }
            }
        }

        private void saveCrossword()
        {
            StringBuilder matrixBuilder = new StringBuilder();

            int row = fromSixToFifteen[RowSelecor.SelectedIndex];
            int col = fromSixToFifteen[ColSelecor.SelectedIndex];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    string cellText = string.Empty;

                    foreach (UIElement child in CrosswordField.Children)
                    {
                        if (Grid.GetRow(child) == i && Grid.GetColumn(child) == j && child is TextBox tb)
                        {
                            cellText = tb.Text ?? string.Empty;
                            break;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(cellText))
                    {
                        MessageBox.Show("None of the fields should be empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    matrixBuilder.Append($"{cellText}");
                }

                matrixBuilder.AppendLine();
            }

            string matrixString = matrixBuilder.ToString();

            string fullPath = System.IO.Path.Combine(".", $"kr{fromOneToTen[fileCounter.SelectedIndex]}.txt");

            try
            {
                File.WriteAllText(fullPath, matrixString, Encoding.UTF8);
                MessageBox.Show("File write is successful", "Successful file writing", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong", "File write error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateCrosswordBtn_Click(object sender, RoutedEventArgs e)
        {
            createCrossword();
        }

        private void SaveCrosswordBtn_Click(object sender, RoutedEventArgs e)
        {
            saveCrossword();
        }
    }
}