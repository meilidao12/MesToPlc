﻿using Services;
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
using System.Windows.Shapes;
using WpfServers;
using MesToPlc.Models;
using Services.DataBase;
using System.Diagnostics;
namespace MesToPlc
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        IniHelper ini = new IniHelper(System.AppDomain.CurrentDomain.BaseDirectory + @"\Set.ini");
        //string PicUri = System.AppDomain.CurrentDomain.BaseDirectory + "/ico/";
        //SqlHelper sql = new SqlHelper();
        AccessHelper sql = new AccessHelper();
        public Login()
        {
            InitializeComponent();
            WindowFactory.windowbackhome += HomeEvent;
            this.Closing += Login_Closing;
            this.txtUserName.Text = "admin";
            this.txtPassWord.Text = "admin";
        }

        public Login(string Name) 
        {
            Debug.WriteLine("切换用户");
        }

        private void Login_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HomeEvent();
        }

        private void HomeEvent()
        {
            this.DialogResult = false;
            WindowFactory.windowbackhome -= HomeEvent;
            WindowFactory.BackHome_Event();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string commandText = "SELECT * FROM [User]";
            List<UserModel> users = sql.GetDataTable<UserModel>(commandText);
            if (users == null) return;
            foreach(var item in users)
            {
                if(item.UserName == this.txtUserName.Text)
                {
                    if(item.PassWord == this.txtPassWord.Text)
                    {
                        ini.WriteIni("Config", "UserName", this.txtUserName.Text);
                        HomeEvent();
                        return;
                    }
                    MessageBox.Show("密码不正确");
                    return;
                }
            }
            MessageBox.Show("用户名不正确");
        }

        private void btnCancle_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
