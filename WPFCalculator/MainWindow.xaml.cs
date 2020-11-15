using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static double? lastNumber, result;
        private static SelectedOperator? operators;

        private enum SelectedOperator
        {
            Addition,
            Subtraction,
            Multiplication,
            Division
        }
        public MainWindow()
        {
            InitializeComponent();

            foreach (var x in contents.Children)
            {
                if(x is Button button && !Regex.IsMatch(button.Content.ToString(), @"\d+"))
                {
                    button.Click += AssignClick(button.Content.ToString());
                }
            }
        }

        private RoutedEventHandler AssignClick(string content)
        {
            const string operation = @"^(\+|\-|\*|\/)$";
            const string specialOperation = @"^(\+\/\-|\%|AC|=|\.)$";

            RoutedEventHandler events = (a, b) => Trace.WriteLine("Unknown");

            if (Regex.IsMatch(content, operation))
            {
                events = (a, b) => HandleOperation(content);
            }
            else if (Regex.IsMatch(content, specialOperation))
            {
                events = AssignSpecialOperationHandler(content);
            }
            return events;
        }

        private void HandleSecondNumber()
        {
            if (result == null) return;
            result = null;
            Result.Content = "0";
        }

        private void HandleOperation(string operation)
        {
            result = null;

            switch (operation)
            {
                case "+": operators = SelectedOperator.Addition; break;
                case "-": operators = SelectedOperator.Subtraction; break;
                case "*": operators = SelectedOperator.Multiplication; break;
                case "/": operators = SelectedOperator.Division; break;

            }
        }
    }
}
