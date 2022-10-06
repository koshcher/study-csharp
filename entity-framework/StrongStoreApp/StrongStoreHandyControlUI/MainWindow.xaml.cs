using HandyControl.Controls;
using HandyControl.Themes;
using HandyControl.Tools;
using StoreDbLibrary;
using StoreDbLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StrongStoreHandyControlUI
{
    public partial class MainWindow
    {
        private int currentPublisherId = 0;
        private int currentUserId = 0;
        private StoreDbContext db;
        private bool isGame = false;
        private bool isStart = true; // not double load store when start

        public MainWindow()
        {
            InitializeComponent();
            db = new StoreDbContext();
            registerUserBirthDate.DisplayDate = DateTime.Now;
            typeComboBox.SelectedIndex = 0;
            isStart = false;
            sortComboBox.SelectedIndex = 0;
        }

        private async void AddToLibrary(object sender, RoutedEventArgs e)
        {
            if (currentUserId == 0)
            {
                HandyControl.Controls.MessageBox.Show("You are not login as user");
                nav.SelectedIndex = 3;
            }
            else
            {
                db.AppUsers.Add(new AppUser
                {
                    UserId = currentUserId,
                    AppId = (int)((Button)sender).Tag,
                });
                await db.SaveChangesAsync();
            }
        }

        private void FiltersChanged(object sender, SelectionChangedEventArgs e)
        {
            if (categoryComboBox.SelectedIndex >= 0)
            {
                UpdateStore();
            }
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            if (asUserRadioBtn.IsChecked == true)
            {
                var res = from user in db.Users
                          where user.Email == loginEmail.Text
                          select user;
                if (res.Count() > 0 && res.First().Password == loginPassword.Password)
                {
                    currentUserId = res.First().Id;
                    HandyControl.Controls.MessageBox.Show("WELCOME!");
                }
                else
                {
                    HandyControl.Controls.MessageBox.Show("Input is incorrect");
                }
            }
            else if (asPublisherRadioBtn.IsChecked == true)
            {
                var res = from publisher in db.Publishers
                          where publisher.Email == loginEmail.Text
                          select publisher;
                if (res.Count() > 0 && res.First().Password == loginPassword.Password)
                {
                    currentPublisherId = res.First().Id;
                    HandyControl.Controls.MessageBox.Show("WELCOME!");
                }
                else
                {
                    HandyControl.Controls.MessageBox.Show("Input is incorrect");
                }
            }
        }

        private void nav_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (nav.SelectedIndex)
            {
                case 1:
                    UpdateLibrary();
                    break;

                case 2:
                    publishLanguageTransfer.ItemsSource = from lang in db.Languages
                                                          select lang;
                    publishAppRadioBtn.IsChecked = true;
                    break;
            }
        }

        private void Publish(object sender, RoutedEventArgs e)
        {
            if (currentPublisherId == 0)
            {
                HandyControl.Controls.MessageBox.Show("You are not login as publisher");
                nav.SelectedIndex = 3;
            }
            else
            {
                double price;
                if (double.TryParse(publishPrice.Text, out price))
                {
                    db.Apps.Add(new StoreDbLibrary.Models.App
                    {
                        Name = publishName.Text,
                        Description = publishDescription.Text,
                        Rating = (int)publishRate.Value,
                        ReleaseDate = publishReleaseDate.DisplayDate,
                        IsGame = publishGameRadioBtn.IsChecked.Value,
                        Price = price,
                        PublisherId = currentPublisherId
                    });
                    db.SaveChanges();

                    int appId = (from app in db.Apps
                                 orderby app.Id descending
                                 select app.Id).FirstOrDefault();

                    foreach (var lang in publishLanguageTransfer.SelectedItems)
                    {
                        db.AppLanguages.Add(new AppLanguage { LanguageId = ((Language)lang).Id, AppId = appId });
                    }
                    foreach (var category in publishCategoryTransfer.SelectedItems)
                    {
                        db.AppCategories.Add(new AppCategory { CategoryId = ((Category)category).Id, AppId = appId });
                    }
                    db.SaveChanges();
                }
                else
                {
                    HandyControl.Controls.MessageBox.Show("Price is incorrect");
                }
            }
        }

        private void publishAppRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            publishCategoryTransfer.ItemsSource = from category in db.Categories
                                                  where category.IsForGame == false
                                                  select category;
        }

        private void publishGameRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            publishCategoryTransfer.ItemsSource = from category in db.Categories
                                                  where category.IsForGame == true
                                                  select category;
        }

        private void RegisterPublisher(object sender, RoutedEventArgs e)
        {
            if (registerPublisherPassword.Password == registerPublisherConfirmPassword.Password && registerPublisherName.Text != "" && registerPublisherEmail.Text != "")
            {
                db.Publishers.Add(new Publisher
                {
                    Name = registerPublisherName.Text,
                    Email = registerPublisherEmail.Text,
                    Description = registerPublisherDescription.Text,
                    Password = registerPublisherPassword.Password
                });
                db.SaveChanges();
            }
            else
            {
                HandyControl.Controls.MessageBox.Show("Input is incorrect");
            }
        }

        private void RegisterUser(object sender, RoutedEventArgs e)
        {
            if (registerUserPassword.Password == registerUserConfirmPassword.Password && registerUserName.Text != "" && registerUserEmail.Text != "")
            {
                db.Users.Add(new User
                {
                    UserName = registerUserName.Text,
                    Email = registerUserEmail.Text,
                    Password = registerUserPassword.Password,
                    BirthDate = registerUserBirthDate.DisplayDate
                });
                db.SaveChanges();
            }
            else
            {
                HandyControl.Controls.MessageBox.Show("Input is incorrect");
            }
        }

        #region Change Theme

        private void ButtonConfig_OnClick(object sender, RoutedEventArgs e) => PopupConfig.IsOpen = true;

        private void ButtonSkins_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button)
            {
                PopupConfig.IsOpen = false;
                if (button.Tag is ApplicationTheme tag)
                {
                    ((App)Application.Current).UpdateTheme(tag);
                }
                else if (button.Tag is Brush accentTag)
                {
                    ((App)Application.Current).UpdateAccent(accentTag);
                }
                else if (button.Tag is "Picker")
                {
                    var picker = SingleOpenHelper.CreateControl<ColorPicker>();
                    var window = new PopupWindow
                    {
                        PopupElement = picker,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        AllowsTransparency = true,
                        WindowStyle = WindowStyle.None,
                        MinWidth = 0,
                        MinHeight = 0,
                        Title = "Select Accent Color"
                    };

                    picker.SelectedColorChanged += delegate
                    {
                        ((App)Application.Current).UpdateAccent(picker.SelectedBrush);
                        window.Close();
                    };
                    picker.Canceled += delegate { window.Close(); };
                    window.Show();
                }
            }
        }

        #endregion Change Theme

        private async void RemoveFromLibrary(object sender, RoutedEventArgs e)
        {
            if (currentUserId == 0)
            {
                HandyControl.Controls.MessageBox.Show("You are not login as user");
                nav.SelectedIndex = 3;
            }
            else
            {
                db.AppUsers.RemoveRange(from appUser in db.AppUsers
                                        where appUser.UserId == currentUserId && appUser.AppId == (int)((Button)sender).Tag
                                        select appUser);
                await db.SaveChangesAsync();
                UpdateLibrary();
            }
        }

        private void typeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (typeComboBox.SelectedIndex)
            {
                case 0:
                    isGame = false;
                    break;

                case 1:
                    isGame = true;
                    break;
            }
            categoryComboBox.ItemsSource = (from category in db.Categories
                                            where category.IsForGame == isGame
                                            select category).ToList();
            categoryComboBox.SelectedIndex = 0;
        }

        private void UpdateLibrary()
        {
            libraryApps.ItemsSource = from appUser in db.AppUsers
                                      where appUser.UserId == currentUserId
                                      select new
                                      {
                                          Id = appUser.App.Id,
                                          Name = appUser.App.Name,
                                          Description = appUser.App.Description,
                                          Rating = appUser.App.Rating,
                                          Price = appUser.App.Price,
                                          Category = appUser.App.AppCategories.First().Category.Name
                                      };
        }

        private void UpdateStore()
        {
            if (!isStart)
            {
                var apps = from appCategory in db.AppCategories
                           join app in db.Apps on appCategory.AppId equals app.Id
                           where app.IsGame == isGame && appCategory.Category.IsForGame == isGame && appCategory.CategoryId == ((Category)categoryComboBox.SelectedItem).Id
                           select new
                           {
                               Id = app.Id,
                               Name = app.Name,
                               Description = app.Description,
                               Rating = app.Rating,
                               Price = app.Price,
                               Category = appCategory.Category.Name,
                               Lang = (from appLanguage in db.AppLanguages
                                       where appLanguage.AppId == app.Id
                                       select new { Name = appLanguage.Language.Name }).ToList()
                           };
                switch (sortComboBox.SelectedIndex)
                {
                    case 0:
                        listViewApps.ItemsSource = apps.OrderBy(app => app.Price);
                        break;

                    case 1:
                        listViewApps.ItemsSource = apps.OrderByDescending(app => app.Price);
                        break;

                    case 2:
                        listViewApps.ItemsSource = apps.OrderBy(app => app.Rating);
                        break;

                    case 3:
                        listViewApps.ItemsSource = apps.OrderByDescending(app => app.Rating);
                        break;
                }
            }
        }
    }
}