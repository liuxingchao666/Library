using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using WpfApp2.Controls;
using WpfApp2.DAL;
using WpfApp2.Model;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public  class QueryViewModel : NotificationObject
    {
        public MainPage Page;
        public QueryViewModel(MainPage page)
        {
            Page = page;
           // TxtCommand = "请输入查询所借的书籍名";
        }
        private string txtCommand { get; set; }
        public string TxtCommand
        {
            get { return txtCommand; }
            set
            {
                txtCommand = value;
                RaisePropertyChanged("TxtCommand");
            }
        }
        private ICommand _BackCommand;
        public ICommand BackCommand
        {
            get
            {
                return _BackCommand ?? (_BackCommand = new DelegateCommand<QueryControl>((data) =>
                {
                    BacAction(data);
                }));
            }
        }

        private ICommand hotSearchCommand;
        public ICommand HotSearchCommand
        {
            get
            {
                return hotSearchCommand ?? (hotSearchCommand = new DelegateCommand<Button>((button) =>
                  {
                      Host(button);
                  }));
            }
        }

        public static ICommand _SelectCommand;
        public  ICommand SelectCommand
        {
            get
            {
                return _SelectCommand ?? (_SelectCommand = new DelegateCommand<TextBox>((data) =>
                {
                    SelectListAction(data);
                }));
            }
        }
        /// <summary>
        /// 搜索
        /// </summary>
        public void SelectListAction(TextBox data)
        {
            if (string.IsNullOrEmpty(data.Text))
            {
                return;
            }
            Page.QueryControl.times = 0;
            Page.QueryControl.timer.Abort();
            
            dealInfo dealInfo = new dealInfo() { paramter=data.Text,Class=ListClass.classList};
            IntroducePage introduce = new IntroducePage(Page, dealInfo,GridClass.BookClassList);
            Page.Grid.Children.Clear();
            Page.Grid.Children.Add(introduce);
        }
        /// <summary>
        ///返回
        /// </summary>
        public void BacAction(QueryControl data)
        {
            data.timer.Abort();
            data.times = 0;
            Page.Grid.Children.Clear();
            MainControl main = new MainControl(Page);
            Page.Grid.Children.Add(main);
            ServerSeting.ISAdd = true;
            ServerSeting.SYSleepTimes = 60;
        }
        public void Host(Button button)
        {
            Page.QueryControl.timer.Abort();
            Page.QueryControl.times = 0;
            dealInfo dealInfo = new dealInfo() { paramter = button.Tag, Class = ListClass.classList };
            object obj = dealInfo;
            IntroducePage introduce = new IntroducePage(Page, obj, GridClass.BookClassList);

            Page.Grid.Children.Clear();
            Page.Grid.Children.Add(introduce);
        }
    }
}
