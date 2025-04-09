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
using TaskManager.ViewModels;

namespace TaskManager.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainViewModel _viewModel;
    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainViewModel();
    }

    private void NewTaskClick(object sender, RoutedEventArgs e)
    {
        string title = TitleTextBox.Text;
        string? description = DescriptionTextBox.Text;
        DateTime? dueDate = DueDatePicker.SelectedDate;

        if (string.IsNullOrWhiteSpace(title))
        {
            MessageBox.Show("O título da tarefa é obrigatório!");
            return;
        }

        _viewModel.CreateTask(1, title, description, dueDate);

        TaskListView.ItemsSource = null;
        TaskListView.ItemsSource = _viewModel.GetTasksByUser(1);

        TitleTextBox.Text = "";
        DescriptionTextBox.Text = "";
        DueDatePicker.SelectedDate = null;

        MessageBox.Show("Tarefa criada com sucesso!");
    }

    private void NewUserClick(object sender, RoutedEventArgs e)
    {
        string name = UserNameTextBox.Text;
        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("O nome do usuário é obrigatório!");
            return;
        }

        _viewModel.CreateUser(name);
        MessageBox.Show("Usuário criado com sucesso!");
    }
}
