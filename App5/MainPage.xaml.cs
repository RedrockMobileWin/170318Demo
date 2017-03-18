using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace App5
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static SQLiteAsyncConnection db { get; set; }
        ObservableCollection<student> list = new ObservableCollection<student>();
        public MainPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Height < 600)
                {
                    state = "VisualState600";
                }
                VisualStateManager.GoToState(this, state, true);
            };
        }

        public async void SQL()
        {
            db = new SQLiteAsyncConnection("Student.db");
            await db.CreateTableAsync<student>();
            student newstu = new student { Name = input.Text };
            await db.InsertAsync(newstu);
            var query = await (db.Table<student>().Where(v => v._Id >= 1)).ToListAsync();
            list = new ObservableCollection<student>(query);
            listview.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SQL();
        }

        private async void refresh_Click(object sender, RoutedEventArgs e)
        {
            db = new SQLiteAsyncConnection("Student.db");
            await db.CreateTableAsync<student>();
            var query = await(db.Table<student>().Where(v => v._Id >= 1)).ToListAsync();
            list = new ObservableCollection<student>(query);
            listview.ItemsSource = list;
        }
    }
}

public class student
{
    [PrimaryKey, AutoIncrement]
    public int _Id { get; set; }
    public string Name { get; set; }
}