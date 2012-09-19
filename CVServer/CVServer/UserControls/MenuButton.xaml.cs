using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Procesta.CvServer.UserControls
{
    /// <summary>
    /// Interaction logic for MenuButton.xaml
    /// </summary>
    public partial class MenuButton : UserControl ,ICommandSource
    {
        public MenuButton()
        {
            InitializeComponent();
           // this.DataContext = this;
            this.toolTipGrid.DataContext = this;
        }
        // Set Image On Button
        #region Show Image
        public static readonly DependencyProperty ButtonImageProperty = DependencyProperty.Register("ButtonImage", typeof(ImageSource), typeof(MenuButton), new PropertyMetadata(null));

        public ImageSource ButtonImage
        {
            get { return (ImageSource)this.GetValue(ButtonImageProperty); }
            set { this.SetValue(ButtonImageProperty, value); }
        }
        #endregion
        // Set supper tip on Tooltip 
        #region Show supper tip
        public static readonly DependencyProperty SupperTipProperty = DependencyProperty.Register("SupperTip", typeof(string), typeof(MenuButton), new PropertyMetadata(null));

        public string SupperTip
        {
            get { return (string)this.GetValue(SupperTipProperty); }
            set { this.SetValue(SupperTipProperty, value); }
        }
        #endregion
        // Set Screen tip on tooltip
        #region Show Screen Tip
        public static readonly DependencyProperty ScreeenTipProperty = DependencyProperty.Register("ScreenTip", typeof(string), typeof(MenuButton), new PropertyMetadata(null));

        public string ScreenTip
        {
            get { return (string)this.GetValue(ScreeenTipProperty); }
            set { this.SetValue(ScreeenTipProperty, value); }
        }
        #endregion
        // Set Keyboard shortcut on tooltip
        #region show Keyboard shorcut
        public static readonly DependencyProperty KeyboardShorcutProperty = DependencyProperty.Register("KeyboardShorcut", typeof(string), typeof(MenuButton), new PropertyMetadata(null));

        public string KeyboardShorcut
        {
            get { return (string)this.GetValue(KeyboardShorcutProperty); }
            set { this.SetValue(KeyboardShorcutProperty, value); }
        }
        #endregion

        #region Command 
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(MenuButton), new PropertyMetadata(null));
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty);}
            set { SetValue(CommandProperty, value); }
        }
        #endregion

        #region Command Parameter 
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(MenuButton), new PropertyMetadata(null));
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty,value); }
        }
        #endregion

        #region Command Target
       // public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(MenuItem), new PropertyMetadata(null));
        public IInputElement CommandTarget
        {
            //get { return (IInputElement)GetValue(CommandTargetProperty); }
            //set { SetValue(CommandTargetProperty, value); }
            get { return default(IInputElement);}
        }
        #endregion

        #region Button on Click
        private void OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
        	var command = Command;
            var parameter = CommandParameter;
            var target = CommandTarget;

            var routedCmd = command as RoutedCommand;
            if (routedCmd != null && routedCmd.CanExecute(parameter, target))
            {
                routedCmd.Execute(parameter, target);
            }
            else if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }
        #endregion
    }
}
