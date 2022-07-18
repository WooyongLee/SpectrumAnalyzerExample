using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace CustomSpectrumAnalyzer
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        // App 에서 사용할 각종 서비스, 뷰모델들을 Injection 하기 위해 미리 등록함
        public App()
        {
            Services = ConfigureServices();
        }

        // Current Instance in use
        public new static App Current => (App)Application.Current;

        // Gets the instance to resolve app. services
        public IServiceProvider Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Transient Service 생성
            services.AddTransient(typeof(SettingViewModel));
            services.AddTransient(typeof(MainViewModel));
            services.AddTransient(typeof(SpectrumViewModel));

            services.AddTransient(typeof(SpectrumCanvasUC));

            return services.BuildServiceProvider();
        }
    }
}
