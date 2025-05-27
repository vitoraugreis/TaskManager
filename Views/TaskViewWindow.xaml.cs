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
        NewTaskHourTextBox.Text = "00"; // Limpa/reseta campo de hora
        NewTaskMinuteTextBox.Text = "00"; // Limpa/reseta campo de minuto
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
        DateTime? completionDate = null;
        if (CompletionDatePicker.SelectedDate.HasValue)
        {
            DateTime selectedDate = CompletionDatePicker.SelectedDate.Value;
            int hour = 0;
            int minute = 0;

            // Tenta parsear, se falhar ou estiver vazio, usa 00:00
            if (int.TryParse(NewTaskHourTextBox.Text, out int parsedHour) && parsedHour >= 0 && parsedHour <= 23)
            {
                hour = parsedHour;
            }
            if (int.TryParse(NewTaskMinuteTextBox.Text, out int parsedMinute) && parsedMinute >= 0 && parsedMinute <= 59)
            {
                minute = parsedMinute;
            }
            completionDate = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);
        }

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
            if (button.ContextMenu != null)
            {
                button.ContextMenu.PlacementTarget = button;
                button.ContextMenu.IsOpen = true;
            }
        }
    }

    private void RemoveTaskMenu_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
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
                    _viewModel.RemoveTask(taskToRemove);
                }
            }
            else
            {
                MessageBox.Show("Não foi possível identificar a tarefa para remoção.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void EditTaskMenu_Click(object sender, RoutedEventArgs e)
    {
        UserTask? originalTask = null;
        if (sender is MenuItem menuItem)
        {
            originalTask = menuItem.DataContext as UserTask;
            if (originalTask == null && menuItem.Parent is ContextMenu contextMenu && contextMenu.PlacementTarget is FrameworkElement placementTarget)
                originalTask = placementTarget.DataContext as UserTask;
        }

        if (originalTask != null)
        {
            UserTask taskClone = new UserTask(originalTask);

            EditTaskWindow editWindow = new EditTaskWindow(taskClone);
            editWindow.Owner = this; // Define a janela principal como "dona" da dialog

            bool? dialogResult = editWindow.ShowDialog();

            if (dialogResult == true)
            {
                // O usuário clicou em "Salvar".
                originalTask.Title = taskClone.Title;
                originalTask.Description = taskClone.Description;
                originalTask.CompletionDate = taskClone.CompletionDate;
                _viewModel.UpdateTask(originalTask);
            }
        }
        else
        {
            MessageBox.Show("Não foi possível identificar a tarefa para edição.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

