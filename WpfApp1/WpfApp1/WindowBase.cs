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
        private CompositeDisposable Disposable { get; } = new();

        // 最小化ボタンが押された時
        public ReactiveCommand WindowMinimum { get; } = new();
        // 最大化、通常サイズのボタンが押された時
        public ReactiveCommand WindowSize { get; } = new();
        // ウインドウを閉じるボタンが押された時
        public ReactiveCommand WindowClose { get; } = new();
        // ウインドウのサイズが変更されるイベントが発生した時(最大化と通常の変化)
        public ReactiveCommand SizeChangedCommand { get; } = new();

        static WindowBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowBase), new FrameworkPropertyMetadata(typeof(WindowBase)));
        }

        public WindowBase()
        {
            this.Closing += WindowBase_Closing;

            // 最小化ボタンが押された
            _ = WindowMinimum.Subscribe(_ => this.WindowState = WindowState.Minimized).AddTo(Disposable);
            // 最大化、通常サイズボタンが押された
            _ = WindowSize.Subscribe(_ => this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal).AddTo(Disposable);
            // 閉じるボタンが押された
            _ = WindowClose.Subscribe(_ => Window.GetWindow(this).Close()).AddTo(Disposable);
            // ウィンドウサイズが変わった
            _ = SizeChangedCommand.Subscribe(_ =>
            {
            }).AddTo(Disposable);
        }

        private void WindowBase_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            Disposable.Dispose();
        }
    }
}
