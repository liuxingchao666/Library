using Rfid系统.Model;
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

namespace Rfid系统.View
{
    /// <summary>
    /// FrequencyChooseControl.xaml 的交互逻辑
    /// </summary>
    public partial class FrequencyChooseControl : Window
    {
        public FrequencyChooseControl(List<HDDCQKInfo> hDDCQKInfo)
        {
            InitializeComponent();
            infos = hDDCQKInfo;
            List<HDDCQKInfo> tempList = new List<HDDCQKInfo>();
            foreach (var temp in infos)
            {
                if (temp.IsCheck)
                    tempList.Add(temp);
            }
            var list = (from c in infos
                        where !(from d in tempList
                                select d.id).Contains(c.id)
                        select c
                        ).ToList();
            LoadPage.Text = "1";
            int count = list.Count() / 9;
            if (list.Count() % 9 != 0)
                count++;
            CountPage.Content = count.ToString();
            this.countPage = count;
            if (count > 1)
            {
                int index = 8;
                while (index >= 0)
                {
                    ShowInfo.Add(list[8 - index]);
                    index--;
                }
            }
            else
            {
                countPage = 1;
                CountPage.Content = 1.ToString();
                LoadPage.Text = 1.ToString();
                foreach (var temp in list)
                    ShowInfo.Add(temp);
            }
            Bind.ItemsSource = tempList;
            grid.ItemsSource = ShowInfo;
            NBInfos = list;
        }
        public int countPage;
        public int loadPage = 1;
        /// <summary>
        /// 展示数据
        /// </summary>
        public List<HDDCQKInfo> ShowInfo = new List<HDDCQKInfo>();
        /// <summary>
        /// 操作数据
        /// </summary>
        public List<HDDCQKInfo> infos = new List<HDDCQKInfo>();
        /// <summary>
        /// 绑定数据
        /// </summary>
        public List<HDDCQKInfo> BingInfos = new List<HDDCQKInfo>();
        /// <summary>
        /// 上有次未绑定已查询记录
        /// </summary>
        public List<HDDCQKInfo> NBInfos = new List<HDDCQKInfo>();
        /// <summary>
        /// 筛选条件数据源
        /// </summary>
        public List<HDDCQKInfo> QueryInfo = new List<HDDCQKInfo>();

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            infos = Bind.ItemsSource as List<HDDCQKInfo>;
            this.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            List<HDDCQKInfo> infos = grid.ItemsSource as List<HDDCQKInfo>;
            if (!(bool)selectAll.IsChecked)
            {
                foreach (var temp in infos)
                {
                    temp.IsCheck = true;
                    if (!BingInfos.Contains(temp))
                        BingInfos.Add(temp);
                }
            }
            else
            {
                foreach (var temp in infos)
                {
                    temp.IsCheck = false;
                    BingInfos.Remove(temp);
                }
            }
            grid.ItemsSource = null;
            grid.ItemsSource = infos;

        }

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            query.Focus();
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            List<HDDCQKInfo> infos = grid.ItemsSource as List<HDDCQKInfo>;
            HDDCQKInfo info = grid.SelectedItem as HDDCQKInfo;
            if (!(bool)info.IsCheck)
            {
                foreach (var temp in infos)
                {
                    if (temp.id == info.id)
                    {
                        temp.IsCheck = true;
                        if (!BingInfos.Contains(temp))
                            BingInfos.Add(temp);
                        break;
                    }
                }
            }
            else
            {
                foreach (var temp in infos)
                {
                    if (temp.id == info.id)
                    {
                        temp.IsCheck = false;
                        BingInfos.Remove(temp);
                        break;
                    }
                }
            }
            bool result = true;
            foreach (var temp in infos)
            {
                if (!temp.IsCheck)
                {
                    result = false;
                    break;
                }
            }
            if (result)
                selectAll.IsChecked = true;
            else
                selectAll.IsChecked = false;
            grid.ItemsSource = null;
            grid.ItemsSource = infos;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ///刷新绑定数据
            List<HDDCQKInfo> list = Bind.ItemsSource as List<HDDCQKInfo>;
            if (QueryInfo.Count() <= 0)
            {
                foreach (var temp in BingInfos)
                {
                    list.Add(temp);
                    NBInfos.Remove(temp);
                }
                BingInfos.Clear();
            }
            else
            {
                List<string> IDList = new List<string>();
                foreach (var temp in BingInfos)
                {
                    foreach (var emp in QueryInfo)
                    {
                        if (temp.id == emp.id)
                        {
                            list.Add(temp);
                            NBInfos.Remove(temp);
                            IDList.Add(temp.id);
                        }
                    }
                }
                QueryInfo = (from c in QueryInfo
                             where !(from d in IDList
                                     select d).Contains(c.id)
                             select c
                        ).ToList();
                BingInfos.Clear();
            }
            Bind.ItemsSource = null;
            Bind.ItemsSource = list;
            ///刷新分页
            List<HDDCQKInfo> infos = new List<HDDCQKInfo>();
            if (QueryInfo.Count() == 0)
                list = NBInfos;
            else
                list = QueryInfo;
            int count = list.Count() / 9;
            if (list.Count() % 9 != 0)
                count++;
            if (count == 0)
                count = 1;
            countPage = count;
            CountPage.Content = count.ToString();
            if (loadPage >= countPage)
                loadPage = countPage;
            LoadPage.Text = loadPage.ToString();
            Flush(loadPage);
        }
        /// <summary>
        /// 解离
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HDDCQKInfo hDDCQKInfo = Bind.SelectedItem as HDDCQKInfo;
            List<HDDCQKInfo> list = Bind.ItemsSource as List<HDDCQKInfo>;
            list.Remove(hDDCQKInfo);
            hDDCQKInfo.IsCheck = false;
            Bind.ItemsSource = null;
            Bind.ItemsSource = list;
            int count = 0;
            if (QueryInfo.Count() == 0)
            {
                NBInfos.Add(hDDCQKInfo);
                count = NBInfos.Count();
            }
            else
            {
                NBInfos.Add(hDDCQKInfo);
                QueryInfo.Add(hDDCQKInfo);
                count = QueryInfo.Count();
            }
            if (count > countPage * 9)
                countPage++;
            CountPage.Content = countPage.ToString();
            Flush(loadPage);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Query_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lock (query.Text)
                {
                    string queryTxt = query.Text;
                    if (string.IsNullOrEmpty(queryTxt))
                    {
                        foreach (var temp in NBInfos)
                        {
                            QueryInfo.Add(temp);
                        }
                    }
                    else
                    {
                        QueryInfo.Clear();
                        foreach (var temp in NBInfos)
                        {
                            if (temp.code.Contains(queryTxt) || temp.anumber.Contains(queryTxt))
                                QueryInfo.Add(temp);
                        }
                    }
                    grid.ItemsSource = null;
                    if (QueryInfo.Count() == 0)
                    {
                        CountPage.Content = "1";
                        countPage = 1;
                        loadPage = 1;
                        LoadPage.Text = "1";
                        return;
                    }
                    int count = QueryInfo.Count() / 9;
                    if (QueryInfo.Count() % 9 != 0)
                        count++;
                    CountPage.Content = count.ToString();
                    countPage = count;
                    if (count == 1)
                    {
                        grid.ItemsSource = QueryInfo;
                        foreach (var temp in QueryInfo)
                            if (temp.IsCheck)
                                BingInfos.Add(temp);
                        loadPage = count;
                        LoadPage.Text = count.ToString();
                    }
                    else
                    {
                        loadPage = 1;
                        LoadPage.Text = 1.ToString();
                        int index = 8;
                        while (index >= 0)
                        {
                            ShowInfo.Add(QueryInfo[8 - index]);
                            index--;
                        }
                        grid.ItemsSource = ShowInfo;
                        foreach (var temp in ShowInfo)
                            if (temp.IsCheck && !BingInfos.Contains(temp))
                            {
                                BingInfos.Add(temp);
                            }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            List<HDDCQKInfo> list = Bind.ItemsSource as List<HDDCQKInfo>;
            infos = new List<HDDCQKInfo>();
            infos = list;
            this.Close();
        }

        private void First_Click(object sender, RoutedEventArgs e)
        {
            if (countPage.ToInt() <= 1)
                return;
            Flush(1);
            loadPage = 1;
            LoadPage.Text = 1.ToString();
        }

        private void Last_Click(object sender, RoutedEventArgs e)
        {
            if (countPage.ToInt() <= 1 || loadPage == 1)
                return;
            loadPage = loadPage - 1;
            Flush(loadPage);
            LoadPage.Text = loadPage.ToString();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (countPage.ToInt() <= 1 || loadPage == countPage)
                return;
            loadPage = loadPage + 1;
            Flush(loadPage);
            LoadPage.Text = loadPage.ToString();
        }

        private void LastOne_Click(object sender, RoutedEventArgs e)
        {
            if (countPage.ToInt() <= 1 || loadPage == countPage)
                return;
            loadPage = countPage;
            Flush(loadPage);
            LoadPage.Text = loadPage.ToString();
        }
        public void Flush(int page)
        {
            ShowInfo.Clear();
            List<HDDCQKInfo> list = new List<HDDCQKInfo>();
            if (QueryInfo.
                Count() == 0)
                list = NBInfos;
            else
                list = QueryInfo;
            if (page == countPage)
            {
                int count = (page - 1) * 9;
                while (count <= list.Count() - 1)
                {
                    ShowInfo.Add(list[count]);
                    count++;
                }
            }
            else
            {
                int count = (page - 1) * 9;
                while (count <= page * 9 - 1)
                {
                    ShowInfo.Add(list[count]);
                    count++;
                }
            }
            bool result = true;
            foreach (var temp in ShowInfo)
            {
                if (!temp.IsCheck)
                {
                    result = false;
                    break;
                }
            }
            if (result)
                selectAll.IsChecked = true;
            else
                selectAll.IsChecked = false;
            grid.ItemsSource = null;
            grid.ItemsSource = ShowInfo;
        }

        private void Bind_GotFocus(object sender, RoutedEventArgs e)
        {
            query.Focus();
        }
    }
}
