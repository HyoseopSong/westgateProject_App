using System;
namespace westgateproject.Helper
{
    public interface ILoginHelper
    {
        void StartLogin();
        void StartSilentLogin();
        void StartLogout();
    }
}
