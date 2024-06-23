using H.Modules.Messages.Dialog;
using H.Providers.Mvvm;
using System.Windows;
using H.Providers.Ioc;
using H.Controls.Form;
using System.Text.RegularExpressions;
using System;
using H.Data.Test;
using H.Modules.Messages.Form;
using H.Modules.Messages.Notice;

namespace H.Test.Demo2
{
    internal class MainViewModel : Bindable
    {
        private Students _students = Student.Randoms(100);
        public Students Students
        {
            get { return _students; }
            set
            {
                _students = value;
                RaisePropertyChanged();
            }
        }
        public RelayCommand ShowAdornerCommand => new RelayCommand(async (s, e) =>
        {
            var r = await AdornerDialog.ShowPresenter("我是AdornerDialog", x =>
            {
                x.DialogButton = DialogButton.SumitAndCancel;
                x.Height = 100;
                x.VerticalContentAlignment = VerticalAlignment.Top;
                x.Padding = new Thickness(20);
            });
            if (r == true)
                System.Diagnostics.Debug.WriteLine("点击了确定");
            if (r == null)
                System.Diagnostics.Debug.WriteLine("点击了关闭按钮");
            if (r == true)
                System.Diagnostics.Debug.WriteLine("点击了取消");
        });

        public RelayCommand ShowFormCommand => new RelayCommand(async (s, e) =>
        {
            Student student = new Student();
            Func<bool> canSumit = () =>
            {
                if (student.ModelStateDeep(out string error) == false)
                {
                    MessageBox.Show(error);
                    return false;
                }
                return true;
            };
            StaticFormPresenter presenter = new StaticFormPresenter(student);
            presenter.UsePropertyView = true;
            await AdornerDialog.ShowPresenter(presenter, x =>
            {
                x.DialogButton = DialogButton.None;
                x.Title = "Form Dailog";
            }, canSumit);
        });

        private NoticeMessageService _noticeMessageService = new NoticeMessageService();
        public RelayCommand ShowInfoNoticeCommand => new RelayCommand(async (s, e) =>
        {
            _noticeMessageService.ShowInfo("提示信息");
        });
        public RelayCommand ShowErrorNoticeCommand => new RelayCommand(async (s, e) =>
        {
            _noticeMessageService.ShowError("错误信息");
        });
        public RelayCommand ShowSuccessNoticeCommand => new RelayCommand(async (s, e) =>
        {
            _noticeMessageService.ShowSuccess("成功提示");
        });
        public RelayCommand ShowWarnNoticeCommand => new RelayCommand(async (s, e) =>
        {
            _noticeMessageService.ShowWarn("警告提示");
        });
        public RelayCommand ShowFatalNoticeCommand => new RelayCommand(async (s, e) =>
        {
            _noticeMessageService.ShowFatal("严重错误提示");
        });

        public RelayCommand ShowProgressNoticeCommand => new RelayCommand(async (s, e) =>
        {
            Func<IPercentNoticeItem, bool> action = x =>
            {
                for (int i = 0; i < 100; i++)
                {
                    x.Value = i;
                    x.Message = $"{x.Value}/100";
                    Thread.Sleep(20);
                }
                x.Value = 100;
                x.Message = $"{x.Value}/100";
                Thread.Sleep(1000);
                return true;
            };
            await _noticeMessageService.ShowProgress(action);
        });

        public RelayCommand ShowStringNoticeCommand => new RelayCommand(async (s, e) =>
        {
            Func<INoticeItem, bool> action = x =>
            {
                for (int i = 0; i < 100; i++)
                {
                    x.Message = $"{i}/100";
                    Thread.Sleep(20);
                }
                x.Message = $"100/100";
                Thread.Sleep(1000);
                return true;
            };
            await _noticeMessageService.ShowString(action);
        });

        public RelayCommand ShowDialogNoticeCommand => new RelayCommand(async (s, e) =>
        {
            bool? r = await _noticeMessageService.ShowDialog("对话框提示");
            if (r == true)
                _noticeMessageService.ShowSuccess("点击了确定");
            else
                _noticeMessageService.ShowError("点击了取消");
        });

        public RelayCommand ShowNoticeCommand => new RelayCommand(async (s, e) =>
        {
            var notice = new MyNotice() { Message = "HeBianGu" };
            _noticeMessageService.Show(notice);
        });
    }

    public class MyNotice : INoticeItem
    {
        public string Message { get; set; }

        public string Time { get; }
    }
}