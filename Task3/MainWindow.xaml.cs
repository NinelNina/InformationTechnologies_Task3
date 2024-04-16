using System;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Task2.Model;
using Animal = Task2.Model.Animal;
using Dog = Task2.Model.Dog;
using Panther = Task2.Model.Panther;
using Turtle = Task2.Model.Turtle;

namespace Task3;

public partial class MainWindow : Window
{
    private Type? selectedType;
    private object createdInstance;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "DLL Files (*.dll)|*.dll";
        if (openFileDialog.ShowDialog() == true)
        {
            string assemblyName = openFileDialog.FileName;
            var assembly = Assembly.LoadFile(assemblyName);
            Type abstractClassType = assembly.GetType("Task2.Model.Animal");
            Type[] implementedClasses = GetImplementedClasses(assembly.GetTypes(), abstractClassType);

            FillComboBox(implementedClasses);
        }
    }

    private void FillComboBox(Type[] types)
    {
        ClassListBox.ItemsSource = types.Select(t => t.FullName);
    }

    private Type[] GetImplementedClasses(Type[] types, Type subtype)
    {
        return types.Where(t => t.IsClass)
            .Where(t => t.IsSubclassOf(subtype))
            .Where(t => t.IsAbstract == false).ToArray();
    }

    private void ClassListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selectedClassName = ClassListBox.SelectedItem as string;
        if (!string.IsNullOrEmpty(selectedClassName))
        {
            Assembly assem = typeof(Animal).Assembly;
            selectedType = assem.GetType(selectedClassName);
            if (selectedType != null)
            {
                ConstructorInfo[] constructors = selectedType.GetConstructors();
                ConstructorStackPanel.Children.Clear();
                foreach (ConstructorInfo constructor in constructors)
                {
                    StackPanel panel = new StackPanel();
                    panel.Orientation = Orientation.Horizontal;
                    panel.Margin = new Thickness(0, 5, 0, 5);

                    ParameterInfo[] parameters = constructor.GetParameters();
                    foreach (ParameterInfo param in parameters)
                    {
                        TextBox textBox = new TextBox();
                        textBox.Margin = new Thickness(5, 0, 0, 0);
                        textBox.Tag = param.ParameterType;
                        textBox.Text = param.Name;
                        panel.Children.Add(textBox);
                    }

                    ConstructorStackPanel.Children.Add(panel);
                }
                ExecuteConstructorButton.IsEnabled = true;
            }
        }
    }

    private void ExecuteConstructorButton_Click(object sender, RoutedEventArgs e)
    {
        if (selectedType != null)
        {
            try
            {
                List<object> parameters = new List<object>();
                foreach (StackPanel panel in ConstructorStackPanel.Children)
                {
                    foreach (TextBox textBox in panel.Children)
                    {
                        object value = Convert.ChangeType(textBox.Text, (Type)textBox.Tag);
                        parameters.Add(value);
                    }
                }
                createdInstance = Activator.CreateInstance(selectedType, parameters.ToArray());

                if (createdInstance is IVoicalizable voicalizable)
                {
                    voicalizable.Voice += OnVoiceEvent;
                }

                if (createdInstance is Panther panther)
                {
                    panther.ClimbTree += OnClimbTreeEvent;
                }


                MethodInfo[] methods = selectedType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

                methods = methods.Where(m => !m.GetParameters().Any(p => p.ParameterType == typeof(EventHandler<VoiceEventArgs>) || p.ParameterType == typeof(EventHandler<ClimbTreeEventArgs>))).ToArray();

                MethodComboBox.ItemsSource = methods;
                ExecuteMethodButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating instance: " + ex.Message);
            }
        }
    }

    private void OnVoiceEvent(object sender, VoiceEventArgs e)
    {
        MessageBox.Show($"Voice event raised: {e.VoiceMessage}");
    }

    private void OnClimbTreeEvent(object sender, ClimbTreeEventArgs e)
    {
        MessageBox.Show($"ClimbTree event raised: {e.ClimbTreeMessage}");
    }

    private void MethodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        MethodInfo selectedMethod = (MethodInfo)MethodComboBox.SelectedItem;
        if (selectedMethod != null)
        {
            MethodParameterStackPanel.Children.Clear();
            ParameterInfo[] parameters = selectedMethod.GetParameters();
            foreach (ParameterInfo param in parameters)
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Margin = new Thickness(0, 5, 0, 5);

                Label label = new Label();
                label.Content = $"{param.ParameterType.Name} {param.Name}:";
                label.Margin = new Thickness(0, 0, 5, 0);
                stackPanel.Children.Add(label);

                TextBox textBox = new TextBox();
                textBox.Tag = new Tuple<Type, string>(param.ParameterType, param.Name);
                textBox.Text = string.Empty;
                textBox.Width = 100;
                stackPanel.Children.Add(textBox);

                MethodParameterStackPanel.Children.Add(stackPanel);
            }
            ExecuteMethodButton.IsEnabled = true;
        }
    }

    private void ExecuteMethodButton_Click(object sender, RoutedEventArgs e)
    {
        MethodInfo selectedMethod = (MethodInfo)MethodComboBox.SelectedItem;
        if (selectedMethod != null)
        {
            try
            {
                List<object> parameters = new List<object>();
                foreach (StackPanel stackPanel in MethodParameterStackPanel.Children)
                {
                    TextBox textBox = (TextBox)stackPanel.Children[1];
                    Tuple<Type, string> tagInfo = (Tuple<Type, string>)textBox.Tag;
                    Type parameterType = tagInfo.Item1;
                    string parameterName = tagInfo.Item2;

                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        throw new ArgumentException($"No value provided for the '{parameterName}' parameter.");
                    }

                    object value = Convert.ChangeType(textBox.Text, parameterType);
                    parameters.Add(value);
                }

                object result = selectedMethod.Invoke(createdInstance, parameters.ToArray());

                if (result != null)
                {
                    MessageBox.Show("Method execution result: " + result.ToString());
                }
                else
                {
                    MessageBox.Show("Method executed successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing method: " + ex.Message);
            }
        }
    }

}
