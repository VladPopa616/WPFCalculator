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
            //const string Number = @"^\d+$";
            const string operation = @"^(\+|\-|\*|\/)$";
            const string specialOperation = @"^(\+\/\-|\%|AC|=|\.)$";

            RoutedEventHandler events = (a, b) => Trace.WriteLine("Unknown");

            //if (Regex.IsMatch(content, Number))
            //{
                //events = (a, b) => HandleNumber(content);
            //}

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

        private void HandleNumber(object sender,RoutedEventArgs eventArgs)
        {
            string number = ((Button)eventArgs.Source).Content.ToString();
            HandleSecondNumber();

            if(operators != null && lastNumber == null)
            {
                lastNumber = double.Parse(Result.Content.ToString());
                Result.Content = number;
            }
            else
            {
                Result.Content = Result.Content.ToString().Equals("0") ? number : Result.Content + number;
            }
        }

        private void HandleEquals(object sender, RoutedEventArgs eventArgs)
        {
            try
            {
                result = DoOperation(operators, double.Parse(lastNumber.ToString()), double.Parse(Result.Content.ToString()));
                Result.Content = result;
            }
            catch (Exception ex) when (ex is ArithmeticException || ex is DivideByZeroException)
            {
                MessageBox.Show("Invalid operation", "An error has occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
            Reset();
        }

        private void HandleAC(object sender, RoutedEventArgs eventArgs)
        {
            Reset();
            Result.Content = "0";
        }

        private void HandlePercent(object sender, RoutedEventArgs eventArgs)
        {
            result = null;
            Result.Content = double.Parse(Result.Content.ToString()) * ((lastNumber ?? 1) / 100);
        }

        private void HandleDecimalPeriod(object sender, RoutedEventArgs eventArgs)
        {
            HandleSecondNumber();
            if (!Result.Content.ToString().Contains("."))
            {
                Result.Content += ".";
            }
        }

        private void HandlePlusMinus(object sender, RoutedEventArgs eventArgs)
        {
            Result.Content = double.Parse(Result.Content.ToString()) * -1;
        }

        private RoutedEventHandler AssignSpecialOperationHandler(string operation)
        {
            switch (operation)
            {
                case "=": return HandleEquals; 
                case "AC": return HandleAC;
                case "+/-": return HandlePlusMinus;
                case "%": return HandlePercent;
                case ".": return HandleDecimalPeriod;
                default: return (a, b) => Trace.WriteLine("Unknown");

            }
        }

        private double? DoOperation(SelectedOperator? so,double x, double y)
        {
            switch (so)
            {
                case SelectedOperator.Addition: return OperationHandlers.Add(x, y);
                case SelectedOperator.Subtraction: return OperationHandlers.Subtract(x, y);
                case SelectedOperator.Multiplication: return OperationHandlers.Multiply(x, y);
                case SelectedOperator.Division: return OperationHandlers.Divide(x, y);
                default: return null;
            }
        }

        private void Reset()
        {
            lastNumber = null;
            operators = null;
        }
    }
}
