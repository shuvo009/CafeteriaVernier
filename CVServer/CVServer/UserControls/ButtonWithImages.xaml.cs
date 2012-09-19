using System;
using System.Collections.Generic;
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
	/// Interaction logic for ButtonWithImages.xaml
	/// </summary>
	public partial class ButtonWithImages : UserControl,ICommandSource
	{
		public ButtonWithImages()
		{
			this.InitializeComponent();
		}

        #region Dependency property
        /// <summary>
        /// To register Image In Button
        /// </summary>
        public static readonly DependencyProperty ButtonImagesProperty = DependencyProperty.Register("ButtonImages", typeof(ImageSource), typeof(ButtonWithImages), new UIPropertyMetadata(null));

        /// <summary>
        /// to register content in Button
        /// </summary>
        public new static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(string), typeof(ButtonWithImages), new UIPropertyMetadata(null));

        /// <summary>
        /// To register Font Size
        /// </summary>
        //public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register("FontSize", typeof(double), typeof(ButtonWithImages), new UIPropertyMetadata(null));
         /// <summary>
        /// To register FontFamily
        /// </summary>
        public new static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(ButtonWithImages), new UIPropertyMetadata(null));
        #endregion

        #region Property
        /// <summary>
        /// Get or Set Image In Button
        /// </summary>
        public ImageSource ButtonImages
        {
            get { return (ImageSource)GetValue(ButtonImagesProperty); }
            set { SetValue(ButtonImagesProperty, value); }
        }

        /// <summary>
        ///Get or Set Content of button
        /// </summary>
        public new string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Get Or Set Font Size
        /// </summary>
        //public double FontSize
        //{
        //    get { return (double)GetValue(FontSizeProperty); }
        //    set { SetValue(FontSizeProperty, value); }
        //}

        /// <summary>
        /// Get And Set FontFamily
        /// </summary>
        public new FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set {SetValue(FontFamilyProperty,value);}
        }
        #endregion

        #region Event Handelar
        /// <summary>
        /// Button Click Handler
        /// </summary>
        public event RoutedEventHandler Click;
        #endregion

        #region Private Eventes
        /// <summary>
        /// Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
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

        #region Command
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ButtonWithImages), new PropertyMetadata(null));
       
        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }
        #endregion

        #region Command Paramete
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(ButtonWithImages), new PropertyMetadata(null));
       
        public object CommandParameter
        {
            get { return this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }
        #endregion

        #region Command Target
        public static readonly DependencyProperty CommandTragetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(ButtonWithImages), new PropertyMetadata(null));
        public IInputElement CommandTarget
        {
            get { return (IInputElement)this.GetValue(CommandTragetProperty); }
            set { this.SetValue(CommandTragetProperty, value); }
        }
        #endregion
    }
}