﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Library;
using Library.Net.Amoeba;

namespace Amoeba.Windows
{
    /// <summary>
    /// SearchItemEditWindow.xaml の相互作用ロジック
    /// </summary>
    partial class SearchItemEditWindow : Window
    {
        private SearchItem _searchItem;
        private List<SearchContains<string>> _searchNameCollection;
        private List<SearchContains<string>> _searchNameRegexCollection;
        private List<SearchContains<string>> _searchSignatureCollection;
        private List<SearchContains<string>> _searchKeywordCollection;
        private List<SearchContains<SearchRange<DateTime>>> _searchCreationTimeRangeCollection;
        private List<SearchContains<SearchRange<long>>> _searchLengthRangeCollection;
        private List<SearchContains<Seed>> _searchSeedCollection;

        public SearchItemEditWindow(ref SearchItem searchItem)
        {
            _searchItem = searchItem;

            _searchNameCollection = _searchItem.SearchNameCollection.Select(n => n.DeepClone()).ToList();
            _searchNameRegexCollection = _searchItem.SearchNameRegexCollection.Select(n => n.DeepClone()).ToList();
            _searchSignatureCollection = _searchItem.SearchSignatureCollection.Select(n => n.DeepClone()).ToList();
            _searchKeywordCollection = _searchItem.SearchKeywordCollection.Select(n => n.DeepClone()).ToList();
            _searchCreationTimeRangeCollection = _searchItem.SearchCreationTimeRangeCollection.Select(n => n.DeepClone()).ToList();
            _searchLengthRangeCollection = _searchItem.SearchLengthRangeCollection.Select(n => n.DeepClone()).ToList();
            _searchSeedCollection = _searchItem.SearchSeedCollection.Select(n => n.DeepClone()).ToList();

            InitializeComponent();

            using (FileStream stream = new FileStream(System.IO.Path.Combine(App.DirectoryPaths["Icons"], "Amoeba.ico"), FileMode.Open))
            {
                this.Icon = BitmapFrame.Create(stream);
            }

            _nameContainsRadioButton.IsChecked = true;
            _nameRegexContainsRadioButton.IsChecked = true;
            _signatureContainsRadioButton.IsChecked = true;
            _keywordContainsRadioButton.IsChecked = true;
            _creationTimeRangeContainsRadioButton.IsChecked = true;
            _lengthRangeContainsRadioButton.IsChecked = true;
            _seedContainsRadioButton.IsChecked = true;

            _nameListView.ItemsSource = _searchNameCollection;
            _nameRegexListView.ItemsSource = _searchNameRegexCollection;
            _signatureListView.ItemsSource = _searchSignatureCollection;
            _keywordListView.ItemsSource = _searchKeywordCollection;
            _creationTimeRangeListView.ItemsSource = _searchCreationTimeRangeCollection;
            _lengthRangeListView.ItemsSource = _searchLengthRangeCollection;
            _seedListView.ItemsSource = _searchSeedCollection;

            _searchTreeViewItemNameTextBox.Text = _searchItem.Name;

            _creationTimeRangeMinTextBox.Text = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            _creationTimeRangeMaxTextBox.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        private void _okButton_Click(object sender, RoutedEventArgs e)
        {
            _searchItem.Name = _searchTreeViewItemNameTextBox.Text;

            _searchItem.SearchNameCollection.Clear();
            _searchItem.SearchNameCollection.AddRange(_searchNameCollection.Select(n => n.DeepClone()).ToList());
            _searchItem.SearchNameRegexCollection.Clear();
            _searchItem.SearchNameRegexCollection.AddRange(_searchNameRegexCollection.Select(n => n.DeepClone()).ToList());
            _searchItem.SearchSignatureCollection.Clear();
            _searchItem.SearchSignatureCollection.AddRange(_searchSignatureCollection.Select(n => n.DeepClone()).ToList());
            _searchItem.SearchKeywordCollection.Clear();
            _searchItem.SearchKeywordCollection.AddRange(_searchKeywordCollection.Select(n => n.DeepClone()).ToList());
            _searchItem.SearchCreationTimeRangeCollection.Clear();
            _searchItem.SearchCreationTimeRangeCollection.AddRange(_searchCreationTimeRangeCollection.Select(n => n.DeepClone()).ToList());
            _searchItem.SearchLengthRangeCollection.Clear();
            _searchItem.SearchLengthRangeCollection.AddRange(_searchLengthRangeCollection.Select(n => n.DeepClone()).ToList());
            _searchItem.SearchSeedCollection.Clear();
            _searchItem.SearchSeedCollection.AddRange(_searchSeedCollection.Select(n => n.DeepClone()).ToList());

            this.DialogResult = true;
        }

        private void _cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #region _nameListView

        private void _nameListViewUpdate()
        {
            _nameListView_SelectionChanged(this, null);
        }

        private void _nameListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectIndex = _nameListView.SelectedIndex;

                if (selectIndex == -1)
                {
                    _nameUpButton.IsEnabled = false;
                    _nameDownButton.IsEnabled = false;
                }
                else
                {
                    if (selectIndex == 0)
                    {
                        _nameUpButton.IsEnabled = false;
                    }
                    else
                    {
                        _nameUpButton.IsEnabled = true;
                    }

                    if (selectIndex == _searchNameCollection.Count - 1)
                    {
                        _nameDownButton.IsEnabled = false;
                    }
                    else
                    {
                        _nameDownButton.IsEnabled = true;
                    }
                }

                _nameListView_PreviewMouseLeftButtonDown(this, null);
            }
            catch (Exception)
            {

            }
        }

