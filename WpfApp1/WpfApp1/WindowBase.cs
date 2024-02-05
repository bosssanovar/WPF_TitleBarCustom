using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    public abstract class WindowBase : Window
    {
        // Disposeが必要な処理をまとめてやる
        protected CompositeDisposable Disposable { get; } = new();

        // 最小化ボタンが押された時
        public ReactiveCommand WindowMinimum { get; } = new();

        // 最大化、通常サイズのボタンが押された時
        public ReactiveCommand WindowSize { get; } = new();

        // ウインドウを閉じるボタンが押された時
        public ReactiveCommand WindowClose { get; } = new();

        // 最大化、通常サイズのボタンデザイン切り替え
        public ReactivePropertySlim<string> ButtonStyle { get; } = new("1");

        static WindowBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowBase), new FrameworkPropertyMetadata(typeof(WindowBase)));
        }

        public WindowBase()
        {
            this.Closing += WindowBase_Closing;
            this.StateChanged += WindowBase_StateChanged;

            // 最小化ボタンが押された
            WindowMinimum.Subscribe(_ => this.WindowState = WindowState.Minimized).AddTo(Disposable);
            // 最大化、通常サイズボタンが押された
            WindowSize.Subscribe(_ => this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal).AddTo(Disposable);
            // 閉じるボタンが押された
            WindowClose.Subscribe(_ => Window.GetWindow(this).Close()).AddTo(Disposable);
        }

        private void WindowBase_StateChanged(object? sender, EventArgs e)
        {
            ButtonStyle.Value = this.WindowState == WindowState.Normal ? "1" : "2";
        }

        private void WindowBase_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            Disposable.Dispose();
        }
    }
}
