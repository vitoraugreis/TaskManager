using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManager.Models;
using TaskManager.Data;
using TaskManager.ViewModels;
using TaskManager.Exceptions;

namespace TaskManager.Views;

public partial class TaskViewWindow : Window
{
    private TaskViewModel _viewModel;

    public TaskViewWindow(User user)
    {
        InitializeComponent();
        _viewModel = new TaskViewModel(user);
        DataContext = _viewModel;
    }

    private void ClearNewTaskForm()
    {
        TitleTextBox.Clear();
        DescriptionTextBox.Clear();
        CompletionDatePicker.SelectedDate = null;
    }

    private void ShowNewTaskFormButton_Click(object sender, RoutedEventArgs e)
    {
        if (NewTaskFormPanel.Visibility == Visibility.Collapsed)
        {
            NewTaskFormPanel.Visibility = Visibility.Visible;
            TitleTextBox.Focus();
        }
        else
        {
            NewTaskFormPanel.Visibility = Visibility.Collapsed;
            ClearNewTaskForm();
        }
    }

    private void AddTaskButton_Click(object sender, RoutedEventArgs e)
    {
        string title = TitleTextBox.Text.Trim();
        string description = DescriptionTextBox.Text.Trim();
        DateTime? completionDate = CompletionDatePicker.SelectedDate;

        if (string.IsNullOrEmpty(title))
        {
            MessageBox.Show("O título da tarefa é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            TitleTextBox.Focus();
            return;
        }

        _viewModel.AddTask(title, string.IsNullOrWhiteSpace(description) ? null : description, completionDate);

        ClearNewTaskForm();
        NewTaskFormPanel.Visibility = Visibility.Collapsed;
    }

    private void CancelAddTaskButton_Click(object sender, RoutedEventArgs e)
    {
        ClearNewTaskForm();
        NewTaskFormPanel.Visibility = Visibility.Collapsed;
    }

    private void TaskOptionsButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            // Abre o ContextMenu associado ao botão
            if (button.ContextMenu != null)
            {
                button.ContextMenu.PlacementTarget = button; // Define o botão como o alvo de posicionamento
                button.ContextMenu.IsOpen = true;          // Abre o menu
            }
        }
    }

    private void RemoveTaskMenu_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            // O DataContext do MenuItem geralmente é herdado.
            // Se não for direto, podemos pegá-lo do PlacementTarget do ContextMenu (que será o botão "...").
            UserTask? taskToRemove = menuItem.DataContext as UserTask;

            if (taskToRemove == null && menuItem.Parent is ContextMenu contextMenu && contextMenu.PlacementTarget is FrameworkElement placementTarget)
            {
                taskToRemove = placementTarget.DataContext as UserTask;
            }

            if (taskToRemove != null)
            {
                MessageBoxResult confirmation = MessageBox.Show(
                    $"Tem certeza que deseja remover a tarefa: \"{taskToRemove.Title}\"?",
                    "Confirmar Remoção",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (confirmation == MessageBoxResult.Yes)
                {
                    _viewModel.RemoveTask(taskToRemove); // Chama o método do ViewModel
                }
            }
            else
            {
                MessageBox.Show("Não foi possível identificar a tarefa para remoção.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                // Para depuração:
                // var dc = menuItem.DataContext;
                // var parentDc = (menuItem.Parent as ContextMenu)?.PlacementTarget?.DataContext;
                // System.Diagnostics.Debug.WriteLine($"MenuItem DC: {dc}, PlacementTarget DC: {parentDc}");
            }
        }
    }
}