        private void _nameListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = _nameListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            _nameContainsRadioButton.IsChecked = item.Contains;
            _nameNotContainsRadioButton.IsChecked = !item.Contains;
            _nameTextBox.Text = item.Value;
        }

        private void _nameUpButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _nameListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            var selectIndex = _nameListView.SelectedIndex;
            if (selectIndex == -1) return;

            _searchNameCollection.Remove(item);
            _searchNameCollection.Insert(selectIndex - 1, item);
            _nameListView.Items.Refresh();

            _nameListViewUpdate();
        }

        private void _nameDownButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _nameListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            var selectIndex = _nameListView.SelectedIndex;
            if (selectIndex == -1) return;

            _searchNameCollection.Remove(item);
            _searchNameCollection.Insert(selectIndex + 1, item);
            _nameListView.Items.Refresh();

            _nameListViewUpdate();
        }

        private void _nameAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_nameTextBox.Text == "") return;

            var item = new SearchContains<string>()
            {
                Contains = _nameContainsRadioButton.IsChecked.Value,
                Value = _nameTextBox.Text,
            };

            if (_searchNameCollection.Contains(item)) return;
            _searchNameCollection.Add(item);

            _nameListView.Items.Refresh();
            _nameListViewUpdate();

            _nameTextBox.Text = "";
        }

        private void _nameEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (_nameTextBox.Text == "") return;

            var item = _nameListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            item.Contains = _nameContainsRadioButton.IsChecked.Value;
            item.Value = _nameTextBox.Text;

            _nameListView.Items.Refresh();
            _nameListViewUpdate();
        }

        private void _nameDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _nameListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            int selectIndex = _nameListView.SelectedIndex;
            _searchNameCollection.Remove(item);
            _nameListView.Items.Refresh();
            _nameListView.SelectedIndex = selectIndex;
            _nameListViewUpdate();

            _nameTextBox.Text = "";
        }

        #endregion

        #region _nameRegexListView

        private void _nameRegexListViewUpdate()
        {
            _nameRegexListView_SelectionChanged(this, null);
        }

        private void _nameRegexListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectIndex = _nameRegexListView.SelectedIndex;

                if (selectIndex == -1)
                {
                    _nameRegexUpButton.IsEnabled = false;
                    _nameRegexDownButton.IsEnabled = false;
                }
                else
                {
                    if (selectIndex == 0)
                    {
                        _nameRegexUpButton.IsEnabled = false;
                    }
                    else
                    {
                        _nameRegexUpButton.IsEnabled = true;
                    }

                    if (selectIndex == _searchNameRegexCollection.Count - 1)
                    {
                        _nameRegexDownButton.IsEnabled = false;
                    }
                    else
                    {
                        _nameRegexDownButton.IsEnabled = true;
                    }
                }

                _nameRegexListView_PreviewMouseLeftButtonDown(this, null);
            }
            catch (Exception)
            {

            }
        }

        private void _nameRegexListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = _nameRegexListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            _nameRegexContainsRadioButton.IsChecked = item.Contains;
            _nameRegexNotContainsRadioButton.IsChecked = !item.Contains;
            _nameRegexTextBox.Text = item.Value;
        }

        private void _nameRegexUpButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _nameRegexListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            var selectIndex = _nameRegexListView.SelectedIndex;
            if (selectIndex == -1) return;

            _searchNameRegexCollection.Remove(item);
            _searchNameRegexCollection.Insert(selectIndex - 1, item);
            _nameRegexListView.Items.Refresh();

            _nameRegexListViewUpdate();
        }

        private void _nameRegexDownButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _nameRegexListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            var selectIndex = _nameRegexListView.SelectedIndex;
            if (selectIndex == -1) return;

            _searchNameRegexCollection.Remove(item);
            _searchNameRegexCollection.Insert(selectIndex + 1, item);
            _nameRegexListView.Items.Refresh();

            _nameRegexListViewUpdate();
        }

        private void _nameRegexAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_nameRegexTextBox.Text == "") return;

            try
            {
                new Regex(_nameRegexTextBox.Text);
            }
            catch (Exception)
            {
                return;
            }

            var item = new SearchContains<string>()
            {
                Contains = _nameRegexContainsRadioButton.IsChecked.Value,
                Value = _nameRegexTextBox.Text,
            };

            if (_searchNameRegexCollection.Contains(item)) return;
            _searchNameRegexCollection.Add(item);

            _nameRegexListView.Items.Refresh();
            _nameRegexListViewUpdate();

            _nameRegexTextBox.Text = "";
        }

        private void _nameRegexEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (_nameRegexTextBox.Text == "") return;

            try
            {
                new Regex(_nameRegexTextBox.Text);
            }
            catch (Exception)
            {
                return;
            }

            var item = _nameRegexListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            item.Contains = _nameRegexContainsRadioButton.IsChecked.Value;
            item.Value = _nameRegexTextBox.Text;

            _nameRegexListView.Items.Refresh();
            _nameRegexListViewUpdate();
        }

        private void _nameRegexDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _nameRegexListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            int selectIndex = _nameRegexListView.SelectedIndex;
            _searchNameRegexCollection.Remove(item);
            _nameRegexListView.Items.Refresh();
            _nameRegexListView.SelectedIndex = selectIndex;
            _nameRegexListViewUpdate();

            _nameRegexTextBox.Text = "";
        }

        #endregion

        #region _signatureListView

        private void _signatureListViewUpdate()
        {
            _signatureListView_SelectionChanged(this, null);
        }

        private void _signatureListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectIndex = _signatureListView.SelectedIndex;

                if (selectIndex == -1)
                {
                    _signatureUpButton.IsEnabled = false;
                    _signatureDownButton.IsEnabled = false;
                }
                else
                {
                    if (selectIndex == 0)
                    {
                        _signatureUpButton.IsEnabled = false;
                    }
                    else
                    {
                        _signatureUpButton.IsEnabled = true;
                    }

                    if (selectIndex == _searchSignatureCollection.Count - 1)
                    {
                        _signatureDownButton.IsEnabled = false;
                    }
                    else
                    {
                        _signatureDownButton.IsEnabled = true;
                    }
                }

                _signatureListView_PreviewMouseLeftButtonDown(this, null);
            }
            catch (Exception)
            {

            }
        }

        private void _signatureListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = _signatureListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            _signatureContainsRadioButton.IsChecked = item.Contains;
            _signatureNotContainsRadioButton.IsChecked = !item.Contains;
            _signatureTextBox.Text = item.Value;
        }

        private void _signatureUpButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _signatureListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            var selectIndex = _signatureListView.SelectedIndex;
            if (selectIndex == -1) return;

            _searchSignatureCollection.Remove(item);
            _searchSignatureCollection.Insert(selectIndex - 1, item);
            _signatureListView.Items.Refresh();

            _signatureListViewUpdate();
        }

        private void _signatureDownButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _signatureListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            var selectIndex = _signatureListView.SelectedIndex;
            if (selectIndex == -1) return;

            _searchSignatureCollection.Remove(item);
            _searchSignatureCollection.Insert(selectIndex + 1, item);
            _signatureListView.Items.Refresh();

            _signatureListViewUpdate();
        }

        private void _signatureAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_signatureTextBox.Text == "") return;

            var item = new SearchContains<string>()
            {
                Contains = _signatureContainsRadioButton.IsChecked.Value,
                Value = _signatureTextBox.Text,
            };

            if (_searchSignatureCollection.Contains(item)) return;
            _searchSignatureCollection.Add(item);

            _signatureListView.Items.Refresh();
            _signatureListViewUpdate();

            _signatureTextBox.Text = "";
        }

        private void _signatureEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (_signatureTextBox.Text == "") return;

            var item = _signatureListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            item.Contains = _signatureContainsRadioButton.IsChecked.Value;
            item.Value = _signatureTextBox.Text;

            _signatureListView.Items.Refresh();
            _signatureListViewUpdate();
        }

        private void _signatureDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _signatureListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            int selectIndex = _signatureListView.SelectedIndex;
            _searchSignatureCollection.Remove(item);
            _signatureListView.Items.Refresh();
            _signatureListView.SelectedIndex = selectIndex;
            _signatureListViewUpdate();

            _signatureTextBox.Text = "";
        }

        #endregion

        #region _keywordListView

        private void _keywordListViewUpdate()
        {
            _keywordListView_SelectionChanged(this, null);
        }

        private void _keywordListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectIndex = _keywordListView.SelectedIndex;

                if (selectIndex == -1)
                {
                    _keywordUpButton.IsEnabled = false;
                    _keywordDownButton.IsEnabled = false;
                }
                else
                {
                    if (selectIndex == 0)
                    {
                        _keywordUpButton.IsEnabled = false;
                    }
                    else
                    {
                        _keywordUpButton.IsEnabled = true;
                    }

                    if (selectIndex == _searchKeywordCollection.Count - 1)
                    {
                        _keywordDownButton.IsEnabled = false;
                    }
                    else
                    {
                        _keywordDownButton.IsEnabled = true;
                    }
                }

                _keywordListView_PreviewMouseLeftButtonDown(this, null);
            }
            catch (Exception)
            {

            }
        }

        private void _keywordListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = _keywordListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            _keywordContainsRadioButton.IsChecked = item.Contains;
            _keywordNotContainsRadioButton.IsChecked = !item.Contains;
            _keywordTextBox.Text = item.Value;
        }

        private void _keywordUpButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _keywordListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            var selectIndex = _keywordListView.SelectedIndex;
            if (selectIndex == -1) return;

            _searchKeywordCollection.Remove(item);
            _searchKeywordCollection.Insert(selectIndex - 1, item);
            _keywordListView.Items.Refresh();

            _keywordListViewUpdate();
        }

        private void _keywordDownButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _keywordListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            var selectIndex = _keywordListView.SelectedIndex;
            if (selectIndex == -1) return;

            _searchKeywordCollection.Remove(item);
            _searchKeywordCollection.Insert(selectIndex + 1, item);
            _keywordListView.Items.Refresh();

            _keywordListViewUpdate();
        }

        private void _keywordAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_keywordTextBox.Text == "") return;

            var item = new SearchContains<string>()
            {
                Contains = _keywordContainsRadioButton.IsChecked.Value,
                Value = _keywordTextBox.Text,
            };

            if (_searchKeywordCollection.Contains(item)) return;
            _searchKeywordCollection.Add(item);

            _keywordListView.Items.Refresh();
            _keywordListViewUpdate();

            _keywordTextBox.Text = "";
        }

        private void _keywordEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (_keywordTextBox.Text == "") return;

            var item = _keywordListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            item.Contains = _keywordContainsRadioButton.IsChecked.Value;
            item.Value = _keywordTextBox.Text;

            _keywordListView.Items.Refresh();
            _keywordListViewUpdate();
        }

        private void _keywordDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _keywordListView.SelectedItem as SearchContains<string>;
            if (item == null) return;

            int selectIndex = _keywordListView.SelectedIndex;
            _searchKeywordCollection.Remove(item);
            _keywordListView.Items.Refresh();
            _keywordListView.SelectedIndex = selectIndex;
            _keywordListViewUpdate();

            _keywordTextBox.Text = "";
        }

        #endregion

        #region _creationTimeRangeListView

        private void _creationTimeRangeListViewUpdate()
        {
            _creationTimeRangeListView_SelectionChanged(this, null);
        }

        private void _creationTimeRangeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectIndex = _creationTimeRangeListView.SelectedIndex;

                if (selectIndex == -1)
                {
                    _creationTimeRangeUpButton.IsEnabled = false;
                    _creationTimeRangeDownButton.IsEnabled = false;
                }
                else
                {
                    if (selectIndex == 0)
                    {
                        _creationTimeRangeUpButton.IsEnabled = false;
                    }
                    else
                    {
                        _creationTimeRangeUpButton.IsEnabled = true;
                    }

                    if (selectIndex == _searchCreationTimeRangeCollection.Count - 1)
                    {
                        _creationTimeRangeDownButton.IsEnabled = false;
                    }
                    else
                    {
                        _creationTimeRangeDownButton.IsEnabled = true;
                    }
                }

                _creationTimeRangeListView_PreviewMouseLeftButtonDown(this, null);
            }
            catch (Exception)
            {

            }
        }

        private void _creationTimeRangeListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = _creationTimeRangeListView.SelectedItem as SearchContains<SearchRange<DateTime>>;
            if (item == null) return;

            _creationTimeRangeContainsRadioButton.IsChecked = item.Contains;
            _creationTimeRangeNotContainsRadioButton.IsChecked = !item.Contains;
            _creationTimeRangeMaxTextBox.Text = item.Value.Max.ToUniversalTime().ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            _creationTimeRangeMinTextBox.Text = item.Value.Min.ToUniversalTime().ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        private void _creationTimeRangeUpButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _creationTimeRangeListView.SelectedItem as SearchContains<SearchRange<DateTime>>;
            if (item == null) return;

            var selectIndex = _creationTimeRangeListView.SelectedIndex;
            if (selectIndex == -1) return;

            _searchCreationTimeRangeCollection.Remove(item);
            _searchCreationTimeRangeCollection.Insert(selectIndex - 1, item);
            _creationTimeRangeListView.Items.Refresh();

            _creationTimeRangeListViewUpdate();
        }

        private void _creationTimeRangeDownButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _creationTimeRangeListView.SelectedItem as SearchContains<SearchRange<DateTime>>;
            if (item == null) return;

            var selectIndex = _creationTimeRangeListView.SelectedIndex;
            if (selectIndex == -1) return;

            _searchCreationTimeRangeCollection.Remove(item);
            _searchCreationTimeRangeCollection.Insert(selectIndex + 1, item);
            _creationTimeRangeListView.Items.Refresh();

            _creationTimeRangeListViewUpdate();
        }

        private void _creationTimeRangeAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_creationTimeRangeMaxTextBox.Text == "") return;
            if (_creationTimeRangeMinTextBox.Text == "") return;

            try
            {
                var item = new SearchContains<SearchRange<DateTime>>()
                {
                    Contains = _creationTimeRangeContainsRadioButton.IsChecked.Value,
                    Value = new SearchRange<DateTime>()
                    {
                        Max = DateTime.ParseExact(_creationTimeRangeMaxTextBox.Text, "yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.AssumeUniversal).ToUniversalTime(),
                        Min = DateTime.ParseExact(_creationTimeRangeMinTextBox.Text, "yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.AssumeUniversal).ToUniversalTime(),
                    }
                };

                if (_searchCreationTimeRangeCollection.Contains(item)) return;
                _searchCreationTimeRangeCollection.Add(item);
            }
            catch (FormatException)
            {

            }

            _creationTimeRangeListView.Items.Refresh();
            _creationTimeRangeListViewUpdate();

            _creationTimeRangeMinTextBox.Text = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            _creationTimeRangeMaxTextBox.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        private void _creationTimeRangeEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (_creationTimeRangeMaxTextBox.Text == "") return;
            if (_creationTimeRangeMinTextBox.Text == "") return;

            var item = _creationTimeRangeListView.SelectedItem as SearchContains<SearchRange<DateTime>>;
            if (item == null) return;

            item.Contains = _creationTimeRangeContainsRadioButton.IsChecked.Value;
            item.Value.Max = DateTime.ParseExact(_creationTimeRangeMaxTextBox.Text, "yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.AssumeUniversal).ToUniversalTime();
            item.Value.Min = DateTime.ParseExact(_creationTimeRangeMinTextBox.Text, "yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.AssumeUniversal).ToUniversalTime();

            _creationTimeRangeListView.Items.Refresh();
            _creationTimeRangeListViewUpdate();
        }

        private void _creationTimeRangeDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _creationTimeRangeListView.SelectedItem as SearchContains<SearchRange<DateTime>>;
            if (item == null) return;

            int selectIndex = _creationTimeRangeListView.SelectedIndex;
            _searchCreationTimeRangeCollection.Remove(item);
            _creationTimeRangeListView.Items.Refresh();
            _creationTimeRangeListView.SelectedIndex = selectIndex;
            _creationTimeRangeListViewUpdate();

            _creationTimeRangeMinTextBox.Text = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            _creationTimeRangeMaxTextBox.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        #endregion

        #region _lengthRangeListView

        private void _lengthRangeListViewUpdate()
        {
            _lengthRangeListView_SelectionChanged(this, null);
        }

        private void _lengthRangeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectIndex = _lengthRangeListView.SelectedIndex;

                if (selectIndex == -1)
                {
                    _lengthRangeUpButton.IsEnabled = false;
                    _lengthRangeDownButton.IsEnabled = false;
                }
                else
                {
                    if (selectIndex == 0)
                    {
                        _lengthRangeUpButton.IsEnabled = false;
                    }
                    else
                    {
                        _lengthRangeUpButton.IsEnabled = true;
                    }

                    if (selectIndex == _searchLengthRangeCollection.Count - 1)
                    {
                        _lengthRangeDownButton.IsEnabled = false;
                    }
                    else
                    {
                        _lengthRangeDownButton.IsEnabled = true;
                    }
                }

                _lengthRangeListView_PreviewMouseLeftButtonDown(this, null);
            }
            catch (Exception)
            {

            }
        }

        private void _lengthRangeListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = _lengthRangeListView.SelectedItem as SearchContains<SearchRange<long>>;
            if (item == null) return;

            _lengthRangeContainsRadioButton.IsChecked = item.Contains;
            _lengthRangeNotContainsRadioButton.IsChecked = !item.Contains;
            _lengthRangeMaxTextBox.Text = item.Value.Max.ToString();
            _lengthRangeMinTextBox.Text = item.Value.Min.ToString();
        }

        private void _lengthRangeUpButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _lengthRangeListView.SelectedItem as SearchContains<SearchRange<long>>;
            if (item == null) return;

            var selectIndex = _lengthRangeListView.SelectedIndex;
            if (selectIndex == -1) return;

            _searchLengthRangeCollection.Remove(item);
            _searchLengthRangeCollection.Insert(selectIndex - 1, item);
            _lengthRangeListView.Items.Refresh();

            _lengthRangeListViewUpdate();
        }

        private void _lengthRangeDownButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _lengthRangeListView.SelectedItem as SearchContains<SearchRange<long>>;
            if (item == null) return;

            var selectIndex = _lengthRangeListView.SelectedIndex;
            if (selectIndex == -1) return;

            _searchLengthRangeCollection.Remove(item);
            _searchLengthRangeCollection.Insert(selectIndex + 1, item);
            _lengthRangeListView.Items.Refresh();

            _lengthRangeListViewUpdate();
        }

        private void _lengthRangeAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_lengthRangeMaxTextBox.Text == "") return;
            if (_lengthRangeMinTextBox.Text == "") return;

            try
            {
                var item = new SearchContains<SearchRange<long>>()
                {
                    Contains = _lengthRangeContainsRadioButton.IsChecked.Value,
                    Value = new SearchRange<long>()
                    {
                        Max = long.Parse(_lengthRangeMaxTextBox.Text),
                        Min = long.Parse(_lengthRangeMinTextBox.Text),
                    }
                };

                if (_searchLengthRangeCollection.Contains(item)) return;
                _searchLengthRangeCollection.Add(item);
            }
            catch (FormatException)
            {

            }

            _lengthRangeListView.Items.Refresh();
            _lengthRangeListViewUpdate();

            _lengthRangeMaxTextBox.Text = "";
            _lengthRangeMinTextBox.Text = "";
        }

        private void _lengthRangeEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (_lengthRangeMaxTextBox.Text == "") return;
            if (_lengthRangeMinTextBox.Text == "") return;

            var item = _lengthRangeListView.SelectedItem as SearchContains<SearchRange<long>>;
            if (item == null) return;

            item.Contains = _lengthRangeContainsRadioButton.IsChecked.Value;
            item.Value.Max = long.Parse(_lengthRangeMaxTextBox.Text);
            item.Value.Min = long.Parse(_lengthRangeMinTextBox.Text);

            _lengthRangeListView.Items.Refresh();
            _lengthRangeListViewUpdate();
        }

        private void _lengthRangeDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _lengthRangeListView.SelectedItem as SearchContains<SearchRange<long>>;
            if (item == null) return;

            int selectIndex = _lengthRangeListView.SelectedIndex;
            _searchLengthRangeCollection.Remove(item);
            _lengthRangeListView.Items.Refresh();
            _lengthRangeListView.SelectedIndex = selectIndex;
            _lengthRangeListViewUpdate();

            _lengthRangeMaxTextBox.Text = "";
            _lengthRangeMinTextBox.Text = "";
        }

        #endregion

        #region _seedListView

        private void _seedListViewUpdate()
        {
            _seedListView_SelectionChanged(this, null);
        }

        private void _seedListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectIndex = _seedListView.SelectedIndex;

                if (selectIndex == -1)
                {
                    _seedUpButton.IsEnabled = false;
                    _seedDownButton.IsEnabled = false;
                }
                else
                {
                    if (selectIndex == 0)
                    {
                        _seedUpButton.IsEnabled = false;
                    }
                    else
                    {
                        _seedUpButton.IsEnabled = true;
                    }

                    if (selectIndex == _searchSeedCollection.Count - 1)
                    {
                        _seedDownButton.IsEnabled = false;
                    }
                    else
                    {
                        _seedDownButton.IsEnabled = true;
                    }
                }

                _seedListView_PreviewMouseLeftButtonDown(this, null);
            }
            catch (Exception)
            {

            }
        }

        private void _seedListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = _seedListView.SelectedItem as SearchContains<Seed>;
            if (item == null) return;

            _seedContainsRadioButton.IsChecked = item.Contains;
            _seedNotContainsRadioButton.IsChecked = !item.Contains;
            _seedTextBox.Text = AmoebaConverter.ToSeedString(item.Value);
            _seedNameTextBox.Text = string.Format("{0}, {1:#,0}", item.Value.Name, item.Value.Length);
        }

        private void _seedUpButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _seedListView.SelectedItem as SearchContains<Seed>;
            if (item == null) return;

            var selectIndex = _seedListView.SelectedIndex;
            if (selectIndex == -1) return;

            _searchSeedCollection.Remove(item);
            _searchSeedCollection.Insert(selectIndex - 1, item);
            _seedListView.Items.Refresh();

            _seedListViewUpdate();
        }

        private void _seedDownButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _seedListView.SelectedItem as SearchContains<Seed>;
            if (item == null) return;

            var selectIndex = _seedListView.SelectedIndex;
            if (selectIndex == -1) return;

            _searchSeedCollection.Remove(item);
            _searchSeedCollection.Insert(selectIndex + 1, item);
            _seedListView.Items.Refresh();

            _seedListViewUpdate();
        }

        private void _seedAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_seedTextBox.Text == "") return;

            try
            {
                var item = new SearchContains<Seed>()
                {
                    Contains = _seedContainsRadioButton.IsChecked.Value,
                    Value = AmoebaConverter.FromSeedString(_seedTextBox.Text),
                };

                if (_searchSeedCollection.Contains(item)) return;
                _searchSeedCollection.Add(item);
            }
            catch (Exception)
            {

            }

            _seedListView.Items.Refresh();
            _seedListViewUpdate();

            _seedTextBox.Text = "";
        }

        private void _seedEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (_seedTextBox.Text == "") return;

            var item = _seedListView.SelectedItem as SearchContains<Seed>;
            if (item == null) return;

            try
            {
                item.Contains = _seedContainsRadioButton.IsChecked.Value;
                item.Value = AmoebaConverter.FromSeedString(_seedTextBox.Text);
            }
            catch (Exception)
            {

            }

            _seedListView.Items.Refresh();
            _seedListViewUpdate();
        }

        private void _seedDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var item = _seedListView.SelectedItem as SearchContains<Seed>;
            if (item == null) return;

            int selectIndex = _seedListView.SelectedIndex;
            _searchSeedCollection.Remove(item);
            _seedListView.Items.Refresh();
            _seedListView.SelectedIndex = selectIndex;
            _seedListViewUpdate();

            _seedTextBox.Text = "";
        }

        #endregion
    }
}