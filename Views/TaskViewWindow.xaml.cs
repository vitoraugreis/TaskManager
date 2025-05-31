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

    // Expande o formulário para a adição de uma nova tarefa.
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

    // Salva a nova tarefa com o clique do usuário ( verificações são feitas ).
    private void AddTaskButton_Click(object sender, RoutedEventArgs e)
    {
        string title = TitleTextBox.Text.Trim();
        string description = DescriptionTextBox.Text.Trim();
        DateTime? completionDate = null;
        bool hasCompletionTime = false;

        if (EnableCompletionDateCheckBox.IsChecked == true)
        {
            hasCompletionTime = SpecifyTimeCheckBoxAddTask.IsChecked == true;
            if (CompletionDatePicker.SelectedDate.HasValue)
            {
                DateTime selectedDateOnly = CompletionDatePicker.SelectedDate.Value.Date;
                if (hasCompletionTime)
                {
                    int hour = 0; int minute = 0;
                    bool hourValid = int.TryParse(NewTaskHourTextBox.Text, out hour) && hour >= 0 && hour <= 23;
                    bool minuteValid = int.TryParse(NewTaskMinuteTextBox.Text, out minute) && minute >= 0 && minute <= 59;
                    if (hourValid && minuteValid)
                    {
                        completionDate = new DateTime(selectedDateOnly.Year, selectedDateOnly.Month, selectedDateOnly.Day, hour, minute, 0);
                    }
                    else
                    {
                        MessageBox.Show("Hora ou minuto inválido. Apenas a data será salva (sem hora específica).", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                        completionDate = selectedDateOnly;
                        hasCompletionTime = false;
                    }
                }
                else
                {
                    completionDate = selectedDateOnly;
                }
            }
        }

        if (string.IsNullOrEmpty(title))
        {
            MessageBox.Show("O título da tarefa é obrigatório.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            TitleTextBox.Focus();
            return;
        }

        _viewModel.AddTask(title, description, completionDate, hasCompletionTime);

        ClearNewTaskForm();
        NewTaskFormPanel.Visibility = Visibility.Collapsed;
    }

    // Limpa as informações da data e horário da tarefa.
    private void ClearCompletionDateButton_Click(object sender, RoutedEventArgs e)
    {
        EnableCompletionDateCheckBox.IsChecked = false;
        CompletionDatePicker.SelectedDate = null;
        SpecifyTimeCheckBoxAddTask.IsChecked = false;
        NewTaskHourTextBox.Text = "00";
        NewTaskMinuteTextBox.Text = "00";
    }

    // Cancela a criação de tarefa.
    private void CancelAddTaskButton_Click(object sender, RoutedEventArgs e)
    {
        ClearNewTaskForm();
        NewTaskFormPanel.Visibility = Visibility.Collapsed;
    }

    // Limpa o formulário de criação de tarefa.
    private void ClearNewTaskForm()
    {
        TitleTextBox.Clear();
        DescriptionTextBox.Clear();
        EnableCompletionDateCheckBox.IsChecked = false;
        CompletionDatePicker.SelectedDate = null;
        SpecifyTimeCheckBoxAddTask.IsChecked = false;
        NewTaskHourTextBox.Text = "00";
        NewTaskMinuteTextBox.Text = "00";
    }

    // Mostra as opções que podem ser feitas com a tarefa.
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

    // Remove a tarefa específica.
    private void RemoveTaskMenu_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuItem menuItem)
        {
            UserTask? taskToRemove = menuItem.DataContext as UserTask;

            if (taskToRemove == null && menuItem.Parent is ContextMenu contextMenu && contextMenu.PlacementTarget is FrameworkElement placementTarget)
                taskToRemove = placementTarget.DataContext as UserTask;

            if (taskToRemove != null)
            {
                MessageBoxResult confirmation = MessageBox.Show(
                    $"Tem certeza que deseja remover a tarefa: \"{taskToRemove.Title}\"?",
                    "Confirmar Remoção",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (confirmation == MessageBoxResult.Yes)
                    _viewModel.RemoveTask(taskToRemove);
            }
            else
            {
                MessageBox.Show("Não foi possível identificar a tarefa para remoção.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Vai para o processo de edição da tarefa.
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
            editWindow.Owner = this;

            bool? dialogResult = editWindow.ShowDialog();

            if (dialogResult == true)
            {
                // O usuário clicou em "Salvar".
                originalTask.Title = taskClone.Title;
                originalTask.Description = taskClone.Description;
                originalTask.CompletionDate = taskClone.CompletionDate;
                originalTask.HasCompletionTime = taskClone.HasCompletionTime;
                _viewModel.UpdateTask(originalTask);
            }
        }
        else
        {
            MessageBox.Show("Não foi possível identificar a tarefa para edição.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

