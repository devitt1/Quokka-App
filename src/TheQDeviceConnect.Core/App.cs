using MvvmCross.IoC;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.ViewModels.Home;

namespace TheQDeviceConnect.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
              .EndingWith("Client")
              .AsInterfaces()
              .RegisterAsLazySingleton();

            RegisterAppStart<HomeViewModel>();
        }
    }
}
