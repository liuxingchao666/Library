using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp2.Controls;
using WpfApp2.DAL;

namespace WpfApp2.View
{
    /// <summary>
    /// Sleeping.xaml 的交互逻辑
    /// </summary>
    public partial class Sleeping : UserControl
    {
        public Sleeping(MainPage page)
        {
            InitializeComponent();
            this.page = page;
        }
        public MainPage page;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
            page.Grid.Children.Clear();

            MainControl control = new MainControl(page);
            page.mainControl = control;
            ServerSeting.Mus = true;
            page.Grid.Children.Add(control);
#pragma warning disable CS0618 // “Thread.Resume()”已过时:“Thread.Resume has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202”
            page.thread.Resume();
#pragma warning restore CS0618 // “Thread.Resume()”已过时:“Thread.Resume has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202”
#pragma warning disable CS0618 // “Thread.Resume()”已过时:“Thread.Resume has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202”
            page.threadSleep.Resume();
#pragma warning restore CS0618 // “Thread.Resume()”已过时:“Thread.Resume has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202”
        }
    }
}
